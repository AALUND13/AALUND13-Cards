using AALUND13Cards.Core;
using AALUND13Cards.Core.Extensions;
using AALUND13Cards.Core.Handlers;
using AALUND13Cards.ExtraCards.Cards;
using ModdingUtils.Utils;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using System.Linq;
using UnboundLib;
using UnboundLib.Networking;

namespace AALUND13Cards.ExtraCards.Handlers.ExtraPickHandlers {
    public class CursedSteelPickHandler : ExtraPickHandler {
        public override int HandSize => 3;

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

        public override void OnPickStart(Player player) {
            player.data.GetCustomStatsRegistry().GetOrCreate<ExtraCardsStats>().FullCurseDraws = true;
        }

        public override void OnPickEnd(Player player, CardInfo card) {
            LoggerUtils.LogInfo("Trying to steel a card");
            NetworkingManager.RPC(typeof(CursedSteelPickHandler), nameof(RPCA_SteelCard), CardChoice.instance.GetSourceCard(card).name, player.playerID);

            player.data.GetCustomStatsRegistry().GetOrCreate<ExtraCardsStats>().FullCurseDraws = false;
        }

        [UnboundRPC]
        private static void RPCA_SteelCard(string cardObjectName, int playerId) {
            if(!PhotonNetwork.OfflineMode && !PhotonNetwork.IsMasterClient) return;

            Player player = PlayerManager.instance.players.Find(p => p.playerID == playerId);
            CardInfo card = ModdingUtils.Utils.Cards.instance.GetCardWithObjectName(cardObjectName);

            List<Player> playersWithCard = new List<Player>();
            foreach(Player otherPlayer in PlayerStatus.GetEnemyPlayers(player)) {
                if(otherPlayer.data.currentCards.Contains(card)) {
                    playersWithCard.Add(otherPlayer);
                }
            }

            if(playersWithCard.Count == 0) return;

            Player randomPlayer = playersWithCard.GetRandom<Player>();
            LoggerUtils.LogInfo($"Steeling a card '{card.cardName}' from player with id of {randomPlayer.playerID}");

            ModdingUtils.Utils.Cards.instance.RemoveCardFromPlayer(randomPlayer, card, ModdingUtils.Utils.Cards.SelectionType.Newest);
        }
    }
}
