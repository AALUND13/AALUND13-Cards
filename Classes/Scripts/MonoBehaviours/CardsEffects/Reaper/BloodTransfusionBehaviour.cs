using AALUND13Cards.Core.Handlers;
using UnityEngine;

namespace AALUND13Cards.Classes.MonoBehaviours.CardsEffects.Reaper {
    [RequireComponent(typeof(AttackLevel))]
    public class BloodTransfusionBehaviour : MonoBehaviour, IOnDoDamageEvent {
        [Header("Particles")]
        public ParticleSystem ParticleSystem;
        public int ParticlesEmitCount;

        [Header("Parameters")]
        public float HealFromDamageMultiplier = 0.25f;
        public float Radius = 10;

        private CharacterData data;
        private AttackLevel attackLevel;

        public float CalculateScaledValue(float value, float maxValue, float scalingRange) {
            float normalizedValue = value / scalingRange;
            return Mathf.Clamp(((normalizedValue * -normalizedValue) * normalizedValue + 1) * maxValue, 0f, maxValue);
        }

        public void OnDamage(DamageInfo damage) {
            float scaleRadius = Radius * transform.localScale.x;

            float scaleDamage = damage.Damage.magnitude * (HealFromDamageMultiplier * attackLevel.attackLevel);
            float distance = Vector3.Distance(data.transform.position, damage.HurtPlayer.transform.position);
            float scaleValue = CalculateScaledValue(distance, scaleDamage, scaleRadius);

            float percentOfScaleValue = scaleValue / scaleDamage;
            int particlesToEmit = (int)(ParticlesEmitCount * percentOfScaleValue);

            data.healthHandler.Heal(scaleValue);
            ParticleSystem.Emit(particlesToEmit);
        }

        private void Start() {
            data = GetComponentInParent<CharacterData>();
            attackLevel = GetComponent<AttackLevel>();

            DamageEventHandler.Instance.RegisterDamageEventForOtherPlayers(this, data.player);
        }

        private void OnDestroy() {
            DamageEventHandler.Instance.UnregisterDamageEvent(this, data.player);
        }
    }
}
