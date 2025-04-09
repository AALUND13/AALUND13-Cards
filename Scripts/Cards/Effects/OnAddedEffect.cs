using UnityEngine;

namespace AALUND13Card.Cards.Effects {
    public abstract class OnAddedEffect : MonoBehaviour {
        public abstract void OnAdded(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats);
    }
}
