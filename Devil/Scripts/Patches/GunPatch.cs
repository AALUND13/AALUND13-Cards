using HarmonyLib;
using System;
using System.Collections.Generic;

namespace AALUND13Cards.Devil.Patches {
    [HarmonyPatch(typeof(Gun))]
    public class GunPatch {
        private static Dictionary<Player, Action> registeredShootActions = new Dictionary<Player, Action>();

        public static void RegisterShootAction(Player player, Action onShoot) {
            if(registeredShootActions.TryGetValue(player, out Action action)) {
                action += onShoot;
            } else {
                registeredShootActions.Add(player, onShoot);
            }
        }

        public static void DeregisterShootAction(Player player, Action onShoot) {
            if(registeredShootActions.TryGetValue(player, out Action action)) {
                action -= onShoot;
            }
        }


        [HarmonyPatch("DoAttack")]
        [HarmonyPostfix]
        public static void TriggerShootAction(Gun __instance) {
            if(registeredShootActions.TryGetValue(__instance.player, out var action)) {
                action?.Invoke();
            }
        }
    }
}
