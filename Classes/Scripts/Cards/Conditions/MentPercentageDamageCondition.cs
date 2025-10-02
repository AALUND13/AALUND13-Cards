using AALUND13Cards.Classes.Cards;
using AALUND13Cards.Core.Cards.Conditions;
using AALUND13Cards.Core.Extensions;

namespace AALUND13Cards.Classes.Cards.Conditions {
    public class MentPercentageDamageCondition : CardCondition {
        public override bool IsPlayerAllowedCard(Player player) {
            return player.data.GetCustomStatsRegistry().GetOrCreate<ReaperStats>().ScalingPercentageDamage < player.data.GetCustomStatsRegistry().GetOrCreate<ReaperStats>().ScalingPercentageDamageCap;
        }
    }
}
