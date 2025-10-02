using UnityEngine;

namespace AALUND13Cards.Core.MonoBehaviours.CardsEffects {
    public class CurrentHPRegenMono : MonoBehaviour {
        [Tooltip("Percentage of current health to activate the regeneration. If the character's health is above this percentage, regeneration will not be applied.")]
        public float activatePercentage = 0.75f;
        public float CurrentHPRegenPercentage = 0.15f;

        private CharacterData data;
        private float oldRegen;

        private void Start() {
            data = GetComponentInParent<CharacterData>();
        }

        public void Update() {
            if(oldRegen != 0 && data.health / data.maxHealth > activatePercentage) {
                data.healthHandler.regeneration -= oldRegen;
                oldRegen = 0;
                return;
            } else if(data.health / data.maxHealth <= activatePercentage) {
                float regen = data.health * CurrentHPRegenPercentage;

                data.healthHandler.regeneration -= oldRegen;
                data.healthHandler.regeneration += regen;

                oldRegen = regen;
            }
        }
    }
}
