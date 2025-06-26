using System.Linq;

namespace AALUND13Cards.Cards.Conditions {
    public class PickPhaseLimitCondition : CardCondition {
        public int AllowedPickCount = 1;

        public override bool IsPlayerAllowedCard(Player player) {
            int amountAlreadyPicked = PickCardTracker.instance.CardPickedInPickPhase.Count(c => c == CardInfo);
            if(amountAlreadyPicked >= AllowedPickCount) {
                return false;
            } else {
                return true;
            }
        }
    }
}
