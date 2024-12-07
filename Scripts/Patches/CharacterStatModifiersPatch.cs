﻿using AALUND13Card.Extensions;
using AALUND13Card.MonoBehaviours;
using HarmonyLib;

namespace AALUND13Card.Patches {
    [HarmonyPatch(typeof(CharacterStatModifiers))]
    public class CharacterStatModifiersPatch {
        [HarmonyPatch("ResetStats")]
        [HarmonyPrefix]
        public static void ResetStats(CharacterStatModifiers __instance) {
            CharacterData data = (CharacterData)Traverse.Create(__instance).Field("data").GetValue();
            data.GetAdditionalData().secondToDealDamage = 0;
            data.GetAdditionalData().dealDamage = true;

            data.GetAdditionalData().SoulStreakStats = new SoulStreakStats();
            data.GetAdditionalData().Souls = 0;
        }
    }
}