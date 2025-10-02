using AALUND13Cards.Core.Utils;
using UnityEngine;

namespace AALUND13Cards.Classes.Cards {
    public class RailgunStats : ICustomStats {
        public struct RailgunChargeStats {
            public float ChargeDamageMultiplier;
            public float ChargeBulletSpeedMultiplier;

            public RailgunChargeStats(float chargeDamageMultiplier, float chargeBulletSpeedMultiplier) {
                ChargeDamageMultiplier = chargeDamageMultiplier;
                ChargeBulletSpeedMultiplier = chargeBulletSpeedMultiplier;
            }
        }

        public bool IsEnabled = false;
        public bool AllowOvercharge = false;

        public float MaximumCharge = 0f;
        public float CurrentCharge = 0f;
        public float ChargeRate = 0f;

        public float FullChargeThreshold = 20f;

        public float RailgunDamageMultiplier = 1f;
        public float RailgunBulletSpeedMultiplier = 1f;

        public float RailgunMinimumChargeDamageMultiplier = 0.25f;
        public float RailgunMinimumChargeBulletSpeedMultiplier = 0.5f;

        public RailgunChargeStats GetChargeStats(float charge) {
            float clampedCharge = charge;
            if(!AllowOvercharge) clampedCharge = Mathf.Min(clampedCharge, FullChargeThreshold);
            else clampedCharge /= 2f;
            float chargeRatio = clampedCharge / FullChargeThreshold;

            float damageMultiplier = RailgunMinimumChargeDamageMultiplier +
                (RailgunDamageMultiplier - RailgunMinimumChargeDamageMultiplier) * chargeRatio;

            float bulletSpeedMultiplier = RailgunMinimumChargeBulletSpeedMultiplier +
                (RailgunBulletSpeedMultiplier - RailgunMinimumChargeBulletSpeedMultiplier) * chargeRatio;

            return new RailgunChargeStats(damageMultiplier, bulletSpeedMultiplier);
        }


        public void UseCharge(RailgunStats stats) {
            if(AllowOvercharge) {
                CurrentCharge = 0f;
            } else {
                CurrentCharge = Mathf.Max(CurrentCharge - FullChargeThreshold, 0);
            }
            AllowOvercharge = false;
        }

        public void ResetStats() {
            IsEnabled = false;
            AllowOvercharge = false;

            MaximumCharge = 0f;
            CurrentCharge = 0f;
            ChargeRate = 0f;

            FullChargeThreshold = 20f;

            RailgunDamageMultiplier = 1f;
            RailgunBulletSpeedMultiplier = 1f;

            RailgunMinimumChargeDamageMultiplier = 0.25f;
            RailgunMinimumChargeBulletSpeedMultiplier = 0.5f;
        }
    }

}
