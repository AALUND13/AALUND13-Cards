using JARL.Armor;
using UnityEngine;

namespace AALUND13Cards.MonoBehaviours.CardsEffects.ExoArmor {
    public class ExoArmorSpawnDamageEffect : MonoBehaviour {
        private DamageSpawnObjects damageSpawnObjects;
        private ArmorHandler armorHandler;

        private void Awake() {
            damageSpawnObjects = GetComponent<DamageSpawnObjects>();
            armorHandler = GetComponentInParent<ArmorHandler>();
            ((AALUND13Cards.Armors.ExoArmor)armorHandler.GetArmorByType<AALUND13Cards.Armors.ExoArmor>()).OnArmorDamaged += OnArmorDamaged;
        }

        private void OnDestroy() {
            ((AALUND13Cards.Armors.ExoArmor)armorHandler.GetArmorByType<AALUND13Cards.Armors.ExoArmor>()).OnArmorDamaged -= OnArmorDamaged;
        }

        private void OnArmorDamaged(float damage) {
            damageSpawnObjects.SpawnDamage(Vector2.up * damage);
        }
    }
}
