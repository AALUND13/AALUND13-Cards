using Photon.Pun;
using System.Collections.Generic;
using UnboundLib;
using UnityEngine;

namespace AALUND13Cards.Cards.Effects {
    public class PickCardFromListEffect : OnAddedEffect {
        public List<GameObject> randomCardsToChoseFrom = new List<GameObject>();
        public int cardCount = 1;

        public override void OnAdded(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats) {
            if(PhotonNetwork.OfflineMode || PhotonNetwork.IsMasterClient) {
                for(int i = 0; i < cardCount; i++) {
                    if(randomCardsToChoseFrom.Count == 0) {
                        LoggerUtils.LogWarn("No cards to choose from. Please add cards to the list.");
                        break;
                    }
                    CardInfo randomCard = randomCardsToChoseFrom.GetRandom<GameObject>().GetComponent<CardInfo>();

                    ModdingUtils.Utils.Cards.instance.AddCardToPlayer(player, randomCard, true, "", 2f, 2f, true);
                    ModdingUtils.Utils.CardBarUtils.instance.ShowAtEndOfPhase(player, randomCard);
                }
            }
        }
    }
}
