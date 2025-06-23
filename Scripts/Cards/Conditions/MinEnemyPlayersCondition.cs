using ModdingUtils.Utils;

namespace AALUND13Cards.Cards.Conditions {
    internal class MinEnemyPlayersCondition : CardCondition {
        public int MinEnemyPlayers = 2;
        public override bool IsPlayerAllowedCard(Player player) {
            return PlayerStatus.GetEnemyPlayers(player).Count >= MinEnemyPlayers;
        }
    }
}
