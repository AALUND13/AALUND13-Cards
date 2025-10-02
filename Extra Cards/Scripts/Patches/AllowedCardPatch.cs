using AALUND13Cards.Core.Cards.Conditions;
using AALUND13Cards.Core.Extensions;
using AALUND13Cards.Core.Handlers;
using AALUND13Cards.ExtraCards.Handlers;
using CardChoiceSpawnUniqueCardPatch.CustomCategories;
using HarmonyLib;
using RarityLib.Utils;
using System.Linq;

namespace AALUND13Cards.ExtraCards.Patches {
    [HarmonyPatch(typeof(ModdingUtils.Utils.Cards), "PlayerIsAllowedCard")]
    public class AllowedCardPatch {
        private static void Postfix(Player player, CardInfo card, ref bool __result) {
            if (player == null || card == null) return;

            // This code block is for the 'Extra Card Pick Handler'
            if(ExtraCardPickHandler.currentPlayer != null && ExtraCardPickHandler.extraPicks.ContainsKey(player) && ExtraCardPickHandler.extraPicks[player].Count > 0) {
                var func = ExtraCardPickHandler.activePickHandler;
                
                bool allowed = func.OnExtraPickStart(player, card);
                __result = __result && allowed;
            }
        }
    }
}
