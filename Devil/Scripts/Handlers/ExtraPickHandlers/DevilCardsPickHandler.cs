using AALUND13Cards.Core.Handlers;
using CardChoiceSpawnUniqueCardPatch.CustomCategories;
using System.Linq;

namespace AALUND13Cards.Devil.Handlers.ExtraPickHandlers {
    public class DevilCardsPickHandler : ExtraPickHandler {
        public override void OnPickStart(Player player) {
            DevilCardsHandler.Instance.AllowDevilCards = true;
        }

        public override void OnPickEnd(Player player, CardInfo card) {
            DevilCardsHandler.Instance.AllowDevilCards = false;
        }

        public override int HandSize => 3;
    }
}
