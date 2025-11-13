using AALUND13Cards.Classes.MonoBehaviours.CardsEffects.Reaper;
using UnityEngine;

namespace AALUND13Cards.Classes.MonoBehaviours.DeathEffects {
    public class GrimFateDeathRespawnEffect : MonoBehaviour, ICustomDeathRespawnEffect {
        public PercentDamageExplosion PercentDamageExplosion;
        public GameObject ActivateObjectWhenRespawn;

        public void OnRespawn(DeathEffect effect, Player player) {
            PercentDamageExplosion.Trigger(player);
            ActivateObjectWhenRespawn.SetActive(true);
        }
    }
}
