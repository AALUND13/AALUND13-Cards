using AALUND13Cards.Classes.MonoBehaviours.CardsEffects.Soulstreak;
using AALUND13Cards.Classes.MonoBehaviours.CardsEffects.Soulstreak.Abilities;
using AALUND13Cards.Core.Cards;
using AALUND13Cards.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AALUND13Cards.Classes.Cards.StatModifers {
    [Flags]
    public enum AbilityType {
        Armor = 1 << 0,
    }

    public class SoulstreakStatModifers : CustomStatModifers {
        [Header("Character Stats")]
        public float MaxHealth = 0;
        public float PlayerSize = 0;
        public float MovementSpeed = 0;

        [Header("Gun Stats")]
        public float AttackSpeed = 0;
        public float Damage = 0;
        public float BulletSpeed = 0;

        [Header("Soul Armor")]
        public float SoulArmorPercentage = 0;
        public float SoulArmorPercentageRegenRate = 0;

        [Header("Soul Drain")]
        public float SoulDrainDamageMultiply = 0;
        public float SoulDrainPercentageDPSFactor = 0;
        public float SoulDrainLifestealMultiply = 0;
        
        [Header("Dreadful Burst")]
        public float BurstDamageMultiplier = 0;
        public float DamageStorage = 0;

        [Header("Damage Resistance Per Kill")]
        public float DamageResistancePerKill = 0;

        [Header("Abilities")]
        public AbilityType AbilityType;

        public override void Apply(Player player) {
            CharacterData data = player.data;
            var soulstreakStats = data.GetAdditionalData().CustomStatsRegistry.GetOrCreate<SoulStreakStats>();

            // Character Stats
            soulstreakStats.MaxHealth += MaxHealth;
            soulstreakStats.PlayerSize += PlayerSize;
            soulstreakStats.MovementSpeed += MovementSpeed;

            // Gun Stats
            soulstreakStats.AttackSpeed += AttackSpeed;
            soulstreakStats.Damage += Damage;
            soulstreakStats.BulletSpeed += BulletSpeed;

            // Soul Armor Stats
            soulstreakStats.SoulArmorPercentage += SoulArmorPercentage;
            soulstreakStats.SoulArmorPercentageRegenRate += SoulArmorPercentageRegenRate;

            // Soul Drain Stats
            soulstreakStats.SoulDrainDPSFactor += SoulDrainDamageMultiply;
            soulstreakStats.SoulDrainPercentageDPSFactor += SoulDrainPercentageDPSFactor;
            soulstreakStats.SoulDrainLifestealMultiply += SoulDrainLifestealMultiply;

            // Dreadful Burst
            soulstreakStats.BurstDamageMultiplier += BurstDamageMultiplier;
            soulstreakStats.DamageStorage += DamageStorage;

            if((AbilityType & AbilityType.Armor) == AbilityType.Armor) {
                soulstreakStats.AddAbility(new ArmorAbility(player, 10f));
            }

            if(DamageResistancePerKill != 0) {
                soulstreakStats.AddAbility(new SoulstealerResistanceAbiilty(player));
                soulstreakStats.DamageResistancePerKill += DamageResistancePerKill;
            }
        }
    }
}
