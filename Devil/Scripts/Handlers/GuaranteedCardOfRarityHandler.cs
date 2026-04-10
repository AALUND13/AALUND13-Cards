using AALUND13Cards.Core;
using AALUND13Cards.Core.Extensions;
using AALUND13Cards.Devil.Cards;
using AALUND13Cards.Devil.Patches;
using NUnit.Framework.Interfaces;
using PickPhaseImprovements;
using RarityLib.Utils;
using System.Collections.Generic;
using System.Linq;
using UnboundLib;
using UnityEngine;

namespace AALUND13Cards.Devil.Handlers {
    public static class GuaranteedCardOfRarityHandler {
        private static Dictionary<int, Rarity> GuaranteedCardSlots = new Dictionary<int, Rarity>();
         
        internal static void GetGuaranteedCardOfRaritesSlots(Player player) {
            GuaranteedCardSlots.Clear();

            // Sort the rarities base off the chnaces
            var sortedRarityList = player.data
                .GetCustomStatsRegistry()
                .GetOrCreate<DevilCardsStats>()
                .GuaranteedRarities
                .OrderByDescending(r => r.calculatedRarity)
                .ToList();

            // Create a list with all the slots that can be taken
            int numOfDraw = DrawNCards.DrawNCards.GetPickerDraws(player.playerID);
            var slotsToTake = new List<int>(numOfDraw);
            for(int i = 1; i <= numOfDraw; i++) {
                slotsToTake.Add(i);
            }

            // Create the list with all the "Guaranteed" rarities slots
            while(sortedRarityList.Count > 0 && slotsToTake.Count > 0) {
                Rarity rarity = sortedRarityList[0];
                sortedRarityList.RemoveAt(0);

                int index = Random.Range(0, slotsToTake.Count);
                int randomSlot = slotsToTake[index];
                slotsToTake.RemoveAt(index);

                GuaranteedCardSlots[randomSlot] = rarity;
            }
        }

        public static PickManager.ValidationResult GuaranteedCardOfRarites(CardInfo[] currentCards, CardInfo thisCard) {
            Player player = (((PickerType)CardChoice.instance.GetFieldValue("pickerType") != 0)
                ? PlayerManager.instance.players[CardChoice.instance.pickrID]
                : PlayerManager.instance.GetPlayersInTeam(CardChoice.instance.pickrID)[0]);

            int slot = currentCards.Length + 1;

            if(GuaranteedCardSlots.TryGetValue(slot, out var rarity)) {
                if(IsBelowRarity(rarity.value, thisCard.rarity)) {
                    return PickManager.ValidationResult.Invalid;
                }
            }

            return PickManager.ValidationResult.Valid;
        }

        private static bool IsBelowRarity(CardInfo.Rarity rarity, CardInfo.Rarity otherRarity) {
            Rarity cardRarity = RarityUtils.GetRarityData(rarity);
            Rarity otherCardRarity = RarityUtils.GetRarityData(otherRarity);
            return cardRarity.relativeRarity < otherCardRarity.relativeRarity;
        }
    }
}
