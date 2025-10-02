using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

namespace AALUND13Cards.Core.MonoBehaviours.ProjectilesEffects {
    public class ConnectingBullets : MonoBehaviour {
        private static Dictionary<Player, GameObject> PlayerLastBullet = new Dictionary<Player, GameObject>();

        public float DamageInterval = 0.5f;
        public float DamageMultiplier = 0.75f;
        public float MaxDistance = 20f;
        public float SpawnTimeTrigger = 0.5f; // Time after spawn to start checking for connection

        public GameObject ConnectingBulletPrefab;

        private Dictionary<Player, float> lastDamageTime = new Dictionary<Player, float>();
        private GameObject connectingBulletInstance;
        private GameObject connectedBullet;
        private ProjectileHit ProjectileHit;
        private ChildRPC childRPC;
        private float spawnTime;

        private void Start() {
            ProjectileHit = GetComponentInParent<ProjectileHit>();
            childRPC = GetComponentInParent<ChildRPC>();

            if(PlayerLastBullet.ContainsKey(ProjectileHit.ownPlayer) 
                && PlayerLastBullet[ProjectileHit.ownPlayer] != null 
                && Vector2.Distance(PlayerLastBullet[ProjectileHit.ownPlayer].transform.position, gameObject.transform.position) < MaxDistance
            ) {
                connectingBulletInstance = Instantiate(ConnectingBulletPrefab, Vector2.zero, Quaternion.identity);
                var lineEffect = connectingBulletInstance.GetComponent<LineEffect>();
                connectedBullet = PlayerLastBullet[ProjectileHit.ownPlayer];

                lineEffect.fromPos = connectedBullet.transform;
                lineEffect.toPos = gameObject.transform;

                var lineRenderer = connectingBulletInstance.GetComponent<LineRenderer>();
                lineRenderer.startColor = GetProjectileColor(connectedBullet.GetComponent<ProjectileHit>());
                lineRenderer.endColor = GetProjectileColor(ProjectileHit);
                lineRenderer.SetPositions(new Vector3[] { connectedBullet.transform.position, gameObject.transform.position });
            }

            PlayerLastBullet[ProjectileHit.ownPlayer] = transform.parent.gameObject;
            spawnTime = Time.time;
        }

        private Color GetProjectileColor(ProjectileHit projectileHit) {
            if(projectileHit.projectileColor == Color.black)
                return new Color(1 * 0.75f, 0.3804f * 0.75f, 0.1176f * 0.75f);
            return projectileHit.projectileColor * 0.75f;
        }

        private void Update() {
            if(connectedBullet == null && connectingBulletInstance != null) {
                Destroy(gameObject);
            } else if(connectingBulletInstance != null && Vector2.Distance(connectedBullet.transform.position, gameObject.transform.position) > MaxDistance) {
                Destroy(gameObject);
            }
        }

        private void FixedUpdate() {
            if(connectingBulletInstance != null && connectedBullet != null) {
                DamageBetweenPoints(connectedBullet.transform.position, gameObject.transform.position);
            }
        }

        private void OnDestroy() {
            if(connectingBulletInstance != null) {
                Destroy(connectingBulletInstance);
            }
        }

        private void DamageBetweenPoints(Vector2 pointA, Vector2 pointB) {
            if(!ProjectileHit.ownPlayer.data.view.IsMine) return;

            Vector2 center = (pointA + pointB) / 2f;

            RaycastHit2D[] hits = Physics2D.LinecastAll(pointA, pointB);
            foreach(RaycastHit2D hit in hits) {
                var player = hit.collider.GetComponent<Player>();
                if(player != null) {
                    if(player == ProjectileHit.ownPlayer && Time.time < spawnTime + SpawnTimeTrigger) continue; // Don't hit self right after spawn
                    if(!lastDamageTime.ContainsKey(player)) lastDamageTime[player] = 0;
                    if(Time.time - lastDamageTime[player] >= DamageInterval) {
                        lastDamageTime[player] = Time.time;
                        Vector2 dir = (player.transform.position - (Vector3)center).normalized;
                        player.data.healthHandler.CallTakeDamage(dir * GetChainDamage(player), player.transform.position, ProjectileHit.ownWeapon, ProjectileHit.ownPlayer);
                    }
                }
            }
        }

        private float GetChainDamage(Player targetPlayer) {
            float damage = ProjectileHit.damage;
            float otherProjectileDamage = connectedBullet.GetComponent<ProjectileHit>().damage;
            
            float percentageDamage = ProjectileHit.percentageDamage;
            float otherPercentageDamage = connectedBullet.GetComponent<ProjectileHit>().percentageDamage;

            return (((damage + otherProjectileDamage) / 2f) + (targetPlayer.data.maxHealth * (percentageDamage + otherPercentageDamage) / 2f)) * DamageMultiplier;
        }
    }
}
