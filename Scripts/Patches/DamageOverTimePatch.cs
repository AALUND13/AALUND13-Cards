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

            DamageEventHandler.TriggerDamageEvent(DamageEventHandler.DamageEventType.OnTakeDamageOvertime, data.player, damage);
        }
    }
}
