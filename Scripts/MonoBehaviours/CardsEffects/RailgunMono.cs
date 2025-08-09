using AALUND13Cards.Extensions;
using ModsPlus;
using UnityEngine;

namespace AALUND13Cards.MonoBehaviours.CardsEffects {
    public class RailgunStats {
        public struct RailgunChargeStats {
            public float ChargeDamageMultiplier;
            public float ChargeBulletSpeedMultiplier;

            public RailgunChargeStats(float chargeDamageMultiplier, float chargeBulletSpeedMultiplier) {
                ChargeDamageMultiplier = chargeDamageMultiplier;
                ChargeBulletSpeedMultiplier = chargeBulletSpeedMultiplier;
            }
        }

        public bool IsEnabled = false;

        public float MaximumCharge = 0f;
        public float CurrentCharge = 0f;
        public float ChargeRate = 0f;

        public float FullChargeThreshold = 20f;

        public float RailgunDamageMultiplier = 1f;
        public float RailgunBulletSpeedMultiplier = 1f;

        public float RailgunMinimumChargeDamageMultiplier = 0.25f;
        public float RailgunMinimumChargeBulletSpeedMultiplier = 0.5f;

        public RailgunChargeStats GetChargeStats(float charge) {
            float clampedCharge = Mathf.Min(charge, FullChargeThreshold);
            float chargeRatio = clampedCharge / FullChargeThreshold;

            float damageMultiplier = Mathf.Lerp(
                RailgunMinimumChargeDamageMultiplier,
                RailgunDamageMultiplier,
                chargeRatio
            );

            float bulletSpeedMultiplier = Mathf.Lerp(
                RailgunMinimumChargeBulletSpeedMultiplier,
                RailgunBulletSpeedMultiplier,
                chargeRatio
            );

            return new RailgunChargeStats(damageMultiplier, bulletSpeedMultiplier);
        }


        public void UseCharge(RailgunStats stats) {
            CurrentCharge = Mathf.Max(CurrentCharge - FullChargeThreshold, 0);
        }
    }

    public class RailgunMono : MonoBehaviour {
        public RailgunStats RailgunStats => player.data.GetAdditionalData().RailgunStats;

        private CustomHealthBar RailgunChargeBar;
        private Player player;

        private void Start() {
            player = GetComponentInParent<Player>();
            RailgunStats.IsEnabled = true;

            RailgunChargeBar = CreateChargeBar();
            player.data.healthHandler.reviveAction += OnRevive;
        }

        public void OnDestroy() {
            if(RailgunChargeBar != null) {
                Destroy(RailgunChargeBar.gameObject);
            }
            RailgunStats.IsEnabled = false;
            player.data.healthHandler.reviveAction -= OnRevive;
        }

        public void Update() {
            if(RailgunStats.IsEnabled) {
                RailgunStats.CurrentCharge = Mathf.Min(RailgunStats.CurrentCharge + RailgunStats.ChargeRate * TimeHandler.deltaTime, RailgunStats.MaximumCharge);
            }

            RailgunChargeBar.SetValues(RailgunStats.CurrentCharge, RailgunStats.MaximumCharge);
        }

        public void OnRevive() {
            RailgunStats.CurrentCharge = RailgunStats.MaximumCharge;
        }

        private CustomHealthBar CreateChargeBar() {
            GameObject chargeBarObj = new GameObject("Railgun Charge Bar");

            CustomHealthBar chargeBar = chargeBarObj.AddComponent<CustomHealthBar>();
            chargeBar.SetColor(Color.cyan * 0.8f);
            player.AddStatusIndicator(chargeBarObj);

            Destroy(chargeBarObj.transform.Find("Healthbar(Clone)/Canvas/Image/White").gameObject);

            return chargeBar;
        }
    }
}
