using AALUND13Cards.Core.MonoBehaviours;
using ModdingUtils.GameModes;
using System.Collections.Generic;
using UnityEngine;

namespace AALUND13Cards.Curses.MonoBehaviours.CardsEffects {
    public class AddPostProcessingEffectInRounds : MonoBehaviour, IRoundStartHookHandler, IRoundEndHookHandler {
        public List<Material> PostProcessingEffects = new List<Material>();

        private List<PostProcessingEffect> addedEffects = new List<PostProcessingEffect>();
        private CharacterData characterData;

        public void OnRoundEnd() {
            foreach(PostProcessingEffect effect in addedEffects) {
                effect.enabled = false;
            }
        }

        public void OnRoundStart() {
            foreach(PostProcessingEffect effect in addedEffects) {
                effect.enabled = true;
            }
        }

        private void Start() {
            characterData = GetComponentInParent<CharacterData>();

            Camera mainCamera = Camera.main;
            if(mainCamera != null && characterData.view.IsMine) {
                foreach(var effect in PostProcessingEffects) {
                    PostProcessingEffect camEffect = mainCamera.gameObject.AddComponent<PostProcessingEffect>();
                    camEffect.Material = effect;
                    camEffect.enabled = false;
                    addedEffects.Add(camEffect);
                }
            }

            InterfaceGameModeHooksManager.instance.RegisterHooks(this);
        }

        private void OnDestroy() {
            foreach(PostProcessingEffect effect in addedEffects) {
                Destroy(effect);
            }

            InterfaceGameModeHooksManager.instance.RemoveHooks(this);
        }
    }
}
