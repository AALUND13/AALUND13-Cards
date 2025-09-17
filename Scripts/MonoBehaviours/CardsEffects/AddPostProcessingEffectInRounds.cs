using ModdingUtils.GameModes;
using System.Collections.Generic;
using UnityEngine;

namespace AALUND13Cards.MonoBehaviours.CardsEffects {
    public class AddPostProcessingEffectInRounds : MonoBehaviour, IRoundStartHookHandler, IRoundEndHookHandler {
        public List<Material> PostProcessingEffects = new List<Material>();

        private List<Effect> addedEffects = new List<Effect>();
        private CharacterData characterData;

        public void OnRoundEnd() {
            foreach(Effect effect in addedEffects) {
                effect.enabled = false;
            }
        }

        public void OnRoundStart() {
            foreach(Effect effect in addedEffects) {
                effect.enabled = true;
            }
        }

        private void Start() {
            characterData = GetComponentInParent<CharacterData>();

            Camera mainCamera = Camera.main;
            if(mainCamera != null && characterData.view.IsMine) {
                foreach(var effect in PostProcessingEffects) {
                    Effect camEffect = mainCamera.gameObject.AddComponent<Effect>();
                    camEffect.Material = effect;
                    camEffect.enabled = false;
                    addedEffects.Add(camEffect);
                }
            }

            InterfaceGameModeHooksManager.instance.RegisterHooks(this);
        }

        private void OnDestroy() {
            foreach(Effect effect in addedEffects) {
                Destroy(effect);
            }

            InterfaceGameModeHooksManager.instance.RemoveHooks(this);
        }
    }
}
