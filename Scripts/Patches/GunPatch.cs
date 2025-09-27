using AALUND13Cards.Extensions;
using AALUND13Cards.MonoBehaviours.CardsEffects;
using AALUND13Cards.Utils;
using HarmonyLib;
using System.Collections.Generic;
using System.Reflection.Emit;
using UnityEngine;

namespace AALUND13Cards.Patches {
    [HarmonyPatch(typeof(Gun))]
    public class GunPatch {
        [HarmonyPatch("ApplyProjectileStats")]
        public static void Prefix(Gun __instance, GameObject obj) {
            RailgunStats railgunStats = __instance.player.data.GetAdditionalData().CustomStatsManager.GetOrCreate<RailgunStats>();

            if(railgunStats.IsEnabled) {
                ProjectileHit projectile = obj.GetComponent<ProjectileHit>();
                MoveTransform moveTransform = obj.GetComponent<MoveTransform>();

                RailgunStats.RailgunChargeStats chargeStats = railgunStats.GetChargeStats(railgunStats.CurrentCharge);

                moveTransform.localForce *= chargeStats.ChargeBulletSpeedMultiplier;
                projectile.damage *= chargeStats.ChargeDamageMultiplier;
            }
        }

        [HarmonyPatch("ApplyProjectileStats")]
        public static void Postfix(Gun __instance, GameObject obj) {
            ProjectileHit projectile = obj.GetComponent<ProjectileHit>();
            projectile.percentageDamage += MathUtils.GetEffectivePercentCap(
                __instance.player.GetSPS(), 
                __instance.player.data.GetAdditionalData().ScalingPercentageDamage,
                __instance.player.data.GetAdditionalData().ScalingPercentageDamageCap
            );
            projectile.percentageDamage += MathUtils.GetEffectivePercent(
                __instance.player.GetSPS(),
                __instance.player.data.GetAdditionalData().ScalingPercentageDamageUnCap
            );
        }

        [HarmonyPatch("DoAttack")]
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions) {
            foreach(var code in instructions) {
                if(code.opcode == OpCodes.Ret) {
                    yield return new CodeInstruction(OpCodes.Ldarg_0);
                    yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(GunPatch), nameof(InjectUseCharge)));
                }
                yield return code;
            }
        }

        public static void InjectUseCharge(Gun gun) {
            var stats = gun.player.data.GetAdditionalData().CustomStatsManager.GetOrCreate<RailgunStats>();
            if(stats.IsEnabled && stats.CurrentCharge > 0) {
                stats.UseCharge(stats);
            }
        }
    }
}
