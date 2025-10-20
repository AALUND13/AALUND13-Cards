using AALUND13Cards.Classes.Cards;
using AALUND13Cards.Classes.Utils;
using AALUND13Cards.Core.Extensions;
using AALUND13Cards.Core.MonoBehaviours.CardsEffects;
using AALUND13Cards.Core.Utils;
using HarmonyLib;
using System.Collections.Generic;
using System.Reflection.Emit;
using UnityEngine;

namespace AALUND13Cards.Core.Patches {
    [HarmonyPatch(typeof(Gun))]
    public class GunPatch {
        [HarmonyPatch("ApplyProjectileStats")]
        [HarmonyPrefix]
        public static void ApplyRailgunStats(Gun __instance, GameObject obj) {
            RailgunStats railgunStats = __instance.player.data.GetCustomStatsRegistry().GetOrCreate<RailgunStats>();

            if(railgunStats.IsEnabled) {
                ProjectileHit projectile = obj.GetComponent<ProjectileHit>();
                MoveTransform moveTransform = obj.GetComponent<MoveTransform>();

                RailgunStats.RailgunChargeStats chargeStats = railgunStats.GetChargeStats(railgunStats.CurrentCharge);

                moveTransform.localForce *= chargeStats.ChargeBulletSpeedMultiplier;
                projectile.damage *= chargeStats.ChargeDamageMultiplier;
            }
        }
        
        [HarmonyPatch("DoAttack")]
        [HarmonyPostfix]
        public static void UseRailgunCharge(Gun __instance) {
            var stats = __instance.player.data.GetCustomStatsRegistry().GetOrCreate<RailgunStats>();
            if(stats.IsEnabled && stats.CurrentCharge > 0) {
                stats.UseCharge(stats);
            }
        }

        [HarmonyPatch("ApplyProjectileStats")]
        [HarmonyPostfix]
        public static void ApplyReaperStats(Gun __instance, GameObject obj) {
            ProjectileHit projectile = obj.GetComponent<ProjectileHit>();
            projectile.percentageDamage += MathUtils.GetEffectivePercentCap(
                __instance.player.GetSPS(), 
                __instance.player.data.GetCustomStatsRegistry().GetOrCreate<ReaperStats>().ScalingPercentageDamage,
                __instance.player.data.GetCustomStatsRegistry().GetOrCreate<ReaperStats>().ScalingPercentageDamageCap
            );
            projectile.percentageDamage += MathUtils.GetEffectivePercent(
                __instance.player.GetSPS(),
                __instance.player.data.GetCustomStatsRegistry().GetOrCreate<ReaperStats>().ScalingPercentageDamageUnCap
            );
        }
    }
}
