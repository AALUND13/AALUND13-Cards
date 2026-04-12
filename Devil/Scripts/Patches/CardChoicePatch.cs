using AALUND13Cards.Core;
using AALUND13Cards.Core.Extensions;
using AALUND13Cards.Devil.Cards;
using AALUND13Cards.Devil.Handlers;
using HarmonyLib;
using RarityLib.Utils;
using System.Collections.Generic;
using System.Linq;
using UnboundLib;
using UnityEngine;

namespace AALUND13Cards.Devil.Patches {
    [HarmonyPatch(typeof(CardChoice), "ReplaceCards")]
    public class CardChoicePatch {
        private static void Prefix(CardChoice __instance) {
            Player player = (((PickerType)CardChoice.instance.GetFieldValue("pickerType") != 0)
                ? PlayerManager.instance.players[CardChoice.instance.pickrID]
                : PlayerManager.instance.GetPlayersInTeam(CardChoice.instance.pickrID)[0]);

            GuaranteedCardOfRarityHandler.GetGuaranteedCardOfRaritesSlots(player);
        }
    }
}