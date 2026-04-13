using AALUND13Cards.Core.Utils;

namespace AALUND13Cards.ExtraCards.Cards {
    public class ExtraCardsStats : ICustomStats {
        public int DuplicatesAsCorrupted = 0;
        public int ExtraCardPicksPerPickPhase = 0;
        
        // Cursed Draws
        public int CurseCardDraws = 0;
        public bool FullCurseDraws = false;
        
        public void ResetStats() {
            DuplicatesAsCorrupted = 0;
            ExtraCardPicksPerPickPhase = 0;

            // Cursed Draws
            CurseCardDraws = 0;
            FullCurseDraws = false;
        }
    }
}
