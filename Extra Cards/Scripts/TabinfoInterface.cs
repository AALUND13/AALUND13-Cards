using AALUND13Cards.Core.Extensions;
using AALUND13Cards.Core.MonoBehaviours.CardsEffects;
using AALUND13Cards.Core.Utils;
using AALUND13Cards.ExtraCards.Cards;
using TabInfo.Utils;
using UnityEngine;

namespace AALUND13Cards.ExtraCards {
    internal class TabinfoInterface {
        public static void Setup() {
            var aaStatsCategory = AALUND13Cards.Core.TabinfoInterface.GetOrCreateCategory("AA Stats", 6);

            // Extra Cards
            TabInfoManager.RegisterStat(aaStatsCategory, "Extra Card Picks", (p) => GetExtraCaedStatsFromPlayer(p).ExtraCardPicksPerPickPhase != 0,
                (p) => $"{GetExtraCaedStatsFromPlayer(p).ExtraCardPicksPerPickPhase}");
            TabInfoManager.RegisterStat(aaStatsCategory, "Duplicates As Corrupted", (p) => GetExtraCaedStatsFromPlayer(p).DuplicatesAsCorrupted != 0,
                (p) => $"{GetExtraCaedStatsFromPlayer(p).DuplicatesAsCorrupted}");
        }

        private static ExtraCardsStats GetExtraCaedStatsFromPlayer(Player player) {
            return player.data.GetCustomStatsRegistry().GetOrCreate<ExtraCardsStats>();
        }
    }
}
