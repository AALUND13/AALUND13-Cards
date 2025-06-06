﻿using AALUND13Cards.Extensions;
using AALUND13Cards.Handlers;
using HarmonyLib;
using ModsPlus;
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
        }
    }
}
