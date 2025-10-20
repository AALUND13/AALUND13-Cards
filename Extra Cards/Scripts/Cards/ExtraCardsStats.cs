using AALUND13Cards.Core.Utils;

namespace AALUND13Cards.ExtraCards.Cards {
    public class ExtraCardsStats : ICustomStats {
        public int DuplicatesAsCorrupted = 0;
        public int ExtraCardPicksPerPickPhase = 0;

        public void ResetStats() {
            DuplicatesAsCorrupted = 0;
            ExtraCardPicksPerPickPhase = 0;
        }
    }
}
