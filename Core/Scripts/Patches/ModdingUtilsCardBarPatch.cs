using HarmonyLib;
using ModdingUtils.Utils;
using System;
using TMPro;
using UnboundLib.Extensions;
using UnityEngine;

namespace AALUND13Cards.Core.Patches {
    [HarmonyPatch(typeof(CardBarUtils), nameof(CardBarUtils.ResetPlayersLineColor), new Type[] { typeof(int) })]
    public class ModdingUtilsCardBarPatch {
        private static void Postfix(int playerID) {
            Player player = PlayerManager.instance.players.Find(p => p.playerID == playerID);
            if (player.GetAdditionalData().colorID == AALUND13Team.TeamID) {
                CardBar cardBar = CardBarUtils.instance.PlayersCardBar(playerID);
                foreach (var text in cardBar.gameObject.GetComponentsInChildren<TextMeshProUGUI>()) {
                    text.color = Color.white;
                }
            }
        }
    }
}
