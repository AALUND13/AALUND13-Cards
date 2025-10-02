using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AALUND13Cards.Core.Cards.Conditions {
    public class MinPlayersCondition : CardCondition {
        public int MinPlayers = 3;

        public override bool IsPlayerAllowedCard(Player player) {
            return PlayerManager.instance.players.Count >= MinPlayers;
        }
    }
}
