using System.Linq;
using UnboundLib;
using UnboundLib.Networking;

namespace AALUND13Card {
    public static class CardRemover {
        public static void RemoveCardFromPlayer(Player player, CardInfo cardInfo, int frameDelay) {
            AALUND13_Cards.Instance.ExecuteAfterFrames(frameDelay, () => {
                int cardId = player.data.currentCards.FindIndex(c => c == cardInfo);
                if(cardId != -1) {
                    NetworkingManager.RPC(typeof(CardRemover), nameof(RPCA_RemoveCardFromPlayer), player.playerID, cardId);
                }
            });
        }

        [UnboundRPC]
        private static void RPCA_RemoveCardFromPlayer(int playerID, int cardId) {
            Player player = PlayerManager.instance.players.Find(p => p.playerID == playerID);

            CardInfo cardInfo = player.data.currentCards[cardId];
            player.data.currentCards.RemoveAt(cardId);

            CardBarButton[] cardBarButtons = ModdingUtils.Utils.CardBarUtils.instance.PlayersCardBar(player).GetComponentsInChildren<CardBarButton>();
            cardBarButtons.ToList().ForEach(cardBarButton => {
                CardInfo card = (CardInfo)cardBarButton.GetFieldValue("card");
                UnityEngine.Debug.Log(cardInfo.cardName + " == " + card.cardName);
                if(card == cardInfo) {
                    UnityEngine.Object.Destroy(cardBarButton.gameObject);
                }
            });
        }
    }
}
