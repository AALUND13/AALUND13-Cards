using AALUND13Cards.Classes.Cards;
using AALUND13Cards.Core.Extensions;
using Sonigon;
using SoundImplementation;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace AALUND13Cards.Classes.MonoBehaviours.CardsEffects.Soulstreak {
    public class SoulstreakDrain : MonoBehaviour {
        [Header("Sounds")]
        public SoundEvent SoundDamage;

        [Header("Effects")]
        public GameObject soulDrainEffect;
        public UnityEvent damagePlayerTrigger;

        [Header("Settings")]
        public float Range = 5f;
        public float Cooldown = 0.5f;

        private SoulStreakStats soulstreakStats;
        private Player player;

        private readonly Dictionary<Player, float> timeSinceHits = new Dictionary<Player, float>();
        private readonly Dictionary<Player, GameObject> playerEffects = new Dictionary<Player, GameObject>();
        private readonly Queue<GameObject> unusedEffects = new Queue<GameObject>();

        private void Start() {
            player = GetComponentInParent<Player>();
            soulstreakStats = player.data.GetCustomStatsRegistry().GetOrCreate<SoulStreakStats>();

            var groups = SoundVolumeManager.Instance.audioMixer.FindMatchingGroups("SFX");
            if(groups.Length > 0) {
                SoundDamage.variables.audioMixerGroup = groups[0];
            }

            soulDrainEffect.SetActive(true);
            unusedEffects.Enqueue(soulDrainEffect);
        }

        private void Update() {
            List<Player> enemiesInRange = GetEnemiesInRange();
            List<Player> enemiesOutOfRange = playerEffects.Keys.ToList();

            bool triggered = false;

            foreach(Player enemy in enemiesInRange) {
                if(!timeSinceHits.TryGetValue(enemy, out float lastHit) || Time.time > lastHit + Cooldown) {
                    TriggerDamageForPlayer(enemy);
                    triggered = true;
                }

                enemiesOutOfRange.Remove(enemy);
                ShowEffectForPlayer(enemy);
            }

            foreach(Player enemy in enemiesOutOfRange) {
                HideEffectForPlayer(enemy);
            }

            if(triggered) {
                damagePlayerTrigger.Invoke();
            }
        }

        private GameObject GetEffectForPlayer(Player target) {
            if(playerEffects.TryGetValue(target, out var effect)) {
                return effect;
            }

            if(unusedEffects.Count > 0) {
                effect = unusedEffects.Dequeue();
            } else {
                effect = Instantiate(soulDrainEffect, transform);
            }

            effect.GetComponentInChildren<ParticleSystem>().Play();

            playerEffects[target] = effect;
            return effect;
        }

        private void ShowEffectForPlayer(Player target) {
            GameObject effect = GetEffectForPlayer(target);
            effect.transform.position = target.transform.position;

            Vector3 dir = (target.transform.position - player.transform.position).normalized;
            if(dir != Vector3.zero) {
                effect.transform.rotation = Quaternion.LookRotation(dir);
            }
        }

        private void HideEffectForPlayer(Player target) {
            if(!playerEffects.TryGetValue(target, out var effect))
                return;

            effect.GetComponentInChildren<ParticleSystem>().Stop();

            unusedEffects.Enqueue(effect);
            playerEffects.Remove(target);
        }

        public void TriggerDamageForPlayer(Player target) {
            if(soulstreakStats == null) return;

            float damage = GetDamage(target) + GetPercentageDamage(target);
            float actualDamage = Mathf.Min(damage, target.data.health);

            Vector2 dir = (target.transform.position - transform.position).normalized;
            float lifesteal = Mathf.Max(0f, actualDamage * soulstreakStats.SoulDrainLifestealMultiply);

            target.data.healthHandler.TakeDamage(
                dir * damage,
                transform.position,
                null,
                player,
                true,
                true
            );

            player.data.healthHandler.Heal(lifesteal);

            SoundManager.Instance.Play(SoundDamage, target.transform);

            timeSinceHits[target] = Time.time;
        }

        private float GetDamage(Player target) {
            float dps = target.GetDPS();
            return dps * soulstreakStats.SoulDrainDPSFactor * Cooldown;
        }

        private float GetPercentageDamage(Player target) {
            float percentageDamage = player.GetDPS() * soulstreakStats.SoulDrainPercentageDPSFactor * Cooldown;
            return target.data.maxHealth * percentageDamage;
        }

        private List<Player> GetEnemiesInRange() {
            List<Player> enemies = new List<Player>();

            foreach(Player enemy in PlayerManager.instance.players) {
                if(enemy.teamID == player.teamID || !enemy.data.isPlaying || enemy.data.dead)
                    continue;

                var info = PlayerManager.instance.CanSeePlayer(player.transform.position, enemy);
                if(!info.canSee)
                    continue;

                if(Vector2.Distance(player.transform.position, enemy.transform.position) >= Range * transform.root.localScale.x)
                    continue;

                enemies.Add(enemy);
            }

            return enemies;
        }
    }
}
