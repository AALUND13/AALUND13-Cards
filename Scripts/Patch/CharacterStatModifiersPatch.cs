using AALUND13Card.Extensions;
using HarmonyLib;
using System.Collections.Generic;

namespace AALUND13Card.Patchs {
    [HarmonyPatch(typeof(CharacterStatModifiers))]
    public class CharacterStatModifiersPatch {
        [HarmonyPatch("ResetStats")]
        [HarmonyPrefix]
        public static void ResetStats(CharacterStatModifiers __instance) {
            CharacterData data = (CharacterData)Traverse.Create(__instance).Field("data").GetValue();
            data.GetAdditionalData().secondToDealDamage = 0;
            data.GetAdditionalData().DamageDealSecond = new List<DamageDealSecond>();
            data.GetAdditionalData().dealDamage = true;
        }
    }
}