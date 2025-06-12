using AALUND13Cards.Extensions;
using UnityEngine;

namespace AALUND13Cards.MonoBehaviours {
    public class CurrentHPRegenMono : MonoBehaviour {
        private CharacterData data;
        private float oldRegen;

        private void Start() {
            data = GetComponentInParent<CharacterData>();
        }

        public void Update() {
            float regenPercentage = data.GetAdditionalData().CurrentHPRegenPercentage;
            if(regenPercentage != 0) {
                float regen = data.health * regenPercentage;
                
                data.healthHandler.regeneration -= oldRegen;
                data.healthHandler.regeneration += regen;

                oldRegen = regen;
            }
        }
    }
}
