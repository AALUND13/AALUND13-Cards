using AALUND13Card.Cards;
using System.Collections.Generic;
using UnboundLib.Cards;
using UnityEngine;


namespace AALUND13Card {
    public class CardResgester : MonoBehaviour {
        public List<GameObject> Cards;
        public List<GameObject> HiddenCards;

        public static Dictionary<string, CardInfo> ModCards = new Dictionary<string, CardInfo>();

        internal void RegisterCards() {
            foreach(var Card in Cards) {
                CardInfo cardInfo = Card.GetComponent<CardInfo>();

                CustomCard.RegisterUnityCard(Card, Card.GetComponent<AACustomCard>().GetModName() ?? AALUND13_Cards.ModInitials, cardInfo.cardName, true, null);
                ModCards.Add(cardInfo.cardName, cardInfo);

                Card.GetComponent<AACustomCard>()?.OnRegister(cardInfo);

                AALUND13_Cards.ModLogger.LogInfo($"Registered Card: {cardInfo.cardName}");
            }
            foreach(var Card in HiddenCards) {
                CardInfo cardInfo = Card.GetComponent<CardInfo>();

                CustomCard.RegisterUnityCard(Card, Card.GetComponent<AACustomCard>().GetModName() ?? AALUND13_Cards.ModInitials, cardInfo.cardName, false, null);
                ModCards.Add(cardInfo.cardName, cardInfo);
                ModdingUtils.Utils.Cards.instance.AddHiddenCard(cardInfo);

                Card.GetComponent<AACustomCard>()?.OnRegister(cardInfo);

                AALUND13_Cards.ModLogger.LogInfo($"Registered Hidden Card: {cardInfo.cardName}");
            }
        }
    }

}