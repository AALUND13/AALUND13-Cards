using AALUND13Cards.Core;
using AALUND13Cards.Core.Extensions;
using AALUND13Cards.Devil.Cards;
using PickPhaseImprovements;
using RarityLib.Utils;
using System.Collections.Generic;
using System.Linq;
using UnboundLib;
using UnityEngine;

namespace AALUND13Cards.Devil.Handlers {
    public static class GuaranteedCardOfRarityHandler {
        private readonly static Dictionary<int, Rarity> GuaranteedCardSlots = new Dictionary<int, Rarity>();
        internal static bool GeneratedGuaranteedCardOfRaritesSlots = false;

        internal static void GetGuaranteedCardOfRaritesSlots(Player player) {
            GuaranteedCardSlots.Clear();

            var RarityList = new Queue<Rarity>(player.data.GetCustomStatsRegistry().GetOrCreate<DevilCardsStats>().GuaranteedRarities);
            int numOfDraw = DrawNCards.DrawNCards.GetPickerDraws(player.playerID);
            
            List<int> alreadyTakenSlots = new List<int>(RarityList.Count);
            for (int i = 0; i < RarityList.Count; i++) {
                Rarity rarity = RarityList.Dequeue();
                
                int takenSlot = Random.Range(0, numOfDraw);
                while(alreadyTakenSlots.Any(s => s == takenSlot)) takenSlot = Random.Range(0, numOfDraw);
                
                alreadyTakenSlots.Add(takenSlot);
                GuaranteedCardSlots.Add(takenSlot, rarity);
                    
                LoggerUtils.Log(BepInEx.Logging.LogLevel.Info, $"Guaranteed card of rarity {rarity.name} in slot {takenSlot}");
            }
        }

        public static PickManager.ValidationResult GuaranteedCardOfRarites(CardInfo[] currentCards, CardInfo thisCard) {
            if(!GeneratedGuaranteedCardOfRaritesSlots) {
                Player player = (((PickerType)CardChoice.instance.GetFieldValue("pickerType") != 0)
                    ? PlayerManager.instance.players[CardChoice.instance.pickrID]
                    : PlayerManager.instance.GetPlayersInTeam(CardChoice.instance.pickrID)[0]);

                GetGuaranteedCardOfRaritesSlots(player);
                GeneratedGuaranteedCardOfRaritesSlots = true;
            }

            int slot = currentCards.Length;

            LoggerUtils.Log(BepInEx.Logging.LogLevel.Info, $"Validating card {thisCard.cardName} in slot {slot}");
            LoggerUtils.Log(BepInEx.Logging.LogLevel.Debug, $"Current Cards Amount: {currentCards.Length}");

            if (GuaranteedCardSlots.TryGetValue(slot, out var rarity)) {
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
