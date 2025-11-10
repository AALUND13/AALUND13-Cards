using UnityEngine;
using UnityEngine.Events;

namespace AALUND13Cards.Classes.MonoBehaviours.CardsEffects {
    public class CooldownTrigger : MonoBehaviour {
        public UnityEvent Trigger;
        public float Cooldown = 5f;

        private float lastTriggerTime;

        public void TriggerCooldown() {
            if(Time.time > lastTriggerTime + Cooldown) {
                lastTriggerTime = Time.time;
                Trigger?.Invoke();
            }
        }
    }
}
