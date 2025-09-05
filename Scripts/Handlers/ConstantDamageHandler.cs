using JARL.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AALUND13Cards.Handlers {
    public class ConstantDamageHandler : MonoBehaviour {
        public Dictionary<Player, float> playerConstantDamages = new Dictionary<Player, float>();
        public Dictionary<Player, float> PlayerConstantPrecentageDamages = new Dictionary<Player, float>();
        
        public static ConstantDamageHandler Instance;

        public void AddConstantDamage(Player player, float damage) {
            if (!playerConstantDamages.ContainsKey(player)) {
                playerConstantDamages[player] = 0f;
            }
            playerConstantDamages[player] += damage;
        }

        public void AddConstantPrecentageDamage(Player player, float precentage) {
            if (!PlayerConstantPrecentageDamages.ContainsKey(player)) {
                PlayerConstantPrecentageDamages[player] = 0f;
            }
            PlayerConstantPrecentageDamages[player] += precentage;
        }

        private void Start() {
            if (Instance == null) {
                Instance = this;
            } else {
                Destroy(this);
            }

            DeathHandler.OnPlayerDeath += (player, playerDamageInfo) => {
                if (playerConstantDamages.ContainsKey(player)) {
                    playerConstantDamages.Remove(player);
                }

                if (PlayerConstantPrecentageDamages.ContainsKey(player)) {
                    PlayerConstantPrecentageDamages.Remove(player);
                }
            };

        }

        private void Update() {
            foreach (var kvp in playerConstantDamages.ToList()) {
                Player player = kvp.Key;
                float damage = kvp.Value * Time.deltaTime;
                if (damage > 0) {
                    player.data.healthHandler.DoDamage(Vector2.down * damage, Vector2.zero, Color.red, null, null, false, true, true);
                }
            }

            foreach (var kvp in PlayerConstantPrecentageDamages.ToList()) {
                Player player = kvp.Key;
                float precentage = kvp.Value * Time.deltaTime;
                if (precentage > 0) {
                    player.data.healthHandler.DoDamage(Vector2.down * (player.data.maxHealth * precentage), Vector2.zero, Color.black, null, null, false, true, true);
                }
            }
        }
    }
}
