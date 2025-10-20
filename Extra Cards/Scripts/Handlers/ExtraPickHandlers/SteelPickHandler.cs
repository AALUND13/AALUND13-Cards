using AALUND13Cards.Core;
using AALUND13Cards.Core.Handlers;
using ModdingUtils.Utils;
using Photon.Pun;
using System.Collections.Generic;
using System.Linq;
using UnboundLib;

namespace AALUND13Cards.ExtraCards.Handlers.ExtraPickHandlers {
    public class SteelPickHandler : ExtraPickHandler {
        public override bool PickConditions(Player player, CardInfo card) {
            if(card.categories.Intersect(AAC_Core.NoSteelCategories).Any()) {
                return false;
            }

            // Get all currest cards of other player
            List<CardInfo> otherPlayerCards = new List<CardInfo>();
            foreach(Player otherPlayer in PlayerStatus.GetEnemyPlayers(player)) {
                if(otherPlayer != player) {
                    otherPlayerCards.AddRange(otherPlayer.data.currentCards);
                }
            }

            return otherPlayerCards.Contains(card);
        }

        public override void OnPickEnd(Player player, CardInfo card) {
            if(PhotonNetwork.OfflineMode || PhotonNetwork.IsMasterClient) {
                // Find all players that have the card
                List<Player> playersWithCard = new List<Player>();
                foreach(Player otherPlayer in PlayerStatus.GetEnemyPlayers(player)) {
                    if(otherPlayer.data.currentCards.Contains(CardChoice.instance.GetSourceCard(card))) {
                        playersWithCard.Add(otherPlayer);
                    }
                }

                if(playersWithCard.Count == 0) return;

                Player randomPlayer = playersWithCard.GetRandom<Player>();
                ModdingUtils.Utils.Cards.instance.RemoveCardFromPlayer(randomPlayer, CardChoice.instance.GetSourceCard(card), ModdingUtils.Utils.Cards.SelectionType.Newest);
            }
        }
    }
}
