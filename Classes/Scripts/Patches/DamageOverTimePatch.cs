using AALUND13Cards.Classes.Cards;
using AALUND13Cards.Core.Extensions;
using HarmonyLib;
using Sonigon;
using UnityEngine;

namespace AALUND13Cards.Classes.Patches {
    [HarmonyPatch(typeof(DamageOverTime))]
    public class DamageOverTimePatch {
        [HarmonyPatch("TakeDamageOverTime")]
        [HarmonyPrefix]
        public static void TakeDamageOverTimePrefix(DamageOverTime __instance, Vector2 damage, Vector2 position, float time, float interval, Color color, SoundEvent soundDamageOverTime, GameObject damagingWeapon, Player damagingPlayer, bool lethal) {
            CharacterData data = (CharacterData)Traverse.Create(__instance).Field("data").GetValue();

            if(data.GetCustomStatsRegistry().GetOrCreate<ClassesStats>().Invulnerable) {
                damage = Vector2.zero;
            }
        }
    }
}
