using AALUND13Cards.Extensions;

namespace AALUND13Cards.Cards.Conditions {
    public class MentPercentageDamageCondition : CardCondition {
        public override bool IsPlayerAllowedCard(Player player) {
            return player.data.GetAdditionalData().ScalingPercentageDamage < player.data.GetAdditionalData().ScalingPercentageDamageCap;
        }
    }
}
