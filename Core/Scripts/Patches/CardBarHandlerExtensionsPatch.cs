using HarmonyLib;
using System;
using System.Linq;
using System.Reflection;
using TMPro;
using UnboundLib;
using UnboundLib.Extensions;
using UnityEngine;
using UnityEngine.UI.ProceduralImage;

namespace AALUND13Cards.Core.Patches {
    public class CardBarHandlerExtensionsPatch {
        public static void Patch(Harmony harmony) {
            MethodInfo patchMethod =
                AccessTools.Method(typeof(CardBarHandlerExtensionsPatch), nameof(CardBarColor));
            MethodInfo unboundVerMethod =
                AccessTools.Method(typeof(CardBarHandlerExtensions), nameof(CardBarHandlerExtensions.Rebuild));

            harmony.Patch(unboundVerMethod, postfix: new HarmonyMethod(patchMethod));
            LoggerUtils.LogInfo("Patch 'UnboundLib' CardBarHandlerExtensions.Rebuild method");

            if(AAC_Core.Plugins.Exists(plugin => plugin.Info.Metadata.GUID == "io.olavim.rounds.rwf")) {
                Type cardBarHandlerExtensionsType = Type.GetType("RWF.CardBarHandlerExtensions, RoundsWithFriends");
                MethodInfo methodToPatch = cardBarHandlerExtensionsType.GetMethod("Rebuild");
                harmony.Patch(methodToPatch, postfix: new HarmonyMethod(patchMethod));

                LoggerUtils.LogInfo("Patch 'RWF' CardBarHandlerExtensions.Rebuild method");
            } else {
                LoggerUtils.LogInfo("No 'RWF' CardBarHandlerExtensions.Rebuild method to patch");
            }
        }

        public static void CardBarColor(CardBarHandler instance) {
            int[] colorTeamId = PlayerManager.instance.players
                .Where(p => UnboundLib.Extensions.PlayerExtensions.GetAdditionalData(p)
                    .colorID == AALUND13Team.TeamID
                )
                .Select(p => p.playerID)
                .ToArray();

            CardBar[] cardBars = (CardBar[])instance.GetFieldValue("cardBars");
            foreach(int teamId in colorTeamId) {
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
