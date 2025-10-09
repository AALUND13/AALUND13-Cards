using JARL.Armor;
using JARL.Armor.Builtin;
using UnityEngine;

namespace AALUND13Cards.Armors.CardsEffects {
    public class RestorationMono : MonoBehaviour {
        public float MnRegenCooldown = 0;

        private bool isActive = false;
        private float regenAmount = 0f;

        private CharacterData data;
        private ArmorHandler armorHandler;

        private void Start() {
            data = GetComponentInParent<CharacterData>();
            armorHandler = GetComponentInParent<ArmorHandler>();
        }

        private void Update() {
            if(!isActive && data.health >= data.maxHealth) {
                regenAmount = data.healthHandler.regeneration;
                AddRegenerationAmountToAllArmors(regenAmount);
                isActive = true;
            } else if(isActive && data.health >= data.maxHealth && regenAmount != data.healthHandler.regeneration) {
                AddRegenerationAmountToAllArmors(-regenAmount);
                regenAmount = data.healthHandler.regeneration;
                AddRegenerationAmountToAllArmors(regenAmount);
            } else if(isActive && data.health < data.maxHealth) {
                AddRegenerationAmountToAllArmors(-regenAmount);
                isActive = false;
            }
        }

        private void AddRegenerationAmountToAllArmors(float regenerationAmount) {
            foreach(var armor in armorHandler.ActiveArmors) {
                if(armor.HasArmorTag("NoRestorationRegen")) continue;

                if(armor.ArmorRegenCooldownSeconds < MnRegenCooldown) {
                    armor.ArmorRegenCooldownSeconds = MnRegenCooldown;
                }

                armor.ArmorRegenerationRate += regenerationAmount;
            }
        }
    }
}
