using AALUND13Cards.Extensions;
using UnityEngine;

namespace AALUND13Cards.MonoBehaviours.CardsEffects {
    public class CurrentHPRegenMono : MonoBehaviour {
        public float activatePercentage = 0.75f; // Percentage of current health to activate the regeneration

        private CharacterData data;
        private float oldRegen;

        private void Start() {
            data = GetComponentInParent<CharacterData>();
        }

        public void Update() {
            float regenPercentage = data.GetAdditionalData().CurrentHPRegenPercentage;
            if(regenPercentage != 0) {
                if (oldRegen != 0 && data.health / data.maxHealth > activatePercentage) {
                    data.healthHandler.regeneration -= oldRegen;
                    oldRegen = 0;
                    return;
                } else if(data.health / data.maxHealth <= activatePercentage) {
                    float regen = data.health * regenPercentage;

                    data.healthHandler.regeneration -= oldRegen;
                    data.healthHandler.regeneration += regen;

                    oldRegen = regen;
                }
            }
        }
    }
}
