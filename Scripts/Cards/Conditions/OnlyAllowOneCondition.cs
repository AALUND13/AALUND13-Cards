using System.Linq;

namespace AALUND13Cards.Cards.Conditions {
    public class OnlyAllowOneCondition : CardCondition {
        public override bool IsPlayerAllowedCard(Player player) {
            CardInfo[] cards = PlayerManager.instance.players.SelectMany(p => p.data.currentCards).ToArray();

            if(cards.Contains(CardInfo)) return false;
            return true;
        }
    }
}
