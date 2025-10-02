using AALUND13Cards.Armors.Cards;
using AALUND13Cards.Core.Extensions;
using TabInfo.Utils;

namespace AALUND13Cards.Armors {
    internal class TabinfoInterface {
        public static void Setup() {
            var aaStatsCategory = AALUND13Cards.Core.TabinfoInterface.GetOrCreateCategory("AA Stats", 6);

            // Armor Stats
            TabInfoManager.RegisterStat(aaStatsCategory, "Damage Against Armor Percentage", (p) => GetArmorStatsFromPlayer(p).DamageAgainstArmorPercentage != 1f,
                (p) => $"{GetArmorStatsFromPlayer(p).DamageAgainstArmorPercentage * 100:0}%");
            TabInfoManager.RegisterStat(aaStatsCategory, "Armor Damage Reduction", (p) => GetArmorStatsFromPlayer(p).ArmorDamageReduction != 0f,
                (p) => $"{GetArmorStatsFromPlayer(p).ArmorDamageReduction * 100:0}%");
        }

        private static ArmorStats GetArmorStatsFromPlayer(Player player) {
            return player.data.GetCustomStatsRegistry().GetOrCreate<ArmorStats>();
        }
    }
}
