using JARL.Armor;
using JARL.Armor.Bases;
using Photon.Pun;
using System;
using System.Collections.Generic;
using UnboundLib;
using UnboundLib.Networking;
using UnityEngine;

namespace AALUND13Cards.Classes.Armors {
    public class ExoArmor : ArmorBase {
        private static Dictionary<Player, bool[]> ReflectChances = new Dictionary<Player, bool[]>();
        private static Dictionary<Player, int> ReflectIndex = new Dictionary<Player, int>();

        public Action<float> OnArmorDamaged;
        public float ArmorDamageReduction;

        public float ReflectChance = 0f;

        private float queuedDamage;

        public override BarColor GetBarColor() {
            return new BarColor(Color.cyan * 0.6f, Color.cyan * 0.45f);
        }

        public override DamageArmorInfo OnDamage(float damage, Player DamagingPlayer, ArmorDamagePatchType? armorDamagePatchType) {
            queuedDamage += damage;

            float reducedDamage = damage * (1 - ArmorDamageReduction);
            return base.OnDamage(reducedDamage, DamagingPlayer, armorDamagePatchType);
        }

        public override void OnUpdate() {
            if(queuedDamage > 0) {
                OnArmorDamaged?.Invoke(queuedDamage);
                queuedDamage = 0;
            }
        }

        public override void OnRespawn() {
            queuedDamage = 0;

            if(!PhotonNetwork.IsMasterClient) return;
            bool[] bools = new bool[10000];
            for(int i = 0; i < 10000; ++i) bools[i] = UnityEngine.Random.value < ReflectChance;
            NetworkingManager.RPC(typeof(ExoArmor), nameof(ArmorReflectOddsRPCA), ArmorHandler.Player.playerID, bools);
        }

        public bool Reflect(float bulletDmage) {
            if(ReflectIndex.TryGetValue(ArmorHandler.Player, out int index)) {
                if(index < 10000) {
                    DamageArmor(bulletDmage / 2);
                    ReflectIndex[ArmorHandler.Player]++;
                    return ReflectChances[ArmorHandler.Player][index];
                }
            }
            return false;
        }

        public ExoArmor() {
            ArmorTags.Add("CanArmorPierce");
        }

        [UnboundRPC]
        public static void ArmorReflectOddsRPCA(int playerId, bool[] bools) {
            Player player = PlayerManager.instance.players.Find(p => p.playerID == playerId);
            ExoArmor armor = (ExoArmor)ArmorFramework.ArmorHandlers[player].GetArmorByType<ExoArmor>();

            if(!ReflectChances.ContainsKey(player)) {
                ReflectChances.Add(player, bools);
                ReflectIndex.Add(player, 0);
            } else {
                ReflectChances[player] = bools;
                ReflectIndex[player] = 0;
            }
        }
    }
}
