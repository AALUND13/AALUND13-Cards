using AALUND13Cards.Core.Extensions;
using AALUND13Cards.Devil.Cards;
using HarmonyLib;
using RarityLib.Utils;
using System.Collections.Generic;
using UnboundLib;
using UnityEngine;

namespace AALUND13Cards.Devil.Patches {
    [HarmonyPatch(typeof(CardChoice))]
    public class CardChoicePatch {
        internal static int GuaranteedCardSlot = 0;

        [HarmonyPrefix]
        [HarmonyPatch(nameof(CardChoice.StartPick))]
        private static void GuaranteedCardOfRarity(CardChoice __instance, int pickerIDToSet) {
            int numOfDraw = DrawNCards.DrawNCards.GetPickerDraws(pickerIDToSet);
            GuaranteedCardSlot = UnityEngine.Random.Range(0, numOfDraw);
        }
    }
}
