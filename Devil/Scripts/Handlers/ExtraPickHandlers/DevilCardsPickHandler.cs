using AALUND13Cards.Core.Handlers;
using CardChoiceSpawnUniqueCardPatch.CustomCategories;
using System.Linq;

namespace AALUND13Cards.Devil.Handlers.ExtraPickHandlers {
    public class DevilCardsPickHandler : ExtraPickHandler {
        private int oldNumberOfDraws;

        public override bool PickConditions(Player player, CardInfo card) {
            return card.categories.Contains(CustomCardCategories.instance.CardCategory("DevilCard"));
        }

        public override void OnPickStart(Player player) {
            oldNumberOfDraws = DrawNCards.DrawNCards.GetPickerDraws(player.playerID);
            DrawNCards.DrawNCards.RPCA_SetPickerDraws(player.playerID, 3);
            DevilCardsHandler.Instance.AllowDevilCards = true;
        }

        public override void OnPickEnd(Player player, CardInfo card) {
            DrawNCards.DrawNCards.RPCA_SetPickerDraws(player.playerID, oldNumberOfDraws);
            DevilCardsHandler.Instance.AllowDevilCards = false;
        }
    }
}
