using BepInEx;
using System;
using System.Collections.Generic;
using UnboundLib;
using UnboundLib.Cards;
using UnityEngine;

namespace AALUND13Cards.Core.Cards {
    public class CardResgester : MonoBehaviour {
        public static List<CardInfo> AllModCards = new List<CardInfo>();

        public List<GameObject> Cards;

        private Dictionary<string, CardInfo> ModCards = new Dictionary<string, CardInfo>();

        private void SetupCard(CustomCard customCard) {
            if(customCard == null) return;

            customCard.cardInfo = customCard.GetComponent<CardInfo>();
            customCard.gun = customCard.GetComponent<Gun>();
            customCard.cardStats = customCard.GetComponent<ApplyCardStats>();
            customCard.statModifiers = customCard.GetComponent<CharacterStatModifiers>();
            customCard.block = customCard.gameObject.GetOrAddComponent<Block>();

            customCard.SetupCard(customCard.cardInfo, customCard.gun, customCard.cardStats, customCard.statModifiers, customCard.block);
        }

        public void RegisterCards() {
            foreach(var Card in Cards) {
                CardInfo cardInfo = Card.GetComponent<CardInfo>();
                AACustomCard customCard = Card.GetComponent<AACustomCard>();
                if(cardInfo == null) {
                    UnityEngine.Debug.LogError($"[{AAC_Core.ModName}][Card] {Card.name} does not have a 'CardInfo' component");
                    continue;
                } else if(customCard == null) {
                    UnityEngine.Debug.LogError($"[{AAC_Core.ModName}][Card] {cardInfo.cardName} does not have a 'AACustomCard' component");
                    continue;
                } else if(!customCard.RequireMod.IsNullOrWhiteSpace() && !AAC_Core.Plugins.Exists(plugin => plugin.Info.Metadata.GUID == customCard.RequireMod)) {
                    UnityEngine.Debug.LogWarning($"[{AAC_Core.ModName}][Card] {cardInfo.cardName} does not have the require mod of '{customCard.RequireMod}'");
                    continue;
                }

                try {
                    SetupCard(customCard);
                } catch(Exception e) {
                    UnityEngine.Debug.LogError($"[{AAC_Core.ModName}][Card] {cardInfo.cardName} failed to setup the card: {e}");
                    continue;
                }
                customCard.RegisterUnityCard((registerCardInfo) => {
                    try {
                        customCard.Register(registerCardInfo);
                    } catch(Exception e) {
                        UnityEngine.Debug.LogError($"[{AAC_Core.ModName}][Card] {registerCardInfo.cardName} failed to execute the 'Register' method: {e}");
                    }
                });

                UnityEngine.Debug.Log($"[{AAC_Core.ModName}][Card] Registered Card: {cardInfo.cardName}");
                ModCards.Add(cardInfo.cardName, cardInfo);
                AllModCards.Add(cardInfo);
            }
        }
    }
}
