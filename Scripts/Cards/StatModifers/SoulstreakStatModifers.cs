using AALUND13Cards.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AALUND13Cards.Cards.StatModifers {
    [Flags]
    public enum AbilityType {
        Armor = 1 << 0,
    }

    public class SoulstreakStatModifers : CustomStatModifers {
        [Header("Character Stats")]
        public float MaxHealth = 0;
        public float PlayerSize = 0;
        public float MovementSpeed = 0;
        public float AttackSpeed = 0;
        public float Damage = 0;
        public float BulletSpeed = 0;

        [Header("Soul Armor")]
        public float SoulArmorPercentage = 0;
        public float SoulArmorPercentageRegenRate = 0;

        [Header("Soul Drain")]
        public float SoulDrainDamageMultiply = 0;
        public float SoulDrainLifestealMultiply = 0;

        [Header("Abilities")]
        public AbilityType AbilityType;

        public override void Apply(Player player) {
            CharacterData data = player.data;
            var additionalData = data.GetAdditionalData();

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
            #endregion
        }
    }
}
