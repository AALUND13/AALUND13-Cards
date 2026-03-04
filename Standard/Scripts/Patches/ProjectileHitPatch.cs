using AALUND13Cards.Core.Extensions;
using AALUND13Cards.Standard.Cards;
using HarmonyLib;
using UnityEngine;

namespace AALUND13Cards.Standard.Patches {
    [HarmonyPatch(typeof(ProjectileHit), "Hit")]
    internal class ProjectileHitPatch {
        private static bool Prefix(ProjectileHit __instance, HitInfo hit, bool forceCall) {
            HealthHandler healthHandler = null;
            if(hit.transform) {
                healthHandler = hit.transform.GetComponent<HealthHandler>();
            }
            if(healthHandler) {
                Player hitPlayer = healthHandler.GetComponent<Player>();

                if(hitPlayer == null) return true;
                if(__instance.ownPlayer.data.GetCustomStatsRegistry().GetOrCreate<StandardStats>().BleedingDamage > 0) {
                    hitPlayer.data.healthHandler.TakeDamageOverTime(
                        __instance.transform.forward * __instance.damage * __instance.ownPlayer.data.GetCustomStatsRegistry().GetOrCreate<StandardStats>().BleedingDamage,
                        hit.point, 5, 1f, Color.red * 0.8f, __instance.ownWeapon, __instance.ownPlayer, true
                    );
                }
            }
            return true;
        }
    }
}
