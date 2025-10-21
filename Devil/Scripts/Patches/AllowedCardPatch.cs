using AALUND13Cards.Core.Extensions;
using AALUND13Cards.Devil.Cards;
using HarmonyLib;
using RarityLib.Utils;
using System.Collections.Generic;
using UnboundLib;
using UnityEngine;

namespace AALUND13Cards.Devil.Patches {
    [HarmonyPatch(typeof(ModdingUtils.Utils.Cards), "PlayerIsAllowedCard")]
    internal class AllowedCardPatch {
        private static void Postfix(Player player, CardInfo card, ref bool __result) {
            if(player == null || card == null) return;

            var spawnedCards = (List<GameObject>)CardChoice.instance.GetFieldValue("spawnedCards");
            if(player.data.GetCustomStatsRegistry().GetOrCreate<DevilCardsStats>().GuaranteedRarity != null
                && CardChoicePatch.GuaranteedCardSlot == spawnedCards.Count
                && IsBelowRarity(player.data.GetCustomStatsRegistry().GetOrCreate<DevilCardsStats>().GuaranteedRarity.value, card.rarity)
            ) {
                __result = false;
            }
        }

        private static bool IsBelowRarity(CardInfo.Rarity rarity, CardInfo.Rarity otherRarity) {
            Rarity cardRarity = RarityUtils.GetRarityData(rarity);
            Rarity otherCardRarity = RarityUtils.GetRarityData(otherRarity);
            return cardRarity.relativeRarity < otherCardRarity.relativeRarity;
        }
    }
}
