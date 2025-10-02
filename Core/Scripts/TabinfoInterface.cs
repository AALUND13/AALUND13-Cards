using AALUND13Cards.Core.Extensions;
using AALUND13Cards.Core.MonoBehaviours.CardsEffects;
using AALUND13Cards.Core.Utils;
using TabInfo.Utils;
using UnityEngine;

namespace AALUND13Cards.Core {
    public class TabinfoInterface {
        public static StatCategory GetOrCreateCategory(string name, int priority) {
            if(!TabInfoManager.Categories.ContainsKey(name.ToLower())) return TabInfoManager.RegisterCategory(name, 6);
            return TabInfoManager.Categories[name.ToLower()];
        }

        internal static void Setup() {
            var aaStatsCategory = GetOrCreateCategory("AA Stats", 6);

            // Blocks
            TabInfoManager.RegisterStat(aaStatsCategory, "Block Pierce Percent", (p) => p.data.GetAdditionalData().BlockPircePercent != 0,
                (p) => $"{p.data.GetAdditionalData().BlockPircePercent * 100:0}%");

            // Uncategorized Stats
            TabInfoManager.RegisterStat(aaStatsCategory, "Damage Per Seconds", (_) => true,
                (p) => $"{p.GetDPS()}");
            TabInfoManager.RegisterStat(aaStatsCategory, "Bullets Per Seconds", (_) => true,
                (p) => $"{p.GetSPS()}");
        }
    }
}
