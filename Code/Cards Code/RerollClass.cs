using ClassesManagerReborn;
using ModdingUtils;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnboundLib;
using UnityEngine;

public class RerollClass : AACustomCard
{
    public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats, CardInfo card)
    {
        if (PhotonNetwork.OfflineMode || PhotonNetwork.IsMasterClient)
        {
            StartCoroutine(OnAddIEnumerator(player, gun, gunAmmo, data, health, gravity, block, characterStats, card));

            //List<ClassObject> otherClassObject = ClassesRegistry.GetClassObjects(CardType.Entry).FindAll(classObj => classObj != classObjects);
            //foreach (ClassObject classObj in otherClassObject)
            //{
            //    UnityEngine.Debug.Log(classObj.card.cardName);
            //}



            //ModdingUtils.Utils.CardBarUtils.instance.ShowAtEndOfPhase(player, randomCard);
        }
    }

    public IEnumerator OnAddIEnumerator(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats, CardInfo card)
    {
        ClassObject currentClassObject = ClassesRegistry.GetClassObjects(CardType.Entry).Find(classObj => player.data.currentCards.Find(c => c.cardName == classObj.card.cardName));
        List<ClassObject> allClassObjects = ClassesRegistry.GetClassObjects(~CardType.Entry).FindAll(classObj => player.data.currentCards.Find(c => c.cardName == classObj.card.cardName));

        if (currentClassObject != null && allClassObjects != null)
        {
            // Get a random class object
            ClassObject newClassObject = ClassesRegistry.GetClassObjects(CardType.Entry).GetRandom<ClassObject>();

            // Replace the current class card with a random one
            yield return ModdingUtils.Utils.Cards.instance.ReplaceCard(player, currentClassObject.card, newClassObject.card, "", 2f, 2f, ModdingUtils.Utils.Cards.SelectionType.All);

            // Log the name of the new class cardyield return
            UnityEngine.Debug.Log("New Class Card: " + newClassObject.card.cardName);

            // Find the replacement cards for the new class
            List<CardInfo> replacementCards = player.data.currentCards.FindAll(cardInfo => ClassesRegistry.GetClassInfos(~CardType.Entry).Contains(cardInfo));

            // Log the names of the replacement cards
            foreach (CardInfo replacementCard in replacementCards)
            {
                UnityEngine.Debug.Log("Replacement Card: " + replacementCard.cardName);
            }
        }

        yield break;
    }

}

