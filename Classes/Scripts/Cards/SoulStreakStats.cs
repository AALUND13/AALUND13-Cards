using AALUND13Cards.Classes.MonoBehaviours.CardsEffects.Soulstreak.Abilities;
using AALUND13Cards.Core.Utils;
using System.Collections.Generic;

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
        public float SoulDrainLifestealMultiply = 0;

        // Abilities
        public List<ISoulstreakAbility> Abilities = new List<ISoulstreakAbility>();


        public uint Souls = 0;

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

            // Abilities
            Abilities.Clear();
        }
    }
}
