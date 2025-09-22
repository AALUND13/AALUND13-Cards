using BepInEx;
using BepInEx.Bootstrap;
using JARL.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using UnboundLib;
using UnboundLib.Cards;
using UnityEngine;

namespace AALUND13Cards.Cards {
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
                CustomUnityCard customCard = Card.GetComponent<CustomUnityCard>();
                if(cardInfo == null) {
                    UnityEngine.Debug.LogError($"[{AALUND13_Cards.ModName}][Card] {Card.name} does not have a 'CardInfo' component");
                    continue;
                } else if(customCard == null) {
                    UnityEngine.Debug.LogError($"[{AALUND13_Cards.ModName}][Card] {cardInfo.cardName} does not have a 'CustomUnityCard' component");
                    continue;
                }

                try {
                    SetupCard(customCard);
                } catch(Exception e) {
                    UnityEngine.Debug.LogError($"[{AALUND13_Cards.ModName}][Card] {cardInfo.cardName} failed to setup the card: {e}");
                    continue;
                }
                customCard.RegisterUnityCard((registerCardInfo) => {
                    try {
                        customCard.Register(registerCardInfo);
                    } catch(Exception e) {
                        UnityEngine.Debug.LogError($"[{AALUND13_Cards.ModName}][Card] {registerCardInfo.cardName} failed to execute the 'Register' method: {e}");
                    }
                });

                UnityEngine.Debug.Log($"[{AALUND13_Cards.ModName}][Card] Registered Card: {cardInfo.cardName}");
                ModCards.Add(cardInfo.cardName, cardInfo);
                AllModCards.Add(cardInfo);
            }
        }
    }
}
