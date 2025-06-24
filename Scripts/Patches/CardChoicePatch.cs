using AALUND13Cards.Extensions;
using AALUND13Cards.Handlers;
using CorruptedCardsManager;
using HarmonyLib;
using ModsPlus;
using System;
using UnityEngine;

namespace AALUND13Cards.Patches {
    [HarmonyPatch(typeof(CardChoice))]
    public class CardChoicePatch {
        [HarmonyPatch("IDoEndPick")]
        private static void Postfix(GameObject pickedCard, int theInt, int pickId) {
            Player player = PlayerManager.instance.GetPlayerWithID(pickId);
            if(player == null) return;

            // This code block is for the 'Extra Card Pick Handler'  
            if(ExtraCardPickHandler.currentPlayer != null && ExtraCardPickHandler.extraPicks.ContainsKey(player) && ExtraCardPickHandler.extraPicks[player].Count > 0) {
                var func = ExtraCardPickHandler.extraPicks[player][0];
                func.OnExtraPick(player, pickedCard.GetComponent<CardInfo>());
            }

            if(player.data.GetAdditionalData().DuplicatesAsCorrupted != 0) {
                CardInfo.Rarity rarity = pickedCard.GetComponent<CardInfo>().rarity;
                if(Enum.IsDefined(typeof(CorruptedCardRarity), rarity.ToString())) {
                    CorruptedCardRarity corruptedRarity = (CorruptedCardRarity)Enum.Parse(typeof(CorruptedCardRarity), rarity.ToString());
                    for(int i = 0; i < player.data.GetAdditionalData().DuplicatesAsCorrupted; i++) {
                        CorruptedCardsManager.CorruptedCardsManager.CorruptedCardsGenerators.CreateRandomCard(corruptedRarity, player);
                    }
                }
            }
        }
    }
}
