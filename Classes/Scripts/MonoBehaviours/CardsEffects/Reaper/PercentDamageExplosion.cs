using UnboundLib;
using UnityEngine;

namespace AALUND13Cards.Classes.MonoBehaviours.CardsEffects.Reaper {
    [RequireComponent(typeof(SpawnedAttack))]
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

        public void Trigger() {
            if(spawnedAttack.IsMine()) {
                float scaleRanage = Ranage * transform.localScale.x;
                float scalePercentDamage = PercentDamage;
                if(ScaleWithLevel && attackLevel != null) {
                    scalePercentDamage *= attackLevel.attackLevel;
                }

                foreach(Player player in ModdingUtils.Utils.PlayerStatus.GetEnemyPlayers(spawnedAttack.spawner)) {
                    if(!PlayerManager.instance.CanSeePlayer(transform.position, player).canSee) return;

                    float damageFromPercent = player.data.maxHealth * scalePercentDamage;
                    float distance = Vector2.Distance(transform.position, player.transform.position);
                    float value = CalculateScaledValue(distance, damageFromPercent, scaleRanage);

                    if(value > 0f) {
                        Vector3 dir = (player.transform.position - transform.position).normalized;
                        player.data.healthHandler.CallTakeDamage(value * dir, transform.position, null, spawnedAttack.spawner);

                        SpawnPlayerDamageEffect(player);
                    }
                }
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
    }
}
