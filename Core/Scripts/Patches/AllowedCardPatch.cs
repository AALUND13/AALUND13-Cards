using AALUND13Cards.Core.Cards.Conditions;
using AALUND13Cards.Core.Extensions;
using AALUND13Cards.Core.Handlers;
using CardChoiceSpawnUniqueCardPatch.CustomCategories;
using HarmonyLib;
using RarityLib.Utils;
using System.Linq;

namespace AALUND13Cards.Core.Patches {
    [HarmonyPatch(typeof(ModdingUtils.Utils.Cards), "PlayerIsAllowedCard")]
    internal class AllowedCardPatch {
        private static void Postfix(Player player, CardInfo card, ref bool __result) {
            if (player == null || card == null) return;

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
        }
    }
}
