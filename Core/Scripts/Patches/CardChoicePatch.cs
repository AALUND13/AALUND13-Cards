using AALUND13Cards.Core.Handlers;
using AALUND13Cards.Core.Utils;
using HarmonyLib;
using ModsPlus;
using UnityEngine;

namespace AALUND13Cards.Core.Patches {
    [HarmonyPatch(typeof(CardChoice))]
    internal class CardChoicePatch {
        [HarmonyPatch("IDoEndPick")]
        private static void Postfix(GameObject pickedCard, int pickId) {
            Player player = PlayerManager.instance.GetPlayerWithID(pickId);
            if(player == null) return;

            PickCardTracker.instance.AddCardPickedInPickPhase(pickedCard.GetComponent<CardInfo>());
        }
    }
}
