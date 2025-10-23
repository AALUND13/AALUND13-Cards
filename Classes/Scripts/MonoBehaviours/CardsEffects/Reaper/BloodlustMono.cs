using AALUND13Cards.Classes.Cards;
using AALUND13Cards.Core;
using AALUND13Cards.Core.Extensions;
using AALUND13Cards.Core.Handlers;
using AALUND13Cards.Core.Utils;
using ModsPlus;
using UnboundLib.Extensions;
using UnityEngine;

namespace AALUND13Cards.Classes.MonoBehaviours.CardsEffects.Reaper {
    public class BloodlustMono : MonoBehaviour, IOnDoDamageEvent {
        public float MaxBlood = 100;
        public float StartingBlood = 50;

        public float BloodDrainPerSecond = 2;
        public float BloodDrainPerSecondRegen = 2;
        public float BloodHealthRegenRate = 0.05f;

        public float BloodFillPerDamage = 50;
        public float BloodDamageMultiplier = 0.25f;

        public float PercentageDamagePerDamage = 0.03f;

        private CharacterData data;
        private CustomHealthBar bloodBar;

        private float decayingPrecentageDamage;
        private float blood;
        private float appliedScaling;
        private float oldRegen;

        public void OnDamage(DamageInfo damage) {
            if(data.player != damage.DamagingPlayer) return;

            float bloodGained = BloodFillPerDamage / this.data.player.GetSPS();
            float added = PercentageDamagePerDamage / this.data.player.GetSPS();
            decayingPrecentageDamage += added;

            data.GetCustomStatsRegistry().GetOrCreate<ReaperStats>().ScalingPercentageDamageUnCap += added;
            appliedScaling += added;


            if(blood < 0) blood = StartingBlood;
            blood = Mathf.Min(MaxBlood, blood + bloodGained);

            LoggerUtils.LogInfo($"Gained {bloodGained} blood from damage, now at {blood}/{MaxBlood}");
        }


        private void OnRevive() {
            blood = StartingBlood;

            data.GetCustomStatsRegistry().GetOrCreate<ReaperStats>().ScalingPercentageDamageUnCap -= appliedScaling;
            appliedScaling = 0f;
            decayingPrecentageDamage = 0f;
        }

        private CustomHealthBar CreateStoredDamageBar() {
            GameObject bloodBarObj = new GameObject("Blood Bar");

            CustomHealthBar bloodBar = bloodBarObj.AddComponent<CustomHealthBar>();
            bloodBar.SetColor(new Color(0.6615686275f, 0.0431372549f, 0.0431372549f, 1f) * 0.8f);

            data.player.AddStatusIndicator(bloodBarObj);

            Destroy(bloodBarObj.transform.Find("Healthbar(Clone)/Canvas/Image/White").gameObject);

            return bloodBar;
        }


        private void Update() {
            if(!GameManager.instance.battleOngoing) return;
            float old = decayingPrecentageDamage;

            decayingPrecentageDamage = Mathf.Max(
                0,
                decayingPrecentageDamage - (decayingPrecentageDamage * decayingPrecentageDamage * 5) * Time.deltaTime
            );

            float diff = decayingPrecentageDamage - old;
            data.GetCustomStatsRegistry().GetOrCreate<ReaperStats>().ScalingPercentageDamageUnCap += diff;
            appliedScaling += diff;

            float bloodDrainPerSecond = BloodDrainPerSecond;
            if(data.health < data.maxHealth)
                bloodDrainPerSecond += BloodDrainPerSecondRegen;

            blood -= bloodDrainPerSecond * Time.deltaTime;

            if(blood <= 0) {
                data.healthHandler.regeneration -= oldRegen;
                oldRegen = 0;

                float damage = (data.maxHealth * ((-blood) * BloodDamageMultiplier)) * Time.deltaTime;
                data.healthHandler.TakeDamage(Vector2.down * damage, Vector2.zero, null, null, true, true);
            } else if(blood > 0 && data.health < data.maxHealth) {
                float regen = data.maxHealth * BloodHealthRegenRate;
                data.healthHandler.regeneration -= oldRegen;
                data.healthHandler.regeneration += regen;
                oldRegen = regen;
            } else if (oldRegen != 0) {
                data.healthHandler.regeneration -= oldRegen;
                oldRegen = 0;
            }

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

            if(oldRegen != 0) {
                data.healthHandler.regeneration -= oldRegen;
            }
        }
    }
}
