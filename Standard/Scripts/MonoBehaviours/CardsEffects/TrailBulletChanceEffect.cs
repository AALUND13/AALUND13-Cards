using System.Linq;
using UnityEngine;

namespace AALUND13Cards.Standard.MonoBehaviours.CardsEffects {
    public class TrailBulletChanceEffect : MonoBehaviour {
        public const string TRAIL_BULLET_KEY = "TrailBulletApply";

        public Color TrailProjectileColor;
        public ObjectsToSpawn ObjectsToSpawn;
        public float TrailBulletChance = 0.2f;

        private Gun gun;
        private ChildRPC childRPC;
        private bool TrailBulletApply = false;

        private void Start() {
            gun = GetComponentInParent<Player>().data.weaponHandler.gun;
            childRPC = GetComponentInParent<ChildRPC>();

            childRPC.childRPCs.Add(TRAIL_BULLET_KEY, SyncTrailBullet);
            gun.ShootPojectileAction += OnShootProjectileAction;
        }

        private void OnDestroy() {
            childRPC.childRPCs.Remove(TRAIL_BULLET_KEY);
            gun.ShootPojectileAction -= OnShootProjectileAction;
        }

        public void OnShootProjectileAction(GameObject obj) {
            if(TrailBulletApply) {
                gun.objectsToSpawn = gun.objectsToSpawn.Except(new ObjectsToSpawn[] { ObjectsToSpawn }).ToArray();
                TrailBulletApply = false;

                if(TrailProjectileColor != Color.black) {
                    float r = Mathf.Pow((TrailProjectileColor.r * TrailProjectileColor.r + TrailProjectileColor.r * TrailProjectileColor.r) / 2f, 0.5f);
                    float g = Mathf.Pow((TrailProjectileColor.g * TrailProjectileColor.g + TrailProjectileColor.g * TrailProjectileColor.g) / 2f, 0.5f);
                    float b = Mathf.Pow((TrailProjectileColor.b * TrailProjectileColor.b + TrailProjectileColor.b * TrailProjectileColor.b) / 2f, 0.5f);
                    Color rgbColor = new Color(r, g, b, 1f);
                    Color.RGBToHSV(rgbColor, out float H, out float S, out float V);
                    obj.GetComponent<SpawnedAttack>().SetColor(Color.HSVToRGB(H, 1, 1));
                }
            }

            if(gun.player.data.view.IsMine && UnityEngine.Random.value < TrailBulletChance) {
                childRPC.CallFunction(TRAIL_BULLET_KEY);
            }
        }

        public void SyncTrailBullet() {
            gun.objectsToSpawn = gun.objectsToSpawn.Concat(new ObjectsToSpawn[] { ObjectsToSpawn }).ToArray();
            TrailBulletApply = true;
        }
    }
}
