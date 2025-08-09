using AALUND13Cards.Extensions;
using AALUND13Cards.Handlers;
using HarmonyLib;
using UnboundLib;
using UnityEngine;

namespace AALUND13Cards.Patches {
    [HarmonyPatch(typeof(HealthHandler))]
    public class HealthHandlerPatch {
        public static bool TakeDamageRunning = false;

        [HarmonyPatch(nameof(HealthHandler.TakeDamage), typeof(Vector2), typeof(Vector2), typeof(Color), typeof(GameObject), typeof(Player), typeof(bool), typeof(bool))]
        [HarmonyPrefix]
        public static void TakeDamagePrefix(HealthHandler __instance, Vector2 damage) {
            TakeDamageRunning = true;
            CharacterData data = (CharacterData)Traverse.Create(__instance).Field("data").GetValue();

            DamageEventHandler.TriggerDamageEvent(DamageEventHandler.DamageEventType.OnTakeDamage, data.player, damage);
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

            if(characterAdditionalData.DamageReduction > 0) {
                damage = new Vector2(damage.x * (1 - characterAdditionalData.DamageReduction), damage.y * (1 - characterAdditionalData.DamageReduction));
            }

            Vector2 damageCopy = damage;
            DamageEventHandler.TriggerDamageEvent(DamageEventHandler.DamageEventType.OnDoDamage, data.player, damageCopy);
            if(characterAdditionalData.secondToDealDamage > 0 && !characterAdditionalData.dealDamage) {
                Vector2 delayedDamage = new Vector2(damage.x, damage.y);
                __instance.gameObject.GetOrAddComponent<DelayDamageHandler>().DelayDamage(delayedDamage, position, blinkColor, damagingWeapon, damagingPlayer, healthRemoval, lethal, ignoreBlock);

                damage = Vector2.zero;
            } else if(characterAdditionalData.dealDamage) {
                characterAdditionalData.dealDamage = false;
            }
        }
    }
}