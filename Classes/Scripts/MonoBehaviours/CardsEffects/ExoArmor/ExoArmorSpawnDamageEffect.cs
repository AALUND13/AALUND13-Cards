using AALUND13Cards.Core.MonoBehaviours;
using JARL.Armor;
using UnityEngine;

namespace AALUND13Cards.Classes.MonoBehaviours.CardsEffects.ExoArmor {
    public class ExoArmorSpawnDamageEffect : MonoBehaviour {
        private DamageSpawnObjects damageSpawnObjects;
        private ArmorHandler armorHandler;

        private void Awake() {
            damageSpawnObjects = GetComponent<DamageSpawnObjects>();
            armorHandler = GetComponentInParent<ArmorHandler>();
            ((Armors.ExoArmor)armorHandler.GetArmorByType<Armors.ExoArmor>()).OnArmorDamaged += OnArmorDamaged;
        }

        private void OnDestroy() {
            ((Armors.ExoArmor)armorHandler.GetArmorByType<Armors.ExoArmor>()).OnArmorDamaged -= OnArmorDamaged;
        }

        private void OnArmorDamaged(float damage) {
            damageSpawnObjects.SpawnDamage(Vector2.up * damage);
        }
    }
}
