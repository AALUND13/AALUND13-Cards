using AALUND13_Card.Utils;
using ModdingUtils;
using ModdingUtils.Utils;
using Photon.Pun;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnboundLib;
using UnboundLib.Networking;

namespace AALUND13Card.Handlers.ExtraPickHandlers {
    public class SteelPickHandler : ExtraPickHandler {
        public override bool OnExtraPickStart(Player player, CardInfo card) {
            // Get all currest cards of other player
            List<CardInfo> otherPlayerCards = new List<CardInfo>();
            foreach(Player otherPlayer in PlayerManager.instance.players) {
                if(otherPlayer != player) {
                    otherPlayerCards.AddRange(otherPlayer.data.currentCards);
                }
            }

            return otherPlayerCards.Contains(card);
        }

        public override void OnExtraPick(Player player, CardInfo card) {
            if (PhotonNetwork.OfflineMode || PhotonNetwork.IsMasterClient) {
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
