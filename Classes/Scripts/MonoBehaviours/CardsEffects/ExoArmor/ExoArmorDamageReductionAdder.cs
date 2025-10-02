using JARL.Armor;
using UnityEngine;

namespace AALUND13Cards.Classes.MonoBehaviours.CardsEffects.ExoArmor {
    public class ExoArmorDamageReductionAdder : MonoBehaviour {
        public float DamageReduction;

        private ArmorHandler armorHandler;
        private float addedArmorDamageReduction;

        private void Awake() {
            armorHandler = GetComponentInParent<ArmorHandler>();
            float ArmorDamageReduction = ((Armors.ExoArmor)armorHandler.GetArmorByType<Armors.ExoArmor>()).ArmorDamageReduction;
            if(addedArmorDamageReduction > 0) {
                addedArmorDamageReduction = Mathf.Max(Mathf.Min(ArmorDamageReduction + DamageReduction, 0.8f) - ArmorDamageReduction, 0);
            } else {
                addedArmorDamageReduction = DamageReduction;
            }

            ((Armors.ExoArmor)armorHandler.GetArmorByType<Armors.ExoArmor>()).ArmorDamageReduction += addedArmorDamageReduction;
        }

        private void OnDestroy() {
            ((Armors.ExoArmor)armorHandler.GetArmorByType<Armors.ExoArmor>()).ArmorDamageReduction -= addedArmorDamageReduction;
        }
    }
}
