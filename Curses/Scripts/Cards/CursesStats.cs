using AALUND13Cards.Core.Utils;

namespace AALUND13Cards.Curses.Cards {
    public class CursesStats : ICustomStats {
        public bool IsBind = false;
        public bool DisableDecayTime = false;

        public void ResetStats() {
            DisableDecayTime = false;
            IsBind = false;
        }
    }
}
