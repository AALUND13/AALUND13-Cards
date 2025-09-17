using AALUND13Cards.Extensions;
using AALUND13Cards.Handlers;
using ModsPlus;
using Photon.Pun;
using Photon.Realtime;
using UnboundLib.Extensions;
using UnityEngine;

namespace AALUND13Cards.MonoBehaviours.CardsEffects.Reaper {
    public class BloodlustMono : MonoBehaviour, IOnDoDamageEvent {
        public float MaxBlood = 100;
        public float StartingBlood = 50;

        public float BloodDrainPerSecond = 5;
        public float BloodFillPerDamage = 50;
        public float BloodDamageMultiplier = 0.25f;

        public float PercentageDamagePerDamage = 0.03f;


        private Player player;
        private CustomHealthBar bloodBar;


        private float decayingPrecentageDamage = 0f;
        private float blood = 0;
        private float appliedScaling = 0f;

        public void OnDamage(DamageInfo damage) {
            if(player != damage.DamagingPlayer) return;

            float bloodGained = BloodFillPerDamage / this.player.GetShootsPerSecond();
            float added = PercentageDamagePerDamage / this.player.GetShootsPerSecond();
            decayingPrecentageDamage += added;

            player.data.GetAdditionalData().ScalingPercentageDamage += added;
            appliedScaling += added;


            if(blood < 0) blood = 0;
            blood = Mathf.Min(MaxBlood, blood + bloodGained);

            LoggerUtils.LogInfo($"Gained {bloodGained} blood from damage, now at {blood}/{MaxBlood}");
        }


        private void OnRevive() {
            blood = StartingBlood;

            player.data.GetAdditionalData().ScalingPercentageDamage -= appliedScaling;
            appliedScaling = 0f;
            decayingPrecentageDamage = 0f;
        }

        private CustomHealthBar CreateStoredDamageBar() {
            GameObject bloodBarObj = new GameObject("Blood Bar");

            CustomHealthBar bloodBar = bloodBarObj.AddComponent<CustomHealthBar>();
            bloodBar.SetColor(new Color(0.6615686275f, 0.0431372549f, 0.0431372549f, 1f) * 0.8f);

            player.AddStatusIndicator(bloodBarObj);

            Destroy(bloodBarObj.transform.Find("Healthbar(Clone)/Canvas/Image/White").gameObject);

            return bloodBar;
        }


        private void Update() {
            float old = decayingPrecentageDamage;

            decayingPrecentageDamage = Mathf.Max(
                0,
                decayingPrecentageDamage - (decayingPrecentageDamage * decayingPrecentageDamage * 5) * Time.deltaTime
            );

            float diff = decayingPrecentageDamage - old;
            player.data.GetAdditionalData().ScalingPercentageDamage += diff;
            appliedScaling += diff;

            blood -= BloodDrainPerSecond * Time.deltaTime;

            if(blood <= 0) {
                float damage = (player.data.maxHealth * ((-blood) * BloodDamageMultiplier)) * Time.deltaTime;
                player.data.healthHandler.TakeDamage(Vector2.down * damage, Vector2.zero, null, null, true, true);
            }

            bloodBar.SetValues(blood, MaxBlood);
        }


        private void Start() {
            player = GetComponentInParent<Player>();
            bloodBar = CreateStoredDamageBar();
            player.data.healthHandler.reviveAction += OnRevive;
            DamageEventHandler.Instance.RegisterDamageEventForOtherPlayers(this, player);
        }

        private void OnDestroy() {
            Destroy(bloodBar.gameObject);

            player.data.GetAdditionalData().ScalingPercentageDamage -= appliedScaling;

            player.data.healthHandler.reviveAction -= OnRevive;
            DamageEventHandler.Instance.UnregisterDamageEvent(this, player);
        }
    }
}
