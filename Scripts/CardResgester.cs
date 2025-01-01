using AALUND13Card.Cards;
using System.Collections.Generic;
using System.Linq;
using UnboundLib.Cards;
using UnityEngine;


namespace AALUND13Card {
    public class CardResgester : MonoBehaviour {
        public List<GameObject> Cards;
        public List<GameObject> HiddenCards;

        internal void RegisterCards() {
            foreach(var Card in Cards) {
                CardInfo cardInfo = Card.GetComponent<CardInfo>();

                CustomCard.RegisterUnityCard(Card, Card.GetComponent<AACustomCard>().GetModName() ?? AALUND13_Cards.modInitials, cardInfo.cardName, true, null);
                Card.GetComponent<AACustomCard>()?.OnRegister(cardInfo);
                AALUND13_Cards.logger.LogInfo($"Registered Card: {cardInfo.cardName}");
            }
            foreach(var Card in HiddenCards) {
                CardInfo cardInfo = Card.GetComponent<CardInfo>();

                CustomCard.RegisterUnityCard(Card, Card.GetComponent<AACustomCard>().GetModName() ?? AALUND13_Cards.modInitials, cardInfo.cardName, false, null);
                ModdingUtils.Utils.Cards.instance.AddHiddenCard(cardInfo);
                Card.GetComponent<AACustomCard>()?.OnRegister(cardInfo);
                AALUND13_Cards.logger.LogInfo($"Registered Hidden Card: {cardInfo.cardName}");
            }
        }
    }

}