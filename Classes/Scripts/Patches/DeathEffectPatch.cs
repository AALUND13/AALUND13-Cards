using AALUND13Cards.Classes.MonoBehaviours;
using AALUND13Cards.Core;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AALUND13Cards.Classes.Patches {
    [HarmonyPatch(typeof(DeathEffect))]
    public class DeathEffectPatch {
        public static Dictionary<DeathEffect, Player> RespawnIds = new Dictionary<DeathEffect, Player>();

        [HarmonyPatch(nameof(DeathEffect.PlayDeath))]
        [HarmonyPostfix]
        public static void PlayDeathPostfix(DeathEffect __instance, int playerIDToRevive) {
            foreach(var customDeathEffect in __instance.GetComponents<ICustomDeathEffect>()) {
                RespawnIds.Add(__instance, PlayerManager.instance.players.First(p => p.playerID == playerIDToRevive));
                customDeathEffect.OnDeath(__instance, RespawnIds[__instance]);
            }
        }

        [HarmonyPatch("RespawnPlayer", MethodType.Enumerator)]
        [HarmonyPostfix]
        public static void RespawnPlayerPostfix(DeathEffect __instance) {
            try {
                foreach(var customDeathEffect in __instance.GetComponents<ICustomDeathRespawnEffect>()) {
                    if(RespawnIds.TryGetValue(__instance, out var player)) {
                        customDeathEffect.OnRespawn(__instance, player);
                    }
                }
            } catch(Exception e) {
                LoggerUtils.LogError(e.ToString());
            }
        }
    }
}
