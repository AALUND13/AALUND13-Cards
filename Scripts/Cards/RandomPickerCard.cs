using Photon.Pun;
using System.Collections.Generic;
using UnboundLib;
using UnityEngine;

namespace AALUND13Card.Cards {
    public class RandomPickerCard : CustomCardAACard {

        public List<GameObject> randomCardsToChoseFrom = new List<GameObject>();

        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats) {
            if(PhotonNetwork.OfflineMode || PhotonNetwork.IsMasterClient) {
                CardInfo randomCard = randomCardsToChoseFrom.GetRandom<GameObject>().GetComponent<CardInfo>();

                ModdingUtils.Utils.Cards.instance.AddCardToPlayer(player, randomCard, true, "", 2f, 2f, true);
                ModdingUtils.Utils.CardBarUtils.instance.ShowAtEndOfPhase(player, randomCard);
            }
        }
    }
}