using AALUND13Cards.Handlers;
using HarmonyLib;
using UnityEngine;

namespace AALUND13Cards.Patches {
    [HarmonyPatch(typeof(DamageOverTime))]
    public class DamageOverTimePatch {
        [HarmonyPatch("TakeDamageOverTime")]
        [HarmonyPrefix]
        public static void TakeDamageOverTimePrefix(DamageOverTime __instance, Vector2 damage) {
            CharacterData data = (CharacterData)Traverse.Create(__instance).Field("data").GetValue();

            // If TakeDamageRunning is true, it means we are already in the process of taking damage
            if(!HealthHandlerPatch.TakeDamageRunning) {
                DamageEventHandler.TriggerDamageEvent(DamageEventHandler.DamageEventType.OnTakeDamageOvertime, data.player, damage);
            }
        }
    }
}
