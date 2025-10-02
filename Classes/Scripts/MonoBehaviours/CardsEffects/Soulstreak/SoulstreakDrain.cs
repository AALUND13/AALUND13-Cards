using AALUND13Cards.Classes.Cards;
using AALUND13Cards.Core;
using AALUND13Cards.Core.Extensions;
using Sonigon;
using SoundImplementation;
using UnityEngine;

namespace AALUND13Cards.Classes.MonoBehaviours.CardsEffects.Soulstreak {
    public class SoulstreakDrain : MonoBehaviour {
        [Header("Sounds")]
        public SoundEvent SoundDamage;

        private SoulStreakStats soulstreakStats;
        private PlayerInRangeTrigger playerInRangeTrigger;

        private Player player;

        public void Start() {
            player = GetComponentInParent<Player>();
            soulstreakStats = player.data.GetCustomStatsRegistry().GetOrCreate<SoulStreakStats>();

            SoundDamage.variables.audioMixerGroup = SoundVolumeManager.Instance.audioMixer.FindMatchingGroups("SFX")[0];

            playerInRangeTrigger = GetComponent<PlayerInRangeTrigger>();
        }

        public void TriggerDamage() {
            if(soulstreakStats == null) return;

            Player closestEnemy = GetClosestEnemy();
            if(closestEnemy == null) return;

            float dps = player.GetDPS();
            float damage = dps * soulstreakStats.SoulDrainDPSFactor * playerInRangeTrigger.cooldown;
            float actualDamage = Mathf.Min(damage, closestEnemy.data.health);

            float lifesteal = Mathf.Max(0f, actualDamage * soulstreakStats.SoulDrainLifestealMultiply);

            closestEnemy.data.healthHandler.TakeDamage(damage * Vector2.up, transform.position, null, player, true, true);
            player.data.healthHandler.Heal(lifesteal);

            SoundManager.Instance.Play(SoundDamage, closestEnemy.transform);
            LoggerUtils.LogInfo($"DPS: {dps}, Damage: {damage}, Lifesteal: {lifesteal}");
        }

        private Player GetClosestEnemy() {
            Player closestEnemy = null;
            float closestDistance = float.MaxValue;
            foreach(Player enemy in PlayerManager.instance.players) {
                if(enemy != player && enemy.data.isPlaying) {
                    float distance = Vector2.Distance(player.transform.position, enemy.transform.position);
                    if(distance < closestDistance) {
                        closestDistance = distance;
                        closestEnemy = enemy;
                    }
                }
            }
            return closestEnemy;
        }
    }
}
