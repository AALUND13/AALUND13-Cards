using AALUND13Cards.Core.Extensions;
using AALUND13Cards.Core.Handlers;
using HarmonyLib;

namespace AALUND13Cards.Core.Patches {
    [HarmonyPatch(typeof(CharacterStatModifiers))]
    public class CharacterStatModifiersPatch {
        [HarmonyPatch("ResetStats")]
        [HarmonyPrefix]
        public static void ResetStats(CharacterStatModifiers __instance) {
            CharacterData data = (CharacterData)Traverse.Create(__instance).Field("data").GetValue();
            data.GetAdditionalData().Reset();

            if(ExtraCardPickHandler.extraPicks.ContainsKey(data.player)) {
                ExtraCardPickHandler.extraPicks[data.player].Clear();
            }
        }
    }
}