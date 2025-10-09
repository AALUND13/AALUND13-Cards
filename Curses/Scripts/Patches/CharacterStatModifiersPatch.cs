using AALUND13Cards.Core.Extensions;
using AALUND13Cards.Curses.MonoBehaviours;
using HarmonyLib;
using UnityEngine;

namespace AALUND13Cards.Curses.Patches {
    [HarmonyPatch(typeof(CharacterStatModifiers))]
    public class CharacterStatModifiersPatch {
        [HarmonyPatch("ResetStats")]
        [HarmonyPrefix]
        public static void ResetStats(CharacterStatModifiers __instance) {
            CharacterData data = (CharacterData)Traverse.Create(__instance).Field("data").GetValue();
            data.GetAdditionalData().Reset();

            if(data.view.IsMine) {
                Camera mainCamera = Camera.main;
                if(mainCamera != null) {
                    foreach(var effect in mainCamera.GetComponents<PostProcessingEffect>()) {
                        MonoBehaviour.Destroy(effect);
                    }
                }
            }
        }
    }
}