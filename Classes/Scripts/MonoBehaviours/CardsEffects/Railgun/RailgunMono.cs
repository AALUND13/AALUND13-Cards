using AALUND13Cards.Classes.Cards;
using AALUND13Cards.Core.Extensions;
using AALUND13Cards.Core.Utils;
using ModsPlus;
using UnityEngine;

namespace AALUND13Cards.Core.MonoBehaviours.CardsEffects {
    public class RailgunMono : MonoBehaviour {
        [HideInInspector] public RailgunStats RailgunStats;

        private CustomHealthBar RailgunChargeBar;
        private Player player;

        private void Start() {
            player = GetComponentInParent<Player>();

            RailgunStats = player.data.GetAdditionalData().CustomStatsRegistry.GetOrCreate<RailgunStats>();
            RailgunStats.IsEnabled = true;

            RailgunChargeBar = CreateChargeBar();

            player.data.healthHandler.reviveAction += OnRevive;
        }

        public void OnDestroy() {
            Destroy(RailgunChargeBar.gameObject);

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
