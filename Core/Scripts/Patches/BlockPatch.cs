using AALUND13Cards.Core.Extensions;
using HarmonyLib;
using System.Collections.Generic;
using UnboundLib;
using UnityEngine;

namespace AALUND13Cards.Core.Patches {
    [HarmonyPatch(typeof(Block))]
    internal class BlockPatch {
        [HarmonyPatch("blocked")]
        [HarmonyPrefix]
        public static void BlockedPrefix(Block __instance, GameObject projectile, Vector3 forward, Vector3 hitPos) {
            ProjectileHit projectileHit = projectile.GetComponent<ProjectileHit>();
            HealthHandler healthHandler = (HealthHandler)__instance.GetFieldValue("health");

            float blockPircePercent = projectileHit.ownPlayer.data.GetAdditionalData().BlockPircePercent;

            if(blockPircePercent > 0f) {
                Vector2 damage = (projectileHit.bulletCanDealDeamage ? projectileHit.damage : 1f) * blockPircePercent * forward.normalized;
                healthHandler.TakeDamage(damage, hitPos, projectileHit.projectileColor, projectileHit.ownWeapon, projectileHit.ownPlayer, true, true);

                GameObject.Destroy(projectile);
            }
        }
    }
}
