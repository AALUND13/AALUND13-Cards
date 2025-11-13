using AALUND13Cards.Core;
using AALUND13Cards.Core.Handlers;
using UnboundLib;
using UnityEngine;

namespace AALUND13Cards.Classes.MonoBehaviours.CardsEffects.Reaper {
    public class GrimFateBehaviour : MonoBehaviour {
        public GameObject GrimFateDeathEffect;

        private CharacterData data;
        private bool alreadyTrigger;

        private void OnDeath() {
            if(alreadyTrigger) return;

            GameObject oldDeathEffect = data.healthHandler.deathEffectPhoenix;
            data.healthHandler.deathEffectPhoenix = GrimFateDeathEffect;
            AAC_Core.Instance.ExecuteAfterFrames(1, () => { data.healthHandler.deathEffectPhoenix = oldDeathEffect; });

            alreadyTrigger = true;
        }

        private void OnTrueDeath() {
            alreadyTrigger = false;
        }


        private void Start() {
            data = GetComponentInParent<CharacterData>();

            DeathActionHandler.Instance.RegisterReviveAction(data.player, OnDeath);
            DeathActionHandler.Instance.RegisterTrueDeathAction(data.player, OnTrueDeath);
        }

        private void OnDestroy() {
            DeathActionHandler.Instance.DeregisterReviveAction(data.player, OnDeath);
            DeathActionHandler.Instance.DeregisterTrueDeathAction(data.player, OnTrueDeath);
        }
    }
}
