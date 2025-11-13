using UnboundLib;
using UnityEngine;

namespace AALUND13Cards.Classes.MonoBehaviours.CardsEffects.Reaper {
    public class PercentDamageExplosion : MonoBehaviour {
        [Header("Settings")]
        public float PercentDamage = 0.45f;
        public float Ranage = 10f;

        [Header("Scales")]
        public bool ScaleWithLevel = false;

        [Header("Trigger")]
        public bool TriggerOnAwake = false;
        public GameObject PlayerDamageEffect;


        private AttackLevel attackLevel;
        private SpawnedAttack spawnedAttack;

        public float CalculateScaledValue(float value, float maxValue, float scalingRange) {
            float normalizedValue = value / scalingRange;
            return Mathf.Clamp(((normalizedValue * -normalizedValue) * normalizedValue + 1) * maxValue, 0f, maxValue);
        }

        public void Trigger(Player player) {
            float scaleRanage = Ranage * transform.localScale.x;
            float scalePercentDamage = PercentDamage;
            if(ScaleWithLevel && attackLevel != null) {
                scalePercentDamage *= attackLevel.attackLevel;
            }

            foreach(Player playerToDamage in ModdingUtils.Utils.PlayerStatus.GetEnemyPlayers(player)) {
                if(!PlayerManager.instance.CanSeePlayer(transform.position, playerToDamage).canSee) return;

                float damageFromPercent = playerToDamage.data.maxHealth * scalePercentDamage;
                float distance = Vector2.Distance(transform.position, playerToDamage.transform.position);
                float value = CalculateScaledValue(distance, damageFromPercent, scaleRanage);

                if(value > 0f) {
                    Vector3 dir = (playerToDamage.transform.position - transform.position).normalized;
                    playerToDamage.data.healthHandler.CallTakeDamage(value * dir, transform.position, null, playerToDamage);

                    SpawnPlayerDamageEffect(playerToDamage);
                }
            }
        }

        public void Trigger() {
            if(spawnedAttack != null && spawnedAttack.IsMine()) {
                Trigger(spawnedAttack.spawner);
            }
        }

        private void SpawnPlayerDamageEffect(Player player) {
            if(PlayerDamageEffect == null) return;

            GameObject effect = GameObject.Instantiate(PlayerDamageEffect, transform);
            effect.transform.position = player.transform.position;
            effect.SetActive(true);

            Vector3 dir = (player.transform.position - transform.position).normalized;
            if(dir != Vector3.zero) effect.transform.rotation = Quaternion.LookRotation(dir);
        }

        private void Start() {
            spawnedAttack = GetComponent<SpawnedAttack>();
            attackLevel = GetComponent<AttackLevel>();

            if(PlayerDamageEffect != null) PlayerDamageEffect.SetActive(false);
        }

        private void Awake() {
            if(TriggerOnAwake) {
                this.ExecuteAfterFrames(1, () => Trigger());
            }
        }

        private void OnDrawGizmosSelected() {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, Ranage * transform.localScale.x);
        }
    }
}
