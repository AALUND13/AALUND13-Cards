using AALUND13Cards.Core.Extensions;
using AALUND13Cards.Core.Handlers;
using HarmonyLib;
using UnboundLib;
using UnityEngine;

namespace AALUND13Cards.Core.Patches {
    [HarmonyPatch(typeof(HealthHandler))]
    internal class HealthHandlerPatch {
        public static bool TakeDamageRunning = false;

        [HarmonyPatch(nameof(HealthHandler.Revive))]
        [HarmonyPostfix]
        public static void RevivePrefix(HealthHandler __instance, CharacterData ___data) {
            ConstantDamageHandler.Instance.RemovePlayerFromAll(___data.player);
            DelayDamageHandler.Instance.StopAllCoroutines();
        }

        [HarmonyPatch(nameof(HealthHandler.TakeDamage), typeof(Vector2), typeof(Vector2), typeof(Color), typeof(GameObject), typeof(Player), typeof(bool), typeof(bool))]
        [HarmonyPrefix]
        public static void TakeDamagePrefix(HealthHandler __instance, Player damagingPlayer, Vector2 damage, bool lethal) {
            TakeDamageRunning = true;
            CharacterData data = (CharacterData)Traverse.Create(__instance).Field("data").GetValue();

            DamageEventHandler.TriggerDamageEvent(DamageEventHandler.DamageEventType.OnTakeDamage, data.player, damagingPlayer, damage, lethal);
        }
        [HarmonyPatch(nameof(HealthHandler.TakeDamage), typeof(Vector2), typeof(Vector2), typeof(Color), typeof(GameObject), typeof(Player), typeof(bool), typeof(bool))]
        [HarmonyPostfix]
        public static void TakeDamagePostfix(HealthHandler __instance, Vector2 damage) {
            TakeDamageRunning = false;
        }

        [HarmonyPatch(nameof(HealthHandler.DoDamage))]
        [HarmonyPrefix]
        public static void DoDamage(HealthHandler __instance, ref Vector2 damage, Vector2 position, Color blinkColor, GameObject damagingWeapon, Player damagingPlayer, bool healthRemoval, bool lethal, bool ignoreBlock) {
            CharacterData data = (CharacterData)Traverse.Create(__instance).Field("data").GetValue();
            var characterAdditionalData = data.GetAdditionalData();

            Vector2 damageCopy = damage;
            DamageEventHandler.TriggerDamageEvent(DamageEventHandler.DamageEventType.OnDoDamage, data.player, damagingPlayer, damageCopy, lethal);
        }
    }
}