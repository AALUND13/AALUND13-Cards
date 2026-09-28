using AALUND13Cards.Core.Cards.Effects;
using CardChoiceSpawnUniqueCardPatch.CustomCategories;
using JARL.Bases;
using System.Collections.Generic;
using UnityEngine;
using WillsWackyManagers.Utils;

namespace AALUND13Cards.Core.Cards {
    public class AACustomCard : CustomUnityCard {
        private CardInfoDisplayer cardInfoDisplayer;

        public string RequireMod = "";
        public bool IsCursed = false;

        [Header("More Info")]
        public bool ctrlForMoreInfo = false;
        public string moreInfoCardDescription = string.Empty;
        public CardInfoStat[] moreInfoCardStats = new CardInfoStat[0];

        private bool showingMoreInfo = false;
        private string mainCardDescription = string.Empty;
        private CardInfoStat[] mainCardStats = new CardInfoStat[0];

        public override void OnRegister(CardInfo cardInfo) {
            if (IsCursed) {
                CurseManager.instance.RegisterCurse(cardInfo);
                List<CardCategory> cardCategoriesList = new List<CardCategory>(cardInfo.categories) {
                    CustomCardCategories.instance.CardCategory("Curse")
                };
                cardInfo.categories = cardCategoriesList.ToArray();
            }

            mainCardDescription = cardInfo.cardDestription;
            mainCardStats = cardInfo.cardStats;
        }

        public override void OnCallback() {
            cardInfoDisplayer = GetComponentInChildren<CardInfoDisplayer>();
        }


        private void Update() {
            if(ctrlForMoreInfo) {
                ToggleMoreInfo(Input.GetKey(KeyCode.LeftControl));
            }
        }


        public override void OnReassignCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats) {
            CustomStatModifers[] statModifers = GetComponents<CustomStatModifers>();
            foreach (CustomStatModifers statModifer in statModifers) {
                statModifer.OnReassign(player);
            }
        }

        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats) {
            OnAddedEffect[] onAddedEffects = GetComponents<OnAddedEffect>();
            foreach (OnAddedEffect onAddedEffect in onAddedEffects) {
                onAddedEffect.OnAdded(player, gun, gunAmmo, data, health, gravity, block, characterStats);
            }
        }


        public override string GetModName() {
            return AAC_Core.ModInitials;
        }


        public void ToggleMoreInfo(bool toggle) {
            if (showingMoreInfo != toggle) {
                cardInfo.cardDestription = toggle ? moreInfoCardDescription : mainCardDescription;
                cardInfo.cardStats = toggle ? moreInfoCardStats : mainCardStats;
                showingMoreInfo = toggle;
                RedrawCard();
            }
        }

        private void RedrawCard() {
            if (cardInfoDisplayer != null) {
                foreach (GameObject gameObj in cardInfoDisplayer.grid.transform) {
                    if (gameObj != cardInfoDisplayer.effectText.gameObject && gameObj.activeSelf) {
                        Destroy(gameObj);
                    }
                }

                cardInfoDisplayer.DrawCard(cardInfo.cardStats, cardInfo.cardName, cardInfo.cardDestription, cardInfo.sprite, GetComponent<Gun>()?.useCharge ?? false);
            }
        }
    }
}