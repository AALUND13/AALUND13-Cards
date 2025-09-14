using JARL.Armor;
using UnityEngine;

namespace AALUND13Cards.MonoBehaviours.CardsEffects.PowerArmor {
    public class ExoArmorDamageReductionAdder : MonoBehaviour {
        public const float DamageReductionMax = 100;
        public float DamageReduction;

        private ArmorHandler armorHandler;
        private float addedArmorDamageReduction;

        private void Awake() {
            armorHandler = GetComponentInParent<ArmorHandler>();
            float ArmorDamageReduction = ((AALUND13Cards.Armors.ExoArmor)armorHandler.GetArmorByType<AALUND13Cards.Armors.ExoArmor>()).ArmorDamageReduction;
            if(addedArmorDamageReduction > 0) {
                addedArmorDamageReduction = Mathf.Max(Mathf.Min(ArmorDamageReduction + DamageReduction, 0.8f) - ArmorDamageReduction, 0);
            } else {
                addedArmorDamageReduction = DamageReduction;
            }

            ((AALUND13Cards.Armors.ExoArmor)armorHandler.GetArmorByType<AALUND13Cards.Armors.ExoArmor>()).ArmorDamageReduction += addedArmorDamageReduction;
        }

        private void OnDestroy() {
            ((AALUND13Cards.Armors.ExoArmor)armorHandler.GetArmorByType<AALUND13Cards.Armors.ExoArmor>()).ArmorDamageReduction -= addedArmorDamageReduction;
        }
    }
}
