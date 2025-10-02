using AALUND13Cards.Core.Extensions;
using AALUND13Cards.Standard.Cards;
using CardChoiceSpawnUniqueCardPatch.CustomCategories;
using HarmonyLib;
using RarityLib.Utils;
using System.Linq;

namespace AALUND13Cards.Core.Patches {
    [HarmonyPatch(typeof(ModdingUtils.Utils.Cards), "PlayerIsAllowedCard")]
    internal class AllowedCardPatch {
        private static void Postfix(Player player, CardInfo card, ref bool __result) {
            if(player == null || card == null) return;

            if(card.categories.Contains(CustomCardCategories.instance.CardCategory("Curse")) && player.data.GetCustomStatsRegistry().GetOrCreate<StandardStats>().MaxRarityForCurse != null) {
                Rarity cardRarity = RarityUtils.GetRarityData(card.rarity);
                Rarity maxRarity = player.data.GetCustomStatsRegistry().GetOrCreate<StandardStats>().MaxRarityForCurse;

                if(cardRarity.relativeRarity < maxRarity.relativeRarity) {
                    __result = false;
                }
            }
        }
    }
}
