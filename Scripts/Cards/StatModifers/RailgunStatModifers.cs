using AALUND13Cards.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AALUND13Cards.Cards.StatModifers {
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
            var additionalData = data.GetAdditionalData();

            // Apply Railgun Add Stats
            additionalData.RailgunStats.MaximumCharge = Mathf.Max(additionalData.RailgunStats.MaximumCharge + MaximumCharge, 0f);
            additionalData.RailgunStats.ChargeRate = Mathf.Max(additionalData.RailgunStats.ChargeRate + ChargeRate, 0f);

            // Apply Railgun Multiplier Stats
            additionalData.RailgunStats.MaximumCharge *= MaximumChargeMultiplier;
            additionalData.RailgunStats.ChargeRate *= ChargeRateMultiplier;
            additionalData.RailgunStats.RailgunDamageMultiplier += RailgunDamageMultiplier - 1f;
            additionalData.RailgunStats.RailgunBulletSpeedMultiplier += RailgunBulletSpeedMultiplier - 1f;
        }
    }
}
