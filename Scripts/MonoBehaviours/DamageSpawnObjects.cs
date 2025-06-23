using AALUND13Cards.Handlers;
using UnityEngine;

namespace AALUND13Cards.MonoBehaviours {
    [RequireComponent(typeof(DamageSpawnObjects))]
    public class DamageSpawnObjects : SpawnObjects, IOnDoDamageEvent, IOnTakeDamageEvent, IOnTakeDamageOvertimeEvent {
        public float DamageThreshold = 0.5f; // Minimum damage to trigger spawning

        public bool TriggerOnDamage = false;
        public bool TriggerOnTakeDamage = true;
        public bool TriggerOnOvertimeDamage = true;

        private Player player;

        public void Start() {
            player = GetComponentInParent<Player>();
            DamageEventHandler.RegisterDamageEvent(this, player);
        }

        public void OnDestroy() {
            DamageEventHandler.UnregisterDamageEvent(this, player);
        }

        public void OnDamage(Vector2 damage) {
            if(TriggerOnDamage && damage.magnitude >= DamageThreshold) {
                SpawnDamage(damage);
            }
        }

        public void OnTakeDamage(Vector2 damage) {
            if(TriggerOnTakeDamage && damage.magnitude >= DamageThreshold) {
                SpawnDamage(damage);
            }
        }

        public void OnTakeDamageOvertime(Vector2 damage) {
            if(TriggerOnOvertimeDamage && damage.magnitude >= DamageThreshold) {
                SpawnDamage(damage);
            }
        }

        public void SpawnDamage(Vector2 damage) {
            foreach(var spawnedAttack in objectToSpawn) {
                DamageSpawnedAttack damageSpawnedAttack = spawnedAttack.GetComponent<DamageSpawnedAttack>();
                if(damageSpawnedAttack != null) {
                    damageSpawnedAttack.Damage = damage;
                }
            }
            Spawn();
        }
    }
}
