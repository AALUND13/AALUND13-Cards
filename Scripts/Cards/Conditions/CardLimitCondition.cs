using System.Linq;

namespace AALUND13Cards.Cards.Conditions {
    public class CardLimitCondition : CardCondition {
        public int AllowedAmount = 1;

        public override bool IsPlayerAllowedCard(Player player) {
            CardInfo[] cards = PlayerManager.instance.players.SelectMany(p => p.data.currentCards).ToArray();
            int cardCount = cards.Count(c => c == CardInfo);

            if(cardCount >= AllowedAmount) {
                return false;
            } else {
                return true;
            }
        }
    }
}
