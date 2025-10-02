using AALUND13Cards.Core.MonoBehaviours;
using System.Collections.Generic;
using UnityEngine;

namespace AALUND13Cards.Core.Cards.Effects {
    public class AddPostProcessingEffect : OnAddedEffect {
        public List<Material> PostProcessingEffects = new List<Material>();

        public override void OnAdded(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats) {
            if(!player.data.view.IsMine) return;

            Camera mainCamera = Camera.main;
            if(mainCamera != null) {
                foreach(var effect in PostProcessingEffects) {
                    mainCamera.gameObject.AddComponent<PostProcessingEffect>().Material = effect;
                }
            }
        }
    }
}
