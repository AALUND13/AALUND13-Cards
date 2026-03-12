using AALUND13Cards.Core.Extensions;
using AALUND13Cards.Standard.Cards;
using AALUND13Cards.Standard.Cards.StatModifers;
using AALUND13Cards.Standard.Handler;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AALUND13Cards.Standard.Patches {
    [HarmonyPatch(typeof(Gun))]
    public class GunPatch {
        [HarmonyPatch(nameof(Gun.BulletInit))]
        [HarmonyPostfix]
        public static void BulletPostfix(Gun __instance, GameObject bullet) {
            PlayerGunActions.InvokeShootAction(__instance.player, __instance, bullet);
        }

        [HarmonyPatch("ApplyProjectileStats")]
        [HarmonyPostfix]
        public static void ApplyProjectileStatsPostfix(Gun __instance, GameObject obj) {
            if(__instance.player != null && __instance.player.data.GetCustomStatsRegistry().GetOrCreate<StandardStats>().GravityDamageMultiplier != 0f) {
                MoveTransform bulletMoveTransform = obj.GetComponent<MoveTransform>();
                ProjectileHit bullet  = obj.GetComponent<ProjectileHit>();
                StandardStats stats = __instance.player.data.GetCustomStatsRegistry().GetOrCreate<StandardStats>();

                float damageMult = Mathf.Max(Mathf.Pow((bulletMoveTransform.gravity / 100) * (stats.GravityDamageMultiplier), 0.5f), 1);
                bullet.damage *= damageMult;
            }
        }
    }
}
