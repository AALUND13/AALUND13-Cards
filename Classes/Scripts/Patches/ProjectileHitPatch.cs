using AALUND13Cards.Classes.Armors;
using AALUND13Cards.Classes.Cards;
using AALUND13Cards.Core.Extensions;
using HarmonyLib;
using JARL.Armor;
using UnboundLib;
using UnityEngine;

namespace AALUND13Cards.Classes.Patches {
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
                if(__instance.ownPlayer.data.GetCustomStatsRegistry().GetOrCreate<ReaperStats>().PercentageDamageBleedingPercentage > 0) {
                    float percentageDamage = GetBulletDamage(__instance, hitPlayer);
                    hitPlayer.data.healthHandler.TakeDamageOverTime(
                        __instance.transform.forward * percentageDamage * __instance.ownPlayer.data.GetCustomStatsRegistry().GetOrCreate<ReaperStats>().PercentageDamageBleedingPercentage,
                        hit.point, 5, 1f, Color.red * 0.8f, __instance.ownWeapon, __instance.ownPlayer, true
                    );
                }
            }
            return true;
        }

        private static float GetBulletDamage(ProjectileHit projectileHit, Player targetPlayer) {
            float damage = projectileHit.damage;
            float percentageDamage = projectileHit.percentageDamage;

            return damage + (targetPlayer.data.maxHealth * percentageDamage);
        }
    }
}
