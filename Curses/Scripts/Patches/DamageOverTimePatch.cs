using AALUND13Cards.Core.Extensions;
using AALUND13Cards.Curses.Cards;
using HarmonyLib;
using UnityEngine;

namespace AALUND13Cards.Curses.Patches {
    [HarmonyPatch(typeof(DamageOverTime))]
    public class DamageOverTimePatch {
        [HarmonyPatch("TakeDamageOverTime")]
        [HarmonyPrefix]
        public static bool TakeDamageOverTimePrefix(DamageOverTime __instance, Vector2 damage, Vector2 position, float time, float interval, Color color, GameObject damagingWeapon, Player damagingPlayer, bool lethal) {
            CharacterData data = (CharacterData)Traverse.Create(__instance).Field("data").GetValue();

            if(data.GetAdditionalData().CustomStatsRegistry.GetOrCreate<CursesStats>().DisableDecayTime) {
                data.healthHandler.DoDamage(damage, position, color, damagingWeapon, damagingPlayer, true, lethal);
                return false;
            }
            return true;
        }
    }
}
