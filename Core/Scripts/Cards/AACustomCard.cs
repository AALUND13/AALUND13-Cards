using AALUND13Cards.Core.Cards.Effects;
using CardChoiceSpawnUniqueCardPatch.CustomCategories;
using JARL.Bases;
using System.Collections.Generic;
using WillsWackyManagers.Utils;
using ToggleCardsCategories;
using UnboundLib.Cards;
using UnityEngine;

namespace AALUND13Cards.Core.Cards {
    public class AACustomCard : CustomUnityCard {
        public string RequireMod = "";
        public bool IsCursed = false;
        
        public override void OnRegister(CardInfo cardInfo) {
            if(IsCursed) {
                CurseManager.instance.RegisterCurse(cardInfo);
                List<CardCategory> cardCategoriesList = new List<CardCategory>(cardInfo.categories) {
                    CustomCardCategories.instance.CardCategory("Curse")
                };
                cardInfo.categories = cardCategoriesList.ToArray();
            }
        }

        public override void OnReassignCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats) {
            CustomStatModifers[] statModifers = GetComponents<CustomStatModifers>();
            foreach(CustomStatModifers statModifer in statModifers) {
                statModifer.OnReassign(player);
            }
        }

        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats) {
            OnAddedEffect[] onAddedEffects = GetComponents<OnAddedEffect>();
            foreach(OnAddedEffect onAddedEffect in onAddedEffects) {
                onAddedEffect.OnAdded(player, gun, gunAmmo, data, health, gravity, block, characterStats);
            }
        }

        public override string GetModName() {
            return AAC_Core.ModInitials;
        }
    }
}