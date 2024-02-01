using Photon.Pun;
using System.Collections.Generic;
using UnboundLib;
using UnityEngine;

namespace AALUND13Card.CustomCards
{
    public class CustomCardRandomPicker : CustomCardAACard
    {

        public List<string> randomCardsToChoseFrom = new List<string>();

        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            if (PhotonNetwork.OfflineMode || PhotonNetwork.IsMasterClient)
            {
                CardInfo randomCard = CardResgester.GetCardsFormString(randomCardsToChoseFrom).GetRandom<GameObject>().GetComponent<CardInfo>();

                ModdingUtils.Utils.Cards.instance.AddCardToPlayer(player, randomCard, true, "", 2f, 2f, true);
                ModdingUtils.Utils.CardBarUtils.instance.ShowAtEndOfPhase(player, randomCard);
            }
        }
    }
}