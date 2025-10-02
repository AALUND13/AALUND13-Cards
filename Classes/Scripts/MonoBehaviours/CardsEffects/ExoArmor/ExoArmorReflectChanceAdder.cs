using JARL.Armor;
using UnityEngine;

namespace AALUND13Cards.Classes.MonoBehaviours.CardsEffects.ExoArmor {
    public class ExoArmorReflectChanceAdder : MonoBehaviour {
        public float ReflectChance;

        private ArmorHandler armorHandler;
        private float addedArmorReflectChance;

        private void Awake() {
            armorHandler = GetComponentInParent<ArmorHandler>();
            var armor = (Armors.ExoArmor)armorHandler.GetArmorByType<Armors.ExoArmor>();

            addedArmorReflectChance = Mathf.Max(Mathf.Min(armor.ReflectChance + ReflectChance, 0.8f) - armor.ReflectChance, 0);
            armor.ReflectChance += addedArmorReflectChance;
        }

        private void OnDestroy() {
            ((Armors.ExoArmor)armorHandler.GetArmorByType<Armors.ExoArmor>()).ReflectChance -= addedArmorReflectChance;
        }
    }
}
