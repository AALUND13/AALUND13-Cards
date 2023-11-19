using ClassesManagerReborn;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnboundLib;
using UnityEngine;
using ModdingUtils.Utils;
using Photon.Pun;

public class RerollClassManager : MonoBehaviour
{
    public static RerollClassManager instance;
    
    public List<Player> rerollClassPlayers = new List<Player>();
    
    public void Start()
    {
        UnityEngine.Debug.Log("RerollClassManager Spawned");
        instance = this;
    }

    public IEnumerator RerollClassPlayerIEnumerator(Player player)
    {
        UnityEngine.Debug.Log("Starting");
        yield return null;

        List<CardInfo> currentCards = player.data.currentCards.ToList();
        Cards.instance.RemoveAllCardsFromPlayer(player);

        foreach (CardInfo card in currentCards)
        {
            UnityEngine.Debug.Log(card.cardName);
            ClassObject cardClassObject = ClassesRegistry.Get(card);
            UnityEngine.Debug.Log("After");

            if (cardClassObject == null || cardClassObject.type.Equals(CardType.NonClassCard))
            {
                UnityEngine.Debug.Log($"{player.playerID} -- {card.cardName}");
                Cards.instance.AddCardToPlayer(player, card, false, "", 0f, 0f);
            }
            else if (cardClassObject.type == CardType.Entry)
            {
                CardInfo replacementEntryCard = ClassesRegistry.GetClassInfos(cardClassObject.type).GetRandom<CardInfo>();
                if (replacementEntryCard != null)
                {
                    Cards.instance.AddCardToPlayer(player, replacementEntryCard, false, "", 0f, 0f);
                }
            }
            else if (cardClassObject.type != CardType.Entry)
            {
                List<ClassObject> classObjects = ClassesRegistry.GetClassObjects(~CardType.Entry)
                    .Where(classObj => Cards.instance.PlayerIsAllowedCard(player, classObj.card) && Cards.active.Contains(classObj.card))
                    .ToList();

                if (classObjects.Count > 0)
                {
                    ClassObject replacementEntryCard = classObjects.GetRandom<ClassObject>();
                    if (replacementEntryCard != null)
                    {
                        Cards.instance.AddCardToPlayer(player, replacementEntryCard.card, false, "", 0f, 0f);
                    }
                }
            }

            yield return new WaitForSeconds(0.5f);
        }

        yield break;
    }



    public IEnumerator RerollPlayer()
    {
        if (!(PhotonNetwork.OfflineMode || PhotonNetwork.IsMasterClient)) yield break;
        for (int i = rerollClassPlayers.Count - 1; i >= 0; i--)
        {
            Player rerollClassPlayer = rerollClassPlayers[i];
            yield return RerollClassPlayerIEnumerator(rerollClassPlayer);
            rerollClassPlayers.Remove(rerollClassPlayers[i]);
        }
        yield break;
    }

    public void AddPlayerToRerollClassPlayer(Player player)
    {
        if (!rerollClassPlayers.Contains(player))
        {
            rerollClassPlayers.Add(player);
        }
    }
}
