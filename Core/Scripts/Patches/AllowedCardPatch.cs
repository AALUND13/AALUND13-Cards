using AALUND13Cards.Core.Cards.Conditions;
using AALUND13Cards.Core.Handlers;
using HarmonyLib;

namespace AALUND13Cards.Core.Patches {
    [HarmonyPatch(typeof(ModdingUtils.Utils.Cards), "PlayerIsAllowedCard")]
    internal class AllowedCardPatch {
        private static void Postfix(Player player, CardInfo card, ref bool __result) {
            if(player == null || card == null) return;

            CardCondition[] conditions = card.GetComponents<CardCondition>();
            if(conditions != null && conditions.Length > 0) {
                foreach(CardCondition condition in conditions) {
                    if(condition == null) continue;
                    if(!condition.IsPlayerAllowedCard(player)) {
                        __result = false;
                        break;
                    }
                }
            }

            if(ExtraCardPickHandler.currentPlayer != null && ExtraCardPickHandler.extraPicks.ContainsKey(player) && ExtraCardPickHandler.extraPicks[player].Count > 0) {
                bool allowed = ExtraCardPickHandler.activePickHandler.PickConditions(player, card);
                __result = __result && allowed;
            }
        }
    }
}
