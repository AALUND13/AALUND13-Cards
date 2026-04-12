using AALUND13Cards.Armors.Cards;
using AALUND13Cards.Core.Extensions;
using HarmonyLib;
using JARL.Armor;
using System.Collections.Generic;
using UnboundLib;
using UnityEngine;

namespace AALUND13Cards.Armors.Patches {
    [HarmonyPatch(typeof(HealthHandler), nameof(HealthHandler.Heal))]
    public class HealthHandlerHealPatch {
        public static Dictionary<Player, float> OldHealth = new Dictionary<Player, float>();

        public static void Prefix(HealthHandler __instance) {
            Player player = (Player)__instance.GetFieldValue("player");
            if(!OldHealth.ContainsKey(player)) {
                OldHealth.Add(player, 0);
            }

            OldHealth[player] = player.data.health;
        }

        public static void Postfix(HealthHandler __instance, float healAmount) {
            if(healAmount == 0f) return;

            Player player = (Player)__instance.GetFieldValue("player");

            float healthAdded = player.data.health - OldHealth[player];
            float overflowHeal = Mathf.Max(healAmount - healthAdded, 0f);
            if(overflowHeal <= 0f) return;

            float remaining = overflowHeal * player.data.GetCustomStatsRegistry().GetOrCreate<ArmorStats>().HealToArmorHealPercentage;

            if(remaining > 0f) {
                foreach(var armor in ArmorFramework.ArmorHandlers[player].ActiveArmors) {
                    if(armor.HasArmorTag("NoRestorationRegen")) continue;
                    if(Time.time < armor.LastStateChangeTime + 5f || armor.Disable) continue;

                    float applied = armor.HealArmor(remaining);
                    remaining -= applied;

                    if(remaining <= 0f) break;
                }
            }
        }
    }
}
