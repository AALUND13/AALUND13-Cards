using AALUND13Card.MonoBehaviours;
using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;
using WillsWackyManagers;
using WillsWackyManagers.Utils;

namespace AALUND13Card.Cards {
    public class CursesEffectCard : AACustomCard {

        public List<Material> PostProcessingEffects = new List<Material>();

        public override void OnRegister(CardInfo cardInfo) {
            CurseManager.instance.RegisterCurse(cardInfo);
        }

        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats) {
            if (!player.data.view.IsMine) return;

            Camera mainCamera = Camera.main;
            if(mainCamera != null) {
                foreach(var effect in PostProcessingEffects) {
                    mainCamera.gameObject.AddComponent<Effect>().Material = effect;
                }
            }
        }

        public override string GetModName() {
            return AALUND13_Cards.CurseInitials;
        }
    }
}