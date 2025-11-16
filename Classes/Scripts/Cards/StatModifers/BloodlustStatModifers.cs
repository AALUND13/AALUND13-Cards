using AALUND13Cards.Core.Cards;
using AALUND13Cards.Core.Extensions;
using UnityEngine;

namespace AALUND13Cards.Classes.Cards.StatModifers {
    public class BloodlustStatModifers : CustomStatModifers {
        [Header("Blood Values")]
        public float MaxBlood;
        public float StartingBlood;

        [Header("Blood Healing/Draining")]
        public float BloodDrainRate;
        public float BloodDrainRateWhenRegen;
        public float BloodHealthRegenRate;

        [Header("Blood Damage")]
        public float DamageFromNoBlood;
        public float BloodFillPerDamage;
        public float DamageMultiplierFromDamage;

        public override void Apply(Player player) {
            CharacterData data = player.data;
            var bloodlustStats = data.GetCustomStatsRegistry().GetOrCreate<BloodlustStats>();

            bloodlustStats.MaxBlood = Mathf.Max(bloodlustStats.MaxBlood + MaxBlood, 0);
            bloodlustStats.StartingBlood = Mathf.Max(bloodlustStats.StartingBlood + StartingBlood, 0);

            bloodlustStats.BloodDrainRate = Mathf.Max(bloodlustStats.BloodDrainRate + BloodDrainRate, 0);
            bloodlustStats.BloodHealthRegenRate = Mathf.Max(bloodlustStats.BloodHealthRegenRate + BloodHealthRegenRate, 0);
            if(BloodDrainRateWhenRegen > 0) {
                bloodlustStats.AddBloodDrain("Regen", BloodDrainRateWhenRegen);
            } else if(BloodDrainRateWhenRegen < 0) {
                bloodlustStats.RemoveBloodDrain("Regen", BloodDrainRateWhenRegen);
            }

            bloodlustStats.DamageFromNoBlood = Mathf.Max(bloodlustStats.DamageFromNoBlood + DamageFromNoBlood, 0);
            bloodlustStats.BloodFillPerDamage = Mathf.Max(bloodlustStats.BloodFillPerDamage + BloodFillPerDamage, 0);
            bloodlustStats.DamageMultiplierFromDamage = Mathf.Max(bloodlustStats.DamageMultiplierFromDamage + DamageMultiplierFromDamage, 0);
        }
    }
}
