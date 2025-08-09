using AALUND13Cards.MonoBehaviours;
using System.Collections.Generic;
using UnboundLib;
using UnityEngine;

namespace AALUND13Cards.Cards.Effects {
    public class AddPostProcessingEffect : OnAddedEffect {
        public List<Material> PostProcessingEffects = new List<Material>();

        public override void OnAdded(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats) {
            if(!player.data.view.IsMine) return;

            AALUND13_Cards.Instance.ExecuteAfterFrames(1, () => {
                Camera mainCamera = Camera.main;
                if(mainCamera != null) {
                    foreach(var effect in PostProcessingEffects) {
                        mainCamera.gameObject.AddComponent<Effect>().Material = effect;
                    }
                }
            });
        }
    }
}
