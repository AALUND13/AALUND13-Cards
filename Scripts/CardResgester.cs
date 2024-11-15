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
                CustomCard.RegisterUnityCard(Card, AALUND13_Cards.modInitials, Card.GetComponent<CardInfo>().cardName, true, null);
            }
            foreach(var Card in HiddenCards) {
                CustomCard.RegisterUnityCard(Card, AALUND13_Cards.modInitials, Card.GetComponent<CardInfo>().cardName, false, null);
                ModdingUtils.Utils.Cards.instance.AddHiddenCard(Card.GetComponent<CardInfo>());
            }
        }
    }

}