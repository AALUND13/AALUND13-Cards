using AALUND13Cards.Classes.Cards;
using AALUND13Cards.Core.Cards;
using AALUND13Cards.Core.Extensions;
using AALUND13Cards.Core.MonoBehaviours.CardsEffects;
using UnityEngine;

namespace AALUND13Cards.Classes.Cards.StatModifers {
    public class RailgunStatModifers : CustomStatModifers {
        [Header("Railgun Stats Add")]
        public float MaximumCharge = 0f;
        public float ChargeRate = 0f;

        [Header("Railgun Stats Multiplier")]
        public float MaximumChargeMultiplier = 1f;
        public float ChargeRateMultiplier = 1f;

        public float RailgunDamageMultiplier = 1f;
        public float RailgunBulletSpeedMultiplier = 1f;

        public override void Apply(Player player) {
            CharacterData data = player.data;
            var railgunStats = data.GetAdditionalData().CustomStatsRegistry.GetOrCreate<RailgunStats>();

            // Apply Railgun Add Stats
            railgunStats.MaximumCharge = Mathf.Max(railgunStats.MaximumCharge + MaximumCharge, 0f);
            railgunStats.ChargeRate = Mathf.Max(railgunStats.ChargeRate + ChargeRate, 0f);

            // Apply Railgun Multiplier Stats
            railgunStats.MaximumCharge *= MaximumChargeMultiplier;
            railgunStats.ChargeRate *= ChargeRateMultiplier;
            railgunStats.RailgunDamageMultiplier += RailgunDamageMultiplier - 1f;
            railgunStats.RailgunBulletSpeedMultiplier += RailgunBulletSpeedMultiplier - 1f;
        }
    }
}
