using AALUND13Card.Extensions;
using AALUND13Card.MonoBehaviours;
using UnityEngine;

namespace Assets.Mods._AALUND13_Card.Scripts.Cards {
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

        [Header("Clamp Stats")]
        public float MaxHealthCap = 0;
        public float MaxDamageCap = 0;

        [Header("Uncategorized Stats")]
        public int RandomCardsAtStart = 0;
        public float secondToDealDamage = 0;

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

            //Apply Clamp Stats
            additionalData.MaxHealthCap += MaxHealthCap;
            additionalData.MaxDamageCap += MaxDamageCap;

            // Apply Uncategorized Stats
            additionalData.RandomCardsAtStart += RandomCardsAtStart;
            additionalData.secondToDealDamage += secondToDealDamage;
        }
    }
}
