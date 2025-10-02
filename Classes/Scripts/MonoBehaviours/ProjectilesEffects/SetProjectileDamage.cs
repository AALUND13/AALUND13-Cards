using UnityEngine;

namespace AALUND13Cards.Classes.MonoBehaviours.ProjectilesEffects {
    public class SetProjectileDamage : MonoBehaviour {
        public float damage;

        private void Start() {
            GetComponentInParent<ProjectileHit>().damage = damage;
        }
    }
}