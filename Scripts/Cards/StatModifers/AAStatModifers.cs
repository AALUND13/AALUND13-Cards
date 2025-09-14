using AALUND13Cards.Armors;
using AALUND13Cards.Extensions;
using AALUND13Cards.Handlers;
using AALUND13Cards.Handlers.ExtraPickHandlers;
using JARL.Armor;
using JARL.Armor.Bases;
using JARL.Bases;
using RarityLib.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using UnboundLib;
using UnityEngine;

namespace AALUND13Cards.Cards.StatModifers {
    public enum ExtraPicksType {
        None,
        Normal,
        Steel
    }

    public enum ArmorType {
        None,
        Battleforged,
        Titanium,
        ExoArmor
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
        public bool DisableDecayTime = false;

        [Header("Blocks Stats")]
        public int BlocksWhenRecharge = 0;
        public float BlockPircePercent = 0f;
        public float StunBlockTime = 0f;

        [Header("Armors Stats")]
        public float ArmorDamageReduction = 0f;

        [Space(10)]
        public ArmorType Armor = ArmorType.None;
        public float ArmorHealth = 0f;
        public float ArmorHealthMultiplier = 1f;
        public float ArmorRegenRate = 0f;
        public float ArmorRegenCooldown = 0f;
        public bool RemoveArmorPirceTag = false;

        [Space(10)]
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
            if(IsBind) additionalData.IsBind = true;
            if(DisableDecayTime) additionalData.DisableDecayTime = true;

            // Apply Blocks Stats
            additionalData.BlocksWhenRecharge += BlocksWhenRecharge;
            additionalData.BlockPircePercent = Mathf.Clamp(additionalData.BlockPircePercent + BlockPircePercent, 0f, 1f);
            additionalData.StunBlockTime += StunBlockTime;

            // Apply Extra Cards Stats
            additionalData.ExtraCardPicks += ExtraCardPicks;

            // Apply Armor Stats
            additionalData.ArmorDamageReduction = Mathf.Min(additionalData.ArmorDamageReduction + ArmorDamageReduction, 0.80f);

            if(Armor != ArmorType.None) {
                ArmorBase armorInstance = ArmorFramework.ArmorHandlers[player].Armors.First(x => x.GetType() == GetArmorType());
                if(armorInstance != null) {
                    armorInstance.MaxArmorValue += ArmorHealth;
                    if(AALUND13_Cards.Plugins.Exists(plugin => plugin.Info.Metadata.GUID == "Systems.R00t.AdditiveStats")) {
                        armorInstance.MaxArmorValue += 100 * (ArmorHealthMultiplier - 1);
                    } else {
                        armorInstance.MaxArmorValue *= ArmorHealthMultiplier;
                    }

                    armorInstance.ArmorRegenerationRate += ArmorRegenRate;
                    armorInstance.ArmorRegenCooldownSeconds += ArmorRegenCooldown;
                    if(ArmorReactivateValue > 0f) {
                        armorInstance.reactivateArmorType = ArmorReactivateType;
                        armorInstance.reactivateArmorValue = ArmorReactivateValue;
                    }
                    if(RemoveArmorPirceTag && armorInstance.HasArmorTag("CanArmorPierce")) {
                        armorInstance.ArmorTags.Remove("CanArmorPierce");
                    }
                }
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
                case ArmorType.ExoArmor:
                    return typeof(ExoArmor);
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
