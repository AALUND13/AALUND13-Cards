using CardChoiceSpawnUniqueCardPatch.CustomCategories;
using System.Linq;

namespace AALUND13Cards.Handlers.ExtraPickHandlers {
    public class PicksPartyPickHandler : ExtraPickHandler {
        public override bool OnExtraPickStart(Player player, CardInfo card) {
            if(card.categories.Contains(CustomCardCategories.instance.CardCategory("DisallowInParty"))) {
                return false;
            }
            return true;
        }
    }
}
