using AALUND13Cards.Handlers;
using UnityEngine;

namespace AALUND13Cards.MonoBehaviours.CardsEffects {
    public class ReverseLifeSteelEffect : MonoBehaviour, IOnDoDamageEvent {
        public float ReverseLifeSteelPercentage = 0.2f;

        private Player player;

        public void OnDamage(DamageInfo damage) {
            if(damage.DamagingPlayer != null) {
                damage.DamagingPlayer.data.healthHandler.Heal(damage.Damage.magnitude * ReverseLifeSteelPercentage);
            }
        }

        private void Start() {
            player = GetComponentInParent<Player>();
            DamageEventHandler.Instance.RegisterDamageEvent(this, player);
        }

        private void OnDestroy() {
            DamageEventHandler.Instance.UnregisterDamageEvent(this, player);
        }
    }
}
