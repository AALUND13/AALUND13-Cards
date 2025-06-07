using AALUND13Cards.Extensions;
using AALUND13Cards.Handlers;
using AALUND13Cards.MonoBehaviours;
using HarmonyLib;
using UnityEngine;

namespace AALUND13Cards.Patches {
    [HarmonyPatch(typeof(CharacterStatModifiers))]
    public class CharacterStatModifiersPatch {
        [HarmonyPatch("ResetStats")]
        [HarmonyPrefix]
        public static void ResetStats(CharacterStatModifiers __instance) {
            CharacterData data = (CharacterData)Traverse.Create(__instance).Field("data").GetValue();
            data.GetAdditionalData().Reset();

            if (ExtraCardPickHandler.extraPicks.ContainsKey(data.player)) {
                ExtraCardPickHandler.extraPicks[data.player].Clear();
            }

            Camera mainCamera = Camera.main;
            if(mainCamera != null) {
                foreach(var effect in mainCamera.GetComponents<Effect>()) {
                    MonoBehaviour.Destroy(effect);
                }
            }
        }
    }
}