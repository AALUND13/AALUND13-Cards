using AALUND13Card.Extensions;
using AALUND13Card.MonoBehaviours;
using HarmonyLib;
using UnityEngine;

namespace AALUND13Card.Patches {
    [HarmonyPatch(typeof(CharacterStatModifiers))]
    public class CharacterStatModifiersPatch {
        [HarmonyPatch("ResetStats")]
        [HarmonyPrefix]
        public static void ResetStats(CharacterStatModifiers __instance) {
            CharacterData data = (CharacterData)Traverse.Create(__instance).Field("data").GetValue();
            data.GetAdditionalData().Reset();

            Camera mainCamera = Camera.main;
            if(mainCamera != null) {
                foreach(var effect in mainCamera.GetComponents<Effect>()) {
                    MonoBehaviour.Destroy(effect);
                }
            }
        }
    }
}