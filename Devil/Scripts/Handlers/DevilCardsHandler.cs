using AALUND13Cards.Core;
using CardChoiceSpawnUniqueCardPatch.CustomCategories;
using System.Linq;
using UnityEngine;

namespace AALUND13Cards.Devil.Handlers {
    public class DevilCardsHandler : MonoBehaviour {
        public static DevilCardsHandler Instance { get; private set; }

        public bool AllowDevilCards = false;

        void Start() {
            Instance = this;
            ModdingUtils.Utils.Cards.instance.AddCardValidationFunction((player, card) => {
                if(!AllowDevilCards && card.categories.Contains(CustomCardCategories.instance.CardCategory("DevilCard"))) return false;
                return true;
            });

            LoggerUtils.LogInfo("'DevilCardsHandler' have been setup");
        }
    }
}
