﻿using AALUND13Card.Armors;
using AALUND13Card.Extensions;
using AALUND13Card.MonoBehaviours;
using JARL.Armor;
using UnityEngine;

namespace AALUND13Card.Cards {
    public class AAStatModifers : MonoBehaviour {
        [Header("Soulstreak Stats")]
        public float MaxHealth = 0;
        public float PlayerSize = 0;
        public float MovementSpeed = 0;

        public float AttackSpeed = 0;
        public float Damage = 0;
        public float BulletSpeed = 0;

        public float SoulArmorPercentage = 0;
        public float SoulArmorPercentageRegenRate = 0;

        public float SoulDrainMultiply = 0;

        public AbilityType AbilityType;

        [Header("Uncategorized Stats")]
        public int RandomCardsAtStart = 0;
        public float SecondToDealDamage = 0;

        [Header("Armors Stats")]
        public float BattleforgedArmor = 0;

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

            additionalData.SoulStreakStats.SoulDrainMultiply += SoulDrainMultiply;

            additionalData.SoulStreakStats.AbilityType |= AbilityType;

            // Apply Uncategorized Stats
            additionalData.RandomCardsAtStart += RandomCardsAtStart;
            additionalData.secondToDealDamage += SecondToDealDamage;

            if(BattleforgedArmor > 0) {
                ArmorFramework.ArmorHandlers[player].AddArmor(typeof(BattleforgedArmor), BattleforgedArmor, 0, 0, ArmorReactivateType.Percent, 0.5f);
            }
        }
    }
}
