using AALUND13Cards.Core;
using CardChoiceSpawnUniqueCardPatch.CustomCategories;
using RarityLib.Utils;
using System.Collections;
using System.Linq;
using UnboundLib.GameModes;
using UnityEngine;

namespace AALUND13Cards.Devil.Handlers {
    public class DevilCardsHandler : MonoBehaviour {
        public static DevilCardsHandler Instance { get; private set; }

        public bool AllowDevilCards = false;

        void Start() {
            Instance = this;

            GameModeManager.AddHook(GameModeHooks.HookGameStart, OnGameStarted);
            ModdingUtils.Utils.Cards.instance.AddCardValidationFunction((player, card) => {
                if(AllowDevilCards && card.rarity != RarityUtils.GetRarity("Devil")) return false;
                return true;
            });
        }

        private IEnumerator OnGameStarted(IGameModeHandler gameModeHandler) {
            AllowDevilCards = false;
            yield break;
        }
    }
}
