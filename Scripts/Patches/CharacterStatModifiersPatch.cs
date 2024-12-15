using AALUND13Card.Extensions;
using AALUND13Card.MonoBehaviours;
using HarmonyLib;

namespace AALUND13Card.Patches {
    [HarmonyPatch(typeof(CharacterStatModifiers))]
    public class CharacterStatModifiersPatch {
        [HarmonyPatch("ResetStats")]
        [HarmonyPrefix]
        public static void ResetStats(CharacterStatModifiers __instance) {
            CharacterData data = (CharacterData)Traverse.Create(__instance).Field("data").GetValue();
            data.GetAdditionalData().Reset();
        }
    }
}