using HarmonyLib;
using InControl;
using TMPro;
using UnboundLib;
using UnityEngine;
using UnityEngine.UI.ProceduralImage;

namespace AALUND13Cards.Core.Patches {
    [HarmonyPatch(typeof(CardBarHandler), nameof(CardBarHandler.AddCard))]
    public class CardBarHandlerPatch {
        public static void Postfix(CardBarHandler __instance, int teamId) {
            int colorId = UnboundLib.Extensions.PlayerExtensions.GetAdditionalData(PlayerManager.instance.players.Find(p => p.teamID == teamId)).colorID;
            if(colorId == AALUND13Team.TeamID) {
                CardBar[] cardBars = (CardBar[])__instance.GetFieldValue("cardBars");
                CardBar cardBar = cardBars[teamId];

                GameObject lastCardButton = cardBar.transform.GetChild(cardBar.transform.childCount - 1).gameObject;
                foreach(var images in lastCardButton.GetComponentsInChildren<ProceduralImage>()) {
                    images.color = Color.black;
                }

                TextMeshProUGUI text = lastCardButton.GetComponentInChildren<TextMeshProUGUI>();
                text.color = Color.white;
            }
        }
    }
}
