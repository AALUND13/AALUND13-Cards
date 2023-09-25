using System.Collections.Generic;
using UnboundLib.Cards;
using UnityEngine;

public class CardResgester : MonoBehaviour
{
    public List<GameObject> Cards;
    public List<GameObject> HiddenCards;

    internal static Dictionary<string, GameObject> ModCards = new Dictionary<string, GameObject>();

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
}
