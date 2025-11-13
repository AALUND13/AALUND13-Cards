using AALUND13Cards.Classes.Cards;
using AALUND13Cards.Core.Extensions;
using AALUND13Cards.Core.Handlers;
using HarmonyLib;
using UnityEngine;

namespace AALUND13Cards.Classes.Patches {
    [HarmonyPatch(typeof(HealthHandler))]
    public class HealthHandlerPatch {
        [HarmonyPatch(nameof(HealthHandler.TakeDamage), typeof(Vector2), typeof(Vector2), typeof(Color), typeof(GameObject), typeof(Player), typeof(bool), typeof(bool))]
        [HarmonyPrefix]
        public static void TakeDamagePrefix(HealthHandler __instance, ref Vector2 damage) {
            CharacterData data = (CharacterData)Traverse.Create(__instance).Field("data").GetValue();

            if(data.GetCustomStatsRegistry().GetOrCreate<ClassesStats>().Invulnerable) {
                damage = Vector2.zero;
            }
        }

        [HarmonyPatch(nameof(HealthHandler.DoDamage))]
        [HarmonyPrefix]
        public static void DoDamage(HealthHandler __instance, ref Vector2 damage) {
            CharacterData data = (CharacterData)Traverse.Create(__instance).Field("data").GetValue();

            if(data.GetCustomStatsRegistry().GetOrCreate<ClassesStats>().Invulnerable) {
                damage = Vector2.zero;
            }
        }
    }
}
