using AALUND13Cards.Core.Extensions;
using AALUND13Cards.Devil.Cards;
using TabInfo.Utils;

namespace AALUND13Cards.Devil {
    internal class TabinfoInterface {
        public static void Setup() {
            var aaStatsCategory = AALUND13Cards.Core.TabinfoInterface.GetOrCreateCategory("AA Stats", 6);

            TabInfoManager.RegisterStat(aaStatsCategory, "Disable Block Cooldown", (p) => GetDevilStatsFromPlayer(p).DisbaleBlockTime,
                (p) => $"{GetDevilStatsFromPlayer(p).DisbaleBlockTime}");
            TabInfoManager.RegisterStat(aaStatsCategory, "Fixed Block Cooldown", (p) => GetDevilStatsFromPlayer(p).FixedBlockCooldown != 0,
                (p) => $"{GetDevilStatsFromPlayer(p).FixedBlockCooldown}s");
        }

        private static DevilCardsStats GetDevilStatsFromPlayer(Player player) {
            return player.data.GetCustomStatsRegistry().GetOrCreate<DevilCardsStats>();
        }
    }
}
