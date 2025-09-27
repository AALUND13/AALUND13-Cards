using JARL.Utils;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AALUND13Cards.Handlers {
    public struct ConstantDamageInfo {
        public Player DamagingPlayer;
        public Color Color;
        public float Damage;

        public ConstantDamageInfo(Player damagingPlayer, Color color, float damage) {
            DamagingPlayer = damagingPlayer;
            Color = color;
            Damage = damage;
        }

        public ConstantDamageInfo AddDamage(float damage) {
            return new ConstantDamageInfo(DamagingPlayer, Color, Damage + damage);
        }
    }

    public class ConstantDamageHandler : MonoBehaviour {
        public Dictionary<Player, List<ConstantDamageInfo>> playerConstantDamages = new Dictionary<Player, List<ConstantDamageInfo>>();
        public Dictionary<Player, List<ConstantDamageInfo>> PlayerConstantPrecentageDamages = new Dictionary<Player, List<ConstantDamageInfo>>();

        public static ConstantDamageHandler Instance;

        public void AddConstantDamage(Player player, Player damagingPlayer, Color color, float damage) {
            if(!playerConstantDamages.ContainsKey(player)) {
                playerConstantDamages[player] = new List<ConstantDamageInfo>();
            }

            int index = playerConstantDamages[player].FindIndex(info => info.Color == color);
            if(index != -1) {
                playerConstantDamages[player][index] = playerConstantDamages[player][index].AddDamage(damage);
            } else {
                playerConstantDamages[player].Add(new ConstantDamageInfo(damagingPlayer, color, damage));
            }
        }

        public void AddConstantPrecentageDamage(Player player, Player damagingPlayer, Color color, float precentage) {
            if(!PlayerConstantPrecentageDamages.ContainsKey(player)) {
                PlayerConstantPrecentageDamages[player] = new List<ConstantDamageInfo>();
            }

            int index = PlayerConstantPrecentageDamages[player].FindIndex(info => info.Color == color);
            if(index != -1) {
                PlayerConstantPrecentageDamages[player][index] = PlayerConstantPrecentageDamages[player][index].AddDamage(precentage);
            } else {
                PlayerConstantPrecentageDamages[player].Add(new ConstantDamageInfo(damagingPlayer, color, precentage));
            }
        }

        internal void RemovePlayerFromAll(Player player) {
            if(playerConstantDamages.ContainsKey(player)) {
                playerConstantDamages.Remove(player);
            }
            if(PlayerConstantPrecentageDamages.ContainsKey(player)) {
                PlayerConstantPrecentageDamages.Remove(player);
            }
        }

        private void Start() {
            if(Instance == null) {
                Instance = this;
            } else {
                Destroy(this);
            }

            DeathHandler.OnPlayerDeath += (player, playerDamageInfo) => {
                if(playerConstantDamages.ContainsKey(player)) {
                    playerConstantDamages.Remove(player);
                }

                if(PlayerConstantPrecentageDamages.ContainsKey(player)) {
                    PlayerConstantPrecentageDamages.Remove(player);
                }
            };

        }

        private void Update() {
            foreach(var kvp in playerConstantDamages.ToList()) {
                foreach(var constantDamageInfo in kvp.Value) {
                    Player player = kvp.Key;
                    float damage = constantDamageInfo.Damage * Time.deltaTime;

                    if(damage > 0) {
                        player.data.healthHandler.DoDamage(Vector2.down * damage, Vector2.zero, constantDamageInfo.Color, null, null, false, true, true);
                    }
                }
            }

            foreach(var kvp in PlayerConstantPrecentageDamages.ToList()) {
                foreach(var constantDamageInfo in kvp.Value) {
                    Player player = kvp.Key;
                    float precentage = constantDamageInfo.Damage * Time.deltaTime;

                    if(precentage > 0) {
                        player.data.healthHandler.DoDamage(Vector2.down * (player.data.maxHealth * precentage), Vector2.zero, constantDamageInfo.Color, null, null, false, true, true);
                    }
                }
            }
        }
    }
}
