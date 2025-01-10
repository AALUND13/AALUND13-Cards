using AALUND13Card.Handlers;
using HarmonyLib;

namespace AALUND13Card.Patches {
    [HarmonyPatch(typeof(ModdingUtils.Utils.Cards), "PlayerIsAllowedCard")]
    public class AllowedCardPatch {
        private static void Postfix(Player player, CardInfo card, ref bool __result, ModdingUtils.Utils.Cards __instance) {
            if (player == null || card == null) return;

            // This code block is for the 'Extra Card Pick Handler'
            if(ExtraCardPickHandler.currentPlayer != null && ExtraCardPickHandler.extraPicks.ContainsKey(player) && ExtraCardPickHandler.extraPicks[player].Count > 0) {
                var func = ExtraCardPickHandler.extraPicks[player][0];
                
                bool allowed = func.OnExtraPickStart(player, card);
                __result = __result && allowed;
            }
        }
    }
}
