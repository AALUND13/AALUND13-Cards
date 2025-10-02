namespace AALUND13Cards.Core.Cards.Conditions {
    public class PlayerHaveRegenCondition : CardCondition {
        public float MinRegen;

        public override bool IsPlayerAllowedCard(Player player) {
            return player.data.healthHandler.regeneration >= MinRegen;
        }
    }
}
