using AALUND13Cards.Cards.Conditions;
using AALUND13Cards.Extensions;
using AALUND13Cards.Handlers;
using CardChoiceSpawnUniqueCardPatch.CustomCategories;
using HarmonyLib;
using RarityLib.Utils;
using System.Linq;

namespace AALUND13Cards.Patches {
    [HarmonyPatch(typeof(ModdingUtils.Utils.Cards), "PlayerIsAllowedCard")]
    public class AllowedCardPatch {
        private static void Postfix(Player player, CardInfo card, ref bool __result) {
            if (player == null || card == null) return;

            // This code block is for the 'Extra Card Pick Handler'
            if(ExtraCardPickHandler.currentPlayer != null && ExtraCardPickHandler.extraPicks.ContainsKey(player) && ExtraCardPickHandler.extraPicks[player].Count > 0) {
                var func = ExtraCardPickHandler.extraPicks[player][0];
                
                bool allowed = func.OnExtraPickStart(player, card);
                __result = __result && allowed;
            }

            // For the `CardCondition` components on the card
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

            // For the `MaxRarityForCurse` condition
            if(card.categories.Contains(CustomCardCategories.instance.CardCategory("Curse")) && player.data.GetAdditionalData().MaxRarityForCurse != null) {
                Rarity cardRarity = RarityUtils.GetRarityData(card.rarity);
                Rarity maxRarity = player.data.GetAdditionalData().MaxRarityForCurse;

                if(cardRarity.relativeRarity < maxRarity.relativeRarity) {
                    __result = false;
                }
            }
        }
    }
}
