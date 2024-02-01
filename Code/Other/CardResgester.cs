using System.Collections.Generic;
using UnboundLib.Cards;
using UnityEngine;


namespace AALUND13Card
{
    public class CardResgester : MonoBehaviour
    {
        public List<GameObject> Cards;
        public List<GameObject> HiddenCards;

        public static Dictionary<string, GameObject> ModCards = new Dictionary<string, GameObject>();

        internal void RegisterCards()
        {
            foreach (var Card in Cards)
            {
                CustomCard.RegisterUnityCard(Card, AALUND13_Cards.modInitials, Card.GetComponent<CardInfo>().cardName, true, null);
                ModCards.Add(Card.GetComponent<CardInfo>().cardName, Card);
            }
            foreach (var Card in HiddenCards)
            {
                CustomCard.RegisterUnityCard(Card, AALUND13_Cards.modInitials, Card.GetComponent<CardInfo>().cardName, false, null);
                ModdingUtils.Utils.Cards.instance.AddHiddenCard(Card.GetComponent<CardInfo>());
                ModCards.Add(Card.GetComponent<CardInfo>().cardName, Card);
            }
        }

        internal static List<GameObject> GetCardsFormString(List<string> cardsOfString)
        {
            List<GameObject> ModCardsObject = new List<GameObject>();
            foreach (string Card in cardsOfString)
            {
                ModCardsObject.Add(ModCards[Card]);
            }
            return ModCardsObject;
        }
    }

}