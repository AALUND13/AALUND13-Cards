using AALUND13Cards.Classes.Cards;
using AALUND13Cards.Core;
using AALUND13Cards.Core.Extensions;
using AALUND13Cards.Core.Handlers;
using ModsPlus;
using UnboundLib;
using UnityEngine;

namespace AALUND13Cards.Classes.MonoBehaviours.CardsEffects.Reaper {
    public class BloodlustBehaviour : MonoBehaviour, IOnDoDamageEvent {
        [Header("Blood Values")]
        public float MaxBlood = 100;
        public float StartingBlood = 50;

        [Header("Blood Healing/Draining")]
        public float BloodDrainPerSecond = 2;
        public float BloodDrainPerSecondRegen = 2;
        public float BloodHealthRegenRate = 0.05f;

        [Header("Blood Damage")]
        public float BloodFillPerDamage = 2;
        public float DamageMultiplierFromDamage = 0.3f;
        public float DamageFromNoBlood = 0.0005f;

        private CharacterData data;
        private CustomHealthBar bloodBar;

        private float decayingPrecentageDamage;
        private float blood;
        private float appliedScaling;

        public ReaperStats ReaperStats => data.GetCustomStatsRegistry().GetOrCreate<ReaperStats>();

        public void OnDamage(DamageInfo damage) {
            if(data.player != damage.DamagingPlayer) return;

            float damagePercent = Mathf.Min(damage.Damage.magnitude, damage.HurtPlayer.data.maxHealth) / damage.HurtPlayer.data.maxHealth;
            float bloodGained = 100 * damagePercent * BloodFillPerDamage;

            float added = damagePercent * DamageMultiplierFromDamage;
            decayingPrecentageDamage += added;
            ReaperStats.ScalingPercentageDamageUnCap += added;
            appliedScaling += added;

            if(blood < 0) blood = 0;
            blood = Mathf.Min(MaxBlood, blood + bloodGained);

            LoggerUtils.LogInfo($"Gained {bloodGained} blood from damage, now at {blood}/{MaxBlood}");
        }

        private void OnRevive() {
            blood = StartingBlood;

            ReaperStats.ScalingPercentageDamageUnCap -= appliedScaling;
            appliedScaling = 0f;
            decayingPrecentageDamage = 0f;
            LoggerUtils.LogInfo($"Reset \"blood\" value to {blood}, and \"decayingPrecentageDamage\" to {decayingPrecentageDamage}");
        }

        private CustomHealthBar CreateStoredDamageBar() {
            GameObject bloodBarObj = new GameObject("Blood Bar");

            CustomHealthBar bloodBar = bloodBarObj.AddComponent<CustomHealthBar>();
            bloodBar.SetColor(new Color(0.6615686275f, 0.0431372549f, 0.0431372549f, 1f) * 0.8f);

            data.player.AddStatusIndicator(bloodBarObj);

            Destroy(bloodBarObj.transform.Find("Healthbar(Clone)/Canvas/Image/White").gameObject);
            LoggerUtils.LogInfo($"Created a blood bar");

            return bloodBar;
        }

        private float DecayValue(float value) {
            return Mathf.Max(
                0,
                value - (value * value * 2) * Time.deltaTime
            );
        }

        private void UpdateBloodState() {
            if(blood <= 0) {
                float damage = (data.maxHealth * ((-blood) * DamageFromNoBlood));
                data.healthHandler.DoDamage(Vector2.down * damage * Time.deltaTime, Vector2.zero, Color.red * 0.6f, null, null, false, true, true);
            } else if(blood > 0 && data.health < data.maxHealth) {
                float regen = data.maxHealth * BloodHealthRegenRate;
                data.healthHandler.Heal(regen * Time.deltaTime);
            }
        }

        #region Unity Methods

        private void Update() {
            if(!(bool)data.playerVel.GetFieldValue("simulated")) return;

            float old = decayingPrecentageDamage;
            decayingPrecentageDamage = DecayValue(decayingPrecentageDamage);
            float diff = decayingPrecentageDamage - old;

            ReaperStats.ScalingPercentageDamageUnCap += diff;
            appliedScaling += diff;

            float bloodDrainPerSecond = BloodDrainPerSecond;
            if(data.health < data.maxHealth) {
                bloodDrainPerSecond += BloodDrainPerSecondRegen;
            }
            blood -= bloodDrainPerSecond * Time.deltaTime;

            UpdateBloodState();

            bloodBar.SetValues(blood, MaxBlood);
        }

        private void Start() {
            data = GetComponentInParent<CharacterData>();
            bloodBar = CreateStoredDamageBar();
            data.healthHandler.reviveAction += OnRevive;
            DamageEventHandler.Instance.RegisterDamageEventForOtherPlayers(this, data.player);
        }

        private void OnDestroy() {
            Destroy(bloodBar.gameObject);

            data.GetCustomStatsRegistry().GetOrCreate<ReaperStats>().ScalingPercentageDamageUnCap -= appliedScaling;

            data.healthHandler.reviveAction -= OnRevive;
            DamageEventHandler.Instance.UnregisterDamageEvent(this, data.player);
        }

        #endregion
    }
}
