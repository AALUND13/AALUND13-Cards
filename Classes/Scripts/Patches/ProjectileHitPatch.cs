using AALUND13Cards.Classes.Armors;
using AALUND13Cards.Classes.Cards;
using AALUND13Cards.Core.Extensions;
using HarmonyLib;
using JARL.Armor;
using Photon.Pun;
using UnboundLib;
using UnityEngine;

namespace AALUND13Cards.Classes.Patches {
    [HarmonyPatch(typeof(ProjectileHit), "RPCA_DoHit")]
    internal class ProjectileHitPatch {
        private static bool Prefix(ProjectileHit __instance, Vector2 hitPoint, Vector2 hitNormal, int viewID = -1, int colliderID = -1, bool wasBlocked = false) {
            HitInfo hitInfo = new HitInfo();

            hitInfo.point = hitPoint;
            hitInfo.normal = hitNormal;
            hitInfo.collider = null;
            if(viewID != -1) {
                PhotonView photonView = PhotonNetwork.GetPhotonView(viewID);
                hitInfo.collider = photonView.GetComponentInChildren<Collider2D>();
                hitInfo.transform = photonView.transform;
            } else if(colliderID != -1) {
                hitInfo.collider = MapManager.instance.currentMap.Map.GetComponentsInChildren<Collider2D>()[colliderID];
                hitInfo.transform = hitInfo.collider.transform;
            }

            HealthHandler healthHandler = null;
            if((bool)hitInfo.transform) {
                healthHandler = hitInfo.transform.GetComponent<HealthHandler>();
            }

            if(healthHandler) {
                Player hitPlayer = healthHandler.GetComponent<Player>();

                if(hitPlayer == null) return true;
                if(__instance.ownPlayer.data.GetCustomStatsRegistry().GetOrCreate<ReaperStats>().PercentageDamageBleedingPercentage > 0) {
                    float percentageDamage = GetBulletDamage(__instance, hitPlayer);
                    hitPlayer.data.healthHandler.TakeDamageOverTime(
                        __instance.transform.forward * percentageDamage * __instance.ownPlayer.data.GetCustomStatsRegistry().GetOrCreate<ReaperStats>().PercentageDamageBleedingPercentage,
                        hitPoint, 5, 1f, Color.red * 0.8f, __instance.ownWeapon, __instance.ownPlayer, true
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
