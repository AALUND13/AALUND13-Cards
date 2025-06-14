﻿using AALUND13Cards.Armors;
using AALUND13Cards.Extensions;
using AALUND13Cards.Handlers;
using AALUND13Cards.Handlers.ExtraPickHandlers;
using AALUND13Cards.MonoBehaviours;
using JARL.Armor;
using UnityEngine;

namespace AALUND13Cards.Cards {
    public enum ExtraPicksType {
        None,
        Normal,
        Steel
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

        [Header("Uncategorized Stats")]
        public float SecondToDealDamage = 0;
        public float CurrentHPRegenPercentage = 0;
        public int BlocksWhenRecharge = 0;

        [Header("Armors Stats")]
        public float BattleforgedArmor = 0;

        [Header("Extra Picks")]
        public int ExtraPicks = 0;
        public ExtraPicksType ExtraPicksType;

        [Header("Extra Cards")]
        public int RandomCardsAtStart = 0;
        public int ExtraCardPicks = 0;

        public void Apply(Player player) {
            CharacterData data = player.data;
            var additionalData = data.GetAdditionalData();

            // Apply Soulstreak Stats
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

            // Apply Uncategorized Stats
            if(SecondToDealDamage > 0) {
                additionalData.dealDamage = false;
            }
            additionalData.secondToDealDamage += SecondToDealDamage;
            additionalData.CurrentHPRegenPercentage += CurrentHPRegenPercentage;
            additionalData.BlocksWhenRecharge += BlocksWhenRecharge;

            // Apply Extra Cards Stats
            additionalData.RandomCardsAtStart += RandomCardsAtStart;
            additionalData.ExtraCardPicks += ExtraCardPicks;

            if(BattleforgedArmor > 0) {
                ArmorFramework.ArmorHandlers[player].AddArmor<BattleforgedArmor>(BattleforgedArmor, 0, 0, ArmorReactivateType.Percent, 0.5f);
            }

            ExtraPickHandler extraPickHandler = GetExtraPickHandler(ExtraPicksType);
            if(extraPickHandler != null && ExtraPicks > 0 && player.data.view.IsMine) {
                ExtraCardPickHandler.AddExtraPick(extraPickHandler, player, ExtraPicks);
            }
        }

        public void OnReassign(Player player) {
            CharacterData data = player.data;
            var additionalData = data.GetAdditionalData();

            additionalData.ExtraCardPicks += ExtraCardPicks;
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
