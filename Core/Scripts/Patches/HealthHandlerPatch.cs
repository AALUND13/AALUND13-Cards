using AALUND13Cards.Core.Extensions;
using AALUND13Cards.Core.Handlers;
using HarmonyLib;
using System;
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
        public static void TakeDamagePrefix(HealthHandler __instance, ref Player damagingPlayer, ref Vector2 damage, ref bool lethal) {
            TakeDamageRunning = true;
            CharacterData data = (CharacterData)Traverse.Create(__instance).Field("data").GetValue();

            DamageInfo damageInfo = DamageEventHandler.TriggerDamageEvent(DamageEventHandler.DamageEventType.OnTakeDamage, data.player, damagingPlayer, damage, lethal);

            damage = damageInfo.Damage;
            lethal = damageInfo.IsLethal;
            damagingPlayer = damageInfo.DamagingPlayer;
        }
        [HarmonyPatch(nameof(HealthHandler.TakeDamage), typeof(Vector2), typeof(Vector2), typeof(Color), typeof(GameObject), typeof(Player), typeof(bool), typeof(bool))]
        [HarmonyPostfix]
        public static void TakeDamagePostfix(HealthHandler __instance, Vector2 damage) {
            TakeDamageRunning = false;
        }

        [HarmonyPatch(nameof(HealthHandler.DoDamage))]
        [HarmonyPrefix]
        public static void DoDamage(HealthHandler __instance, ref Vector2 damage, Vector2 position, Color blinkColor, GameObject damagingWeapon, ref Player damagingPlayer, bool healthRemoval, ref bool lethal, bool ignoreBlock) {
            CharacterData data = (CharacterData)Traverse.Create(__instance).Field("data").GetValue();
            var characterAdditionalData = data.GetAdditionalData();

            DamageInfo damageInfo = DamageEventHandler.TriggerDamageEvent(DamageEventHandler.DamageEventType.OnDoDamage, data.player, damagingPlayer, damage, lethal);

            damage = damageInfo.Damage;
            lethal = damageInfo.IsLethal;
            damagingPlayer = damageInfo.DamagingPlayer;
        }

        [HarmonyPatch("RPCA_Die_Phoenix")]
        [HarmonyPrefix]
        public static void PhoenixRevive(Player ___player, bool ___isRespawning, Vector2 deathDirection) {
            if(___isRespawning || ___player.data.dead) return;

            if(DeathActionHandler.Instance.registeredReviveActions.TryGetValue(___player, out Action onRevive)) {
                onRevive?.Invoke();
            }
        }

        [HarmonyPatch("RPCA_Die")]
        [HarmonyPrefix]
        public static void TrueDeath(Player ___player, bool ___isRespawning, Vector2 deathDirection) {
            if(___isRespawning || ___player.data.dead) return;

            if(DeathActionHandler.Instance.registeredTrueDeathActions.TryGetValue(___player, out Action onTrueDeath)) {
                onTrueDeath?.Invoke();
            }
        }
    }
}