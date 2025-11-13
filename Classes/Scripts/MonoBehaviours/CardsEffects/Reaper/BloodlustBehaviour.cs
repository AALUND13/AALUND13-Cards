using AALUND13Cards.Classes.Cards;
using AALUND13Cards.Core;
using AALUND13Cards.Core.Extensions;
using AALUND13Cards.Core.Handlers;
using ModsPlus;
using UnboundLib;
using UnityEngine;

namespace AALUND13Cards.Classes.MonoBehaviours.CardsEffects.Reaper {
    public class BloodlustBehaviour : MonoBehaviour, IOnDoDamageEvent {
        public const float DEFAULT_MAX_BLOOD = 100f;

        private CharacterData data;
        private CustomHealthBar bloodBar;

        private float decayingPrecentageDamage;
        private float appliedScaling;

        public ReaperStats ReaperStats => data.GetCustomStatsRegistry().GetOrCreate<ReaperStats>();
        public BloodlustStats BloodlustStats => data.GetCustomStatsRegistry().GetOrCreate<BloodlustStats>();

        public void OnDamage(DamageInfo damage) {
            if(data.player != damage.DamagingPlayer || BloodlustStats.DisableDamageGain) return;

            float damagePercent = Mathf.Min(damage.Damage.magnitude, damage.HurtPlayer.data.maxHealth) / damage.HurtPlayer.data.maxHealth;
            float bloodGained = DEFAULT_MAX_BLOOD * damagePercent * BloodlustStats.BloodFillPerDamage;

            float added = damagePercent * BloodlustStats.DamageMultiplierFromDamage;
            decayingPrecentageDamage += added;
            ReaperStats.ScalingPercentageDamageUnCap += added;
            appliedScaling += added;

            if(BloodlustStats.Blood < 0) BloodlustStats.Blood = 0;
            BloodlustStats.Blood = Mathf.Min(BloodlustStats.MaxBlood, BloodlustStats.Blood + bloodGained);

            LoggerUtils.LogInfo($"Gained {bloodGained} blood from damage, now at {BloodlustStats.Blood}/{BloodlustStats.MaxBlood}");
        }

        private void OnRevive() {
            BloodlustStats.Blood = BloodlustStats.StartingBlood;

            ReaperStats.ScalingPercentageDamageUnCap -= appliedScaling;
            appliedScaling = 0f;
            decayingPrecentageDamage = 0f;
            LoggerUtils.LogInfo($"Reset \"blood\" value to {BloodlustStats.Blood}, and \"decayingPrecentageDamage\" to {decayingPrecentageDamage}");
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
            if(BloodlustStats.Blood <= 0) {
                float damage = (data.maxHealth * ((-BloodlustStats.Blood) * BloodlustStats.DamageFromNoBlood));
                data.healthHandler.DoDamage(Vector2.down * damage * Time.deltaTime, Vector2.zero, Color.red * 0.6f, null, null, false, true, true);
            } else if(BloodlustStats.Blood > 0 && data.health < data.maxHealth) {
                float regen = data.maxHealth * BloodlustStats.BloodHealthRegenRate;
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

            if(data.health < data.maxHealth) BloodlustStats.ToggleBloodDrain("Regen", true);
            else BloodlustStats.ToggleBloodDrain("Regen", false);
            BloodlustStats.Blood -= BloodlustStats.GetBloodDrain() * Time.deltaTime;

            UpdateBloodState();

            bloodBar.SetValues(BloodlustStats.Blood, BloodlustStats.MaxBlood);
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
