using AALUND13Cards.Core.Extensions;
using AALUND13Cards.ExtraCards.Handlers;
using HarmonyLib;

namespace AALUND13Cards.ExtraCards.Patches {
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