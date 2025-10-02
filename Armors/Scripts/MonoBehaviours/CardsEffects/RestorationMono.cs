using JARL.Armor;
using JARL.Armor.Builtin;
using UnityEngine;

namespace AALUND13Cards.Armors.CardsEffects {
    public class RestorationMono : MonoBehaviour {
        private bool isActive = false;
        private float regenerationAmount = 0f;

        private CharacterData data;
        private ArmorHandler armorHandler;



        private void Start() {
            data = GetComponentInParent<CharacterData>();
            armorHandler = GetComponentInParent<ArmorHandler>();
        }

        private void Update() {
            if(!isActive && data.health >= data.maxHealth) {
                regenerationAmount = data.healthHandler.regeneration;
                armorHandler.GetArmorByType<DefaultArmor>().ArmorRegenerationRate += regenerationAmount;
                isActive = true;
            } else if(isActive && data.health >= data.maxHealth && regenerationAmount != data.healthHandler.regeneration) {
                armorHandler.GetArmorByType<DefaultArmor>().ArmorRegenerationRate -= regenerationAmount;
                regenerationAmount = data.healthHandler.regeneration;
                armorHandler.GetArmorByType<DefaultArmor>().ArmorRegenerationRate += regenerationAmount;
            } else if(isActive && data.health < data.maxHealth) {
                armorHandler.GetArmorByType<DefaultArmor>().ArmorRegenerationRate -= data.healthHandler.regeneration;
                isActive = false;
            }
        }
    }
}
