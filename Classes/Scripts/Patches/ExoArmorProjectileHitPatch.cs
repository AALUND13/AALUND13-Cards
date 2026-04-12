using AALUND13Cards.Classes.Armors;
using HarmonyLib;
using JARL.Armor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnboundLib;

namespace AALUND13Cards.Classes.Patches {
    internal class ExoArmorProjectileHitPatch {
        public static void Patch(Harmony harmony) {
            var original = AccessTools.Method(typeof(ProjectileHit), nameof(ProjectileHit.Hit));
            var prefix = AccessTools.Method(typeof(ExoArmorProjectileHitPatch), nameof(Prefix));

            harmony.Patch(original, prefix: new HarmonyMethod(prefix));
        }

        private static bool Prefix(ProjectileHit __instance, HitInfo hit, bool forceCall) {
            HealthHandler healthHandler = null;
            if(hit.transform) {
                healthHandler = hit.transform.GetComponent<HealthHandler>();
            }
            if(healthHandler) {
                Player hitPlayer = healthHandler.GetComponent<Player>();

                ExoArmor armor = (ExoArmor)ArmorFramework.ArmorHandlers[hitPlayer].GetArmorByType<ExoArmor>();
                if(hitPlayer == null) return true;

                if(armor.IsActive && armor.Reflect(GetBulletDamage(__instance.GetComponent<ProjectileHit>(), hitPlayer))) {
                    __instance.GetComponent<ProjectileHit>().RemoveOwnPlayerFromPlayersHit();
                    __instance.GetComponent<ProjectileHit>().AddPlayerToHeld(healthHandler);
                    __instance.GetComponent<MoveTransform>().velocity *= -1f;
                    __instance.transform.position += __instance.GetComponent<MoveTransform>().velocity * TimeHandler.deltaTime;
                    __instance.GetComponent<RayCastTrail>().WasBlocked();
                    if(__instance.destroyOnBlock) {
                        __instance.InvokeMethod("DestroyMe");
                    }
                    __instance.sinceReflect = 0f;
                    return false;
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
