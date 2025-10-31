using UnityEngine;

namespace AALUND13Cards.Core.MonoBehaviours.CardsEffects {
    [RequireComponent(typeof(DamageSpawnedAttack), typeof(Explosion))]
    public class SetExplosionDamageOfTakenDamage : MonoBehaviour {
        public float ExplosionDamageMultiplier = 1f;

        private DamageSpawnedAttack damageSpawnedAttack;
        private Explosion explosion;

        private void Awake() {
            damageSpawnedAttack = GetComponent<DamageSpawnedAttack>();
            explosion = GetComponent<Explosion>();
        }

        private void Start() {
            explosion.damage = damageSpawnedAttack.Damage.magnitude * ExplosionDamageMultiplier;
        }
    }
}
