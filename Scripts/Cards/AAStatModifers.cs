using AALUND13Cards.Armors;
using AALUND13Cards.Extensions;
using AALUND13Cards.Handlers;
using AALUND13Cards.Handlers.ExtraPickHandlers;
using AALUND13Cards.MonoBehaviours.CardsEffects.Soulstreak;
using JARL.Armor;
using JARL.Armor.Bases;
using JARL.Bases;
using RarityLib.Utils;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnboundLib;
using UnityEngine;

namespace AALUND13Cards.Cards {
    public enum ExtraPicksType {
        None,
        Normal,
        Steel
    }

    public enum ArmorType {
        Battleforged,
        Titanium,
        None
    }

    public class AAStatModifers : MonoBehaviour {
        #region Soulstreak Stats
        [Header("Soulstreak Stats")]

        public float MaxHealth = 0;
        public float PlayerSize = 0;
        public float MovementSpeed = 0;

        public float AttackSpeed = 0;
        public float Damage = 0;
        public float BulletSpeed = 0;

        public float SoulArmorPercentage = 0;
        public float SoulArmorPercentageRegenRate = 0;

        public float SoulDrainDamageMultiply = 0;
        public float SoulDrainLifestealMultiply = 0;

        public AbilityType AbilityType;
        #endregion

        #region Railgun Stats
        [Header("Railgun Stats Add")]
        public float MaximumCharge = 0f;
        public float ChargeRate = 0f;
        public float FullChargeThreshold = 0f;

        [Header("Railgun Stats Multiplier")]
        public float MaximumChargeMultiplier = 1f;
        public float ChargeRateMultiplier = 1f;

        public float RailgunDamageMultiplier = 1f;
        public float RailgunBulletSpeedMultiplier = 1f;
        #endregion

        [Header("Uncategorized Stats")]
        public float SecondToDealDamage = 0;

        [Header("Curses Stats")]
        public bool SetMaxRarityForCurse = false;
        public CardRarity MaxRarityForCurse;
        public bool IsBind = false;

        [Header("Blocks Stats")]
        public int BlocksWhenRecharge = 0;
        public float BlockPircePercent = 0f;

        [Header("Armors Stats")]
        public float ArmorDamageReduction = 0f;

        [Space(10)]
        public ArmorType Armor = ArmorType.None;
        public float ArmorHealth = 0f;
        public float ArmorRegenRate = 0f;
        public float ArmorRegenCooldown = 0f;
        public ArmorReactivateType ArmorReactivateType = ArmorReactivateType.Percent;
        public float ArmorReactivateValue = 0f;

        [Header("Armor Pierce Stats")]
        public float ArmorPiercePercent = 0f;
        public float DamageAgainstArmorPercentage = 1f;

        [Header("Extra Picks")]
        public int ExtraPicks = 0;
        public ExtraPicksType ExtraPicksType;
        public int ExtraPicksForEnemies = 0;
        public ExtraPicksType ExtraPicksTypeForEnemies;


        [Header("Extra Cards")]
        public int ExtraCardPicks = 0;
        public int DuplicatesAsCorrupted = 0;

        public void Apply(Player player) {
            CharacterData data = player.data;
            var additionalData = data.GetAdditionalData();
            var jarlAdditionalData = JARL.Extensions.CharacterDataExtensions.GetAdditionalData(data);

            #region Soulstreak Stats
            additionalData.SoulStreakStats.MaxHealth += MaxHealth;
            additionalData.SoulStreakStats.PlayerSize += PlayerSize;
            additionalData.SoulStreakStats.MovementSpeed += MovementSpeed;

            additionalData.SoulStreakStats.AttackSpeed += AttackSpeed;
            additionalData.SoulStreakStats.Damage += Damage;
            additionalData.SoulStreakStats.BulletSpeed += BulletSpeed;

            additionalData.SoulStreakStats.SoulArmorPercentage += SoulArmorPercentage;
            additionalData.SoulStreakStats.SoulArmorPercentageRegenRate += SoulArmorPercentageRegenRate;

            additionalData.SoulStreakStats.SoulDrainDPSFactor += SoulDrainDamageMultiply;
            additionalData.SoulStreakStats.SoulDrainLifestealMultiply += SoulDrainLifestealMultiply;

            additionalData.SoulStreakStats.AbilityType |= AbilityType;
            #endregion

            #region Railgun Stats
            // Apply Railgun Add Stats
            additionalData.RailgunStats.MaximumCharge = Mathf.Max(additionalData.RailgunStats.MaximumCharge + MaximumCharge, 0f);
            additionalData.RailgunStats.ChargeRate = Mathf.Max(additionalData.RailgunStats.ChargeRate + ChargeRate, 0f);
            additionalData.RailgunStats.FullChargeThreshold = Mathf.Max(additionalData.RailgunStats.FullChargeThreshold + FullChargeThreshold, 0f);

            // Apply Railgun Multiplier Stats
            additionalData.RailgunStats.MaximumCharge *= MaximumChargeMultiplier;
            additionalData.RailgunStats.ChargeRate *= ChargeRateMultiplier;
            additionalData.RailgunStats.RailgunDamageMultiplier += RailgunDamageMultiplier - 1f;
            additionalData.RailgunStats.RailgunBulletSpeedMultiplier += RailgunBulletSpeedMultiplier - 1f;
            #endregion

            // Apply Uncategorized Stats
            if(SecondToDealDamage > 0) additionalData.dealDamage = false;
            additionalData.secondToDealDamage += SecondToDealDamage;

            // Apply Curses Stats
            if(SetMaxRarityForCurse) {
                var rarity = RarityUtils.GetRarity(MaxRarityForCurse.ToString());
                additionalData.MaxRarityForCurse = RarityUtils.GetRarityData(rarity);
            }
            if(IsBind) additionalData.isBind = true;

            // Apply Blocks Stats
            additionalData.BlocksWhenRecharge += BlocksWhenRecharge;
            additionalData.BlockPircePercent = Mathf.Clamp(additionalData.BlockPircePercent + BlockPircePercent, 0f, 1f);

            // Apply Extra Cards Stats
            additionalData.ExtraCardPicks += ExtraCardPicks;

            // Apply Armor Stats
            additionalData.ArmorDamageReduction = Mathf.Min(additionalData.ArmorDamageReduction + ArmorDamageReduction, 0.80f);

            if(Armor != ArmorType.None) {
                // Because the "AddArmor" method is a generic method and just generic methods, we need to use reflection to invoke it
                MethodInfo addArmorMethodInfo = typeof(ArmorHandler).GetMethod("AddArmor", BindingFlags.Public | BindingFlags.Instance);
                addArmorMethodInfo.MakeGenericMethod(GetArmorType()).Invoke(ArmorFramework.ArmorHandlers[player], new object[] { ArmorHealth, ArmorRegenRate, ArmorRegenCooldown, ArmorReactivateType, ArmorReactivateValue });
            }

            jarlAdditionalData.ArmorPiercePercent = Mathf.Clamp(jarlAdditionalData.ArmorPiercePercent + ArmorPiercePercent, 0f, 1f);
            additionalData.DamageAgainstArmorPercentage += DamageAgainstArmorPercentage - 1f;

            #region Extra Picks
            ExtraPickHandler extraPickHandler = GetExtraPickHandler(ExtraPicksType);
            if(extraPickHandler != null && ExtraPicks > 0 && player.data.view.IsMine) {
                ExtraCardPickHandler.AddExtraPick(extraPickHandler, player, ExtraPicks);
            }

            ExtraPickHandler enemyExtraPickHandler = GetExtraPickHandler(ExtraPicksTypeForEnemies);
            if(extraPickHandler != null && ExtraPicksForEnemies > 0 && player.data.view.IsMine) {
                List<Player> enemies = ModdingUtils.Utils.PlayerStatus.GetEnemyPlayers(player);

                foreach(Player enemy in enemies) {
                    ExtraCardPickHandler.AddExtraPick(enemyExtraPickHandler, enemy, ExtraPicksForEnemies);
                }
            }
            #endregion

            if(DuplicatesAsCorrupted > 0) {
                AALUND13_Cards.Instance.ExecuteAfterFrames(1, () => {
                    additionalData.DuplicatesAsCorrupted += DuplicatesAsCorrupted;
                });
            }
        }

        public void OnReassign(Player player) {
            CharacterData data = player.data;
            var additionalData = data.GetAdditionalData();

            additionalData.ExtraCardPicks += ExtraCardPicks;
        }

        public Type GetArmorType() {
            switch(Armor) {
                case ArmorType.Battleforged:
                    return typeof(BattleforgedArmor);
                case ArmorType.Titanium:
                    return typeof(TitaniumArmor);
                default:
                    return null;
            }
        }

        public ExtraPickHandler GetExtraPickHandler(ExtraPicksType type) {
            switch(type) {
                case ExtraPicksType.Normal:
                    return new ExtraPickHandler();
                case ExtraPicksType.Steel:
                    return new SteelPickHandler();
                default:
                    return null;
            }
        }
    }
}
