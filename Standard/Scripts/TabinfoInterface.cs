using AALUND13Cards.Core.Extensions;
using AALUND13Cards.Standard.Cards;
using TabInfo.Utils;

namespace AALUND13Cards.Standard {
    internal class TabinfoInterface {
        public static void Setup() {
            var aaStatsCategory = AALUND13Cards.Core.TabinfoInterface.GetOrCreateCategory("AA Stats", 6);

            // Delayed Damage
            TabInfoManager.RegisterStat(aaStatsCategory, "Delay Damage", (p) => GetStandardStatsFromPlayer(p).secondToDealDamage != 0,
                (p) => $"{GetStandardStatsFromPlayer(p).secondToDealDamage} seconds");

            // Blocks Stats
            TabInfoManager.RegisterStat(aaStatsCategory, "Blocks When Recharge", (p) => GetStandardStatsFromPlayer(p).BlocksWhenRecharge != 0,
                (p) => $"{GetStandardStatsFromPlayer(p).BlocksWhenRecharge}");
            TabInfoManager.RegisterStat(aaStatsCategory, "Stun Block Time", (p) => GetStandardStatsFromPlayer(p).StunBlockTime != 0,
                (p) => $"{GetStandardStatsFromPlayer(p).StunBlockTime:0}s");

            // Curses Stats
            TabInfoManager.RegisterStat(aaStatsCategory, "Max Rarity For Curse", (p) => GetStandardStatsFromPlayer(p).MaxRarityForCurse != null,
                (p) => GetStandardStatsFromPlayer(p).MaxRarityForCurse.name);

            // Uncategorized Stats
            TabInfoManager.RegisterStat(aaStatsCategory, "Damage Reduction", (p) => GetStandardStatsFromPlayer(p).DamageReduction != 0,
                (p) => $"{GetStandardStatsFromPlayer(p).DamageReduction * 100:0}%");
        }

        private static StandardStats GetStandardStatsFromPlayer(Player player) {
            return player.data.GetCustomStatsRegistry().GetOrCreate<StandardStats>();
        }
    }
}
