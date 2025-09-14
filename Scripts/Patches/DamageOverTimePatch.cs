using AALUND13Cards.Extensions;
using AALUND13Cards.Handlers;
using HarmonyLib;
using Sonigon;
using UnityEngine;

namespace AALUND13Cards.Patches {
    [HarmonyPatch(typeof(DamageOverTime))]
    public class DamageOverTimePatch {
        [HarmonyPatch("TakeDamageOverTime")]
        [HarmonyPrefix]
        public static bool TakeDamageOverTimePrefix(DamageOverTime __instance, Vector2 damage, Vector2 position, float time, float interval, Color color, SoundEvent soundDamageOverTime, GameObject damagingWeapon, Player damagingPlayer, bool lethal) {
            CharacterData data = (CharacterData)Traverse.Create(__instance).Field("data").GetValue();

            // If TakeDamageRunning is true, it means we are already in the process of taking damage
            if(!HealthHandlerPatch.TakeDamageRunning) {
                DamageEventHandler.TriggerDamageEvent(DamageEventHandler.DamageEventType.OnTakeDamageOvertime, data.player, damagingPlayer, damage, lethal);
            }

            if(data.GetAdditionalData().DisableDecayTime) {
                data.healthHandler.DoDamage(damage, position, color, damagingWeapon, damagingPlayer, true, lethal);
                return false;
            }
            return true;
        }
    }
}
