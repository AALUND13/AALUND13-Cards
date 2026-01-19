using AALUND13Cards.Classes.MonoBehaviours.CardsEffects.Soulstreak.Abilities;
using AALUND13Cards.Core.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace AALUND13Cards.Classes.Cards {
    public class SoulStreakStats : ICustomStats {
        // Character Stats
        public float MaxHealth = 1;
        public float PlayerSize = 1;
        public float MovementSpeed = 1;
        public float AttackSpeed = 1;
        public float Damage = 1;
        public float BulletSpeed = 1;

        // Soul Armor Stats
        public float SoulArmorPercentage = 0;
        public float SoulArmorPercentageRegenRate = 0;

        // Soul Drain Stats
        public float SoulDrainDPSFactor = 0;
        public float SoulDrainPercentageDPSFactor = 0;
        public float SoulDrainLifestealMultiply = 0;

        // Other Stats
        public float DamageResistancePerKill = 0;

        // Abilities
        public Dictionary<Type, ISoulstreakAbility> AbilitiesMap = new Dictionary<Type, ISoulstreakAbility>();
        public ReadOnlyCollection<ISoulstreakAbility> Abilities => AbilitiesMap.Values.ToList().AsReadOnly();

        public uint Souls = 0;

        public TAbility AddAbility<TAbility>(TAbility soulstreakAbility)
            where TAbility : SoulstreakAbility<TAbility> 
        {
            if(AbilitiesMap.TryGetValue(soulstreakAbility.GetType(), out ISoulstreakAbility existing)) {
                if(existing is TAbility typedExisting) {
                    return typedExisting;
                }
            }

            AbilitiesMap.Add(soulstreakAbility.GetType(), soulstreakAbility);
            soulstreakAbility.SoulstreakStats = this;
            
            return soulstreakAbility;
        }

        public TAbility GetAbility<TAbility>()
            where TAbility : SoulstreakAbility<TAbility> 
        {
            if(AbilitiesMap.TryGetValue(typeof(TAbility), out ISoulstreakAbility ability)) {
                return (TAbility)ability;
            }

            return null;
        }


        public void ResetStats() {
            // Character Stats
            MaxHealth = 1;
            PlayerSize = 1;
            MovementSpeed = 1;
            AttackSpeed = 1;
            Damage = 1;
            BulletSpeed = 1;

            // Soul Armor Stats
            SoulArmorPercentage = 0;
            SoulArmorPercentageRegenRate = 0;

            // Soul Drain Stats
            SoulDrainDPSFactor = 0;
            SoulDrainPercentageDPSFactor = 0;
            SoulDrainLifestealMultiply = 0;

            // Other Resistance Stats
            DamageResistancePerKill = 0;

            // Abilities
            AbilitiesMap.Clear();
        }
    }
}
