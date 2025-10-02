using AALUND13Cards.Core.Extensions;
using AALUND13Cards.Core.Handlers;
using AALUND13Cards.Core.Utils;
using AALUND13Cards.ExtraCards;
using AALUND13Cards.ExtraCards.Cards;
using AALUND13Cards.ExtraCards.Handlers;
using CorruptedCardsManager;
using HarmonyLib;
using ModsPlus;
using Photon.Pun;
using RandomCardsGenerators.Cards;
using System;
using UnityEngine;

namespace AALUND13Cards.Core.Patches {
    [HarmonyPatch(typeof(CardChoice))]
    public class CardChoicePatch {
        [HarmonyPatch("IDoEndPick")]
        private static void Postfix(GameObject pickedCard, int theInt, int pickId) {
            var player = PlayerManager.instance.GetPlayerWithID(pickId);
            if(player == null) return;

            // This code block is for the 'Extra Card Pick Handler'  
            if(ExtraCardPickHandler.currentPlayer != null && ExtraCardPickHandler.extraPicks.ContainsKey(player) && ExtraCardPickHandler.extraPicks[player].Count > 0) {
                var func = ExtraCardPickHandler.activePickHandler;
                func.OnExtraPick(player, pickedCard.GetComponent<CardInfo>());
            }

            var extraCardsStats = player.data.GetCustomStatsRegistry().GetOrCreate<ExtraCardsStats>();

            if(extraCardsStats.DuplicatesAsCorrupted != 0 && (pickedCard.GetComponent<RandomCard>() == null || !pickedCard.GetComponent<RandomCard>().StatGenName.StartsWith("CCM_CorruptedCardsGenerator")) && (PhotonNetwork.IsMasterClient || PhotonNetwork.OfflineMode)) {
                CardInfo.Rarity rarity = pickedCard.GetComponent<CardInfo>().rarity;
                if(Enum.IsDefined(typeof(CorruptedCardRarity), rarity.ToString())) {
                    CorruptedCardRarity corruptedRarity = (CorruptedCardRarity)Enum.Parse(typeof(CorruptedCardRarity), rarity.ToString());
                    for(int i = 0; i < extraCardsStats.DuplicatesAsCorrupted; i++) {
                        CorruptedCardsManager.CorruptedCardsManager.CorruptedCardsGenerators.CreateRandomCard(corruptedRarity, player);
                    }
                }
            }
        }
    }
}
