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

namespace AALUND13Cards.Cards.StatModifers {
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

    public class AAStatModifers : CustomStatModifers {
        [Header("Uncategorized Stats")]
        public float SecondToDealDamage = 0;
        public float DamageMultiplier = 1f;

        [Header("Percentage Damage")]
        public float ScalingPercentageDamage = 0;

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

        public override void Apply(Player player) {
            CharacterData data = player.data;
            var additionalData = data.GetAdditionalData();
            var jarlAdditionalData = JARL.Extensions.CharacterDataExtensions.GetAdditionalData(data);

            // Apply Uncategorized Stats
            if(SecondToDealDamage > 0) additionalData.dealDamage = false;
            additionalData.secondToDealDamage += SecondToDealDamage;
            data.weaponHandler.gun.damage *= DamageMultiplier;

            // Apply Percentage Damage
            additionalData.ScalingPercentageDamage += ScalingPercentageDamage;

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

        public override void OnReassign(Player player) {
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
