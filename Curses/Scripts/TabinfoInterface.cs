using AALUND13Cards.Core.Extensions;
using AALUND13Cards.Core.MonoBehaviours.CardsEffects;
using AALUND13Cards.Core.Utils;
using AALUND13Cards.Curses.Cards;
using TabInfo.Utils;
using UnityEngine;

namespace AALUND13Cards.Curses {
    internal class TabinfoInterface {
        public static void Setup() {
            var aaStatsCategory = AALUND13Cards.Core.TabinfoInterface.GetOrCreateCategory("AA Stats", 6);

            TabInfoManager.RegisterStat(aaStatsCategory, "Is Bind", (p) => GetCursesStatsFromPlayer(p).IsBind,
                (p) => GetCursesStatsFromPlayer(p).IsBind ? "Yes" : "No");
            TabInfoManager.RegisterStat(aaStatsCategory, "Is Decay Time Disable", (p) => GetCursesStatsFromPlayer(p).DisableDecayTime,
                (p) => GetCursesStatsFromPlayer(p).DisableDecayTime ? "Yes" : "No");
        }

        private static CursesStats GetCursesStatsFromPlayer(Player player) {
            return player.data.GetCustomStatsRegistry().GetOrCreate<CursesStats>();
        }
    }
}
