using UnboundLib;
using UnityEngine;

namespace AALUND13Cards.Extensions {
    public static class PlayerExtensions {
        public static float GetDPS(this Player player) {
            float damage = player.data.weaponHandler.gun.damage
                * player.data.weaponHandler.gun.bulletDamageMultiplier
                * player.data.weaponHandler.gun.projectiles[0].objectToSpawn.GetComponent<ProjectileHit>().damage;

            GunAmmo ammoComponent = player.data.weaponHandler.gun.GetComponentInChildren<GunAmmo>();

            int ammo = ammoComponent.maxAmmo;
            int projectiles = player.data.weaponHandler.gun.numberOfProjectiles;
            int bursts = Mathf.Max(player.data.weaponHandler.gun.bursts, 1);

            float timeBetweenShots = player.data.weaponHandler.gun.timeBetweenBullets;
            float reloadTime = (float)ammoComponent.InvokeMethod("ReloadTime");

            float attackSpeed = Mathf.Max(player.data.weaponHandler.gun.attackSpeed, 0.01f);

            int ammoPerTiggerPull = bursts * projectiles;
            int tiggerPull = Mathf.Max(ammo / ammoPerTiggerPull, 1);

            float burstFireTime = (bursts - 1) * timeBetweenShots;

            float totalFireCycleTime = (attackSpeed + burstFireTime) * (tiggerPull - 1) + Mathf.Max(reloadTime, attackSpeed + burstFireTime);

            float firingCycleMultiplier = 1f / totalFireCycleTime;

            return damage * tiggerPull * ammoPerTiggerPull * firingCycleMultiplier / 1.33333f;
        }
    }
}
