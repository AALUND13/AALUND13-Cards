using AALUND13Cards.Standard.Handler;
using System.Linq;
using UnityEngine;

namespace AALUND13Cards.Standard.MonoBehaviours.CardsEffects {
    public class ProjectileEffectChanceMono : MonoBehaviour {
        [Header("Sync Time")]
        public string RPCSyncName = "TrailBulletApply";

        [Header("Projectile")]
        public float ProjectileEffectChance = 0.2f;
        public Color ProjectileColor = Color.black;
        public ObjectsToSpawn ObjectsToSpawn;

        private Player player;
        private Gun applyGun;
        private ChildRPC childRPC;
        private bool TrailBulletApply = false;

        private void Start() {
            player = GetComponentInParent<Player>();
            childRPC = GetComponentInParent<ChildRPC>();

            PlayerGunActions.AddShootAction(GetComponentInParent<Player>(), OnShootProjectileAction);
            childRPC.childRPCs.Add(RPCSyncName, SyncTrailBullet);
        }

        private void OnDestroy() {
            PlayerGunActions.RemoveShootAction(GetComponentInParent<Player>(), OnShootProjectileAction);
            childRPC.childRPCs.Remove(RPCSyncName);
        }

        public void OnShootProjectileAction(Gun gun, GameObject obj) {
            if(TrailBulletApply && applyGun != null) {
                applyGun.objectsToSpawn = applyGun.objectsToSpawn.Except(new ObjectsToSpawn[] { ObjectsToSpawn }).ToArray();
                TrailBulletApply = false;
                applyGun = null;

                if(ProjectileColor != Color.black) {
                    float r = Mathf.Pow((ProjectileColor.r * ProjectileColor.r + ProjectileColor.r * ProjectileColor.r) / 2f, 0.5f);
                    float g = Mathf.Pow((ProjectileColor.g * ProjectileColor.g + ProjectileColor.g * ProjectileColor.g) / 2f, 0.5f);
                    float b = Mathf.Pow((ProjectileColor.b * ProjectileColor.b + ProjectileColor.b * ProjectileColor.b) / 2f, 0.5f);
                    Color rgbColor = new Color(r, g, b, 1f);
                    Color.RGBToHSV(rgbColor, out float H, out float S, out float V);
                    obj.GetComponent<SpawnedAttack>().SetColor(Color.HSVToRGB(H, 1, 1));
                }
            }

            if(player.data.view.IsMine && UnityEngine.Random.value < ProjectileEffectChance) {
                childRPC.CallFunction(RPCSyncName);
            }
        }

        public void SyncTrailBullet() {
            applyGun = player.data.weaponHandler.gun;
            applyGun.objectsToSpawn = applyGun.objectsToSpawn.Concat(new ObjectsToSpawn[] { ObjectsToSpawn }).ToArray();
            TrailBulletApply = true;
        }
    }
}
