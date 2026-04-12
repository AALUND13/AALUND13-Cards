using AALUND13Cards.Core.Extensions;
using AALUND13Cards.Core.Handlers;
using AALUND13Cards.Standard.Cards;
using HarmonyLib;
using Photon.Realtime;
using UnboundLib;
using UnityEngine;

namespace AALUND13Cards.Standard.Patches {
    [HarmonyPatch(typeof(HealthHandler))]
    internal class HealthHandlerPatch {
        [HarmonyPatch(nameof(HealthHandler.DoDamage))]
        [HarmonyPrefix]
        public static void DoDamagePrefix(HealthHandler __instance, ref Vector2 damage, Vector2 position, Color blinkColor, GameObject damagingWeapon, Player damagingPlayer, bool healthRemoval, ref bool lethal, bool ignoreBlock) {
            CharacterData data = (CharacterData)Traverse.Create(__instance).Field("data").GetValue();
            var stats = data.GetCustomStatsRegistry().GetOrCreate<StandardStats>();

            if(stats.DamageReduction != 0) {
                damage = new Vector2(damage.x * (1 - stats.DamageReduction), damage.y * (1 - stats.DamageReduction));
            }

            if(stats.secondToDealDamage > 0 && !stats.dealDamage) {
                Vector2 delayedDamage = new Vector2(damage.x, damage.y);
                __instance.gameObject.GetOrAddComponent<DelayDamageHandler>().DelayDamage(new DelayDamageInfo(delayedDamage, position, blinkColor, damagingWeapon, damagingPlayer, healthRemoval, lethal, ignoreBlock), 
                    stats.secondToDealDamage,
                    () => { stats.dealDamage = true; });

                damage = Vector2.zero;
            } else if(stats.dealDamage) {
                stats.dealDamage = false;
            }
        }


        [HarmonyPatch(nameof(HealthHandler.Revive))]
        [HarmonyPrefix]
        public static void RevivePrefix(HealthHandler __instance, bool isFullRevive) {
            CharacterData data = (CharacterData)Traverse.Create(__instance).Field("data").GetValue();
            var characterAdditionalData = data.GetCustomStatsRegistry().GetOrCreate<StandardStats>();

            if(characterAdditionalData.FrozenTime > 0) {
                CharacterStatModifiers characterStatModifiers = __instance.GetComponent<CharacterStatModifiers>();

                characterAdditionalData.DamageReduction -= NegativeSaturateCurve(characterAdditionalData.OldFrozenTime, 10);
                characterStatModifiers.slowSlow -= characterAdditionalData.OldFrozenTime;
                characterAdditionalData.FrozenTime = 0;
                characterAdditionalData.OldFrozenTime = 0;
            }
        }

        [HarmonyPatch("Update")]
        [HarmonyPrefix]
        public static void UpdatePrefix(HealthHandler __instance) {
            CharacterData data = (CharacterData)Traverse.Create(__instance).Field("data").GetValue();
            var characterAdditionalData = data.GetCustomStatsRegistry().GetOrCreate<StandardStats>();

            if(characterAdditionalData.FrozenTime != characterAdditionalData.OldFrozenTime) {
                CharacterStatModifiers characterStatModifiers = __instance.GetComponent<CharacterStatModifiers>();
                characterStatModifiers.InvokeMethod("DoSlowDown", characterAdditionalData.FrozenTime);
            }
            
            if(characterAdditionalData.FrozenTime > 0) {
                CharacterStatModifiers characterStatModifiers = __instance.GetComponent<CharacterStatModifiers>();

                characterAdditionalData.DamageReduction -= NegativeSaturateCurve(characterAdditionalData.OldFrozenTime, 10);
                characterStatModifiers.slowSlow -= characterAdditionalData.OldFrozenTime;
                characterAdditionalData.FrozenTime = Mathf.Max(characterAdditionalData.FrozenTime - Time.deltaTime, 0);
                
                characterAdditionalData.DamageReduction += NegativeSaturateCurve(characterAdditionalData.FrozenTime, 10);
                characterAdditionalData.OldFrozenTime = characterAdditionalData.FrozenTime;
                characterStatModifiers.slowSlow += characterAdditionalData.FrozenTime;
            }
        }

        public static float NegativeSaturateCurve(float x, float k) {
            float x2 = x * x;
            return -(x2 / (x2 + k));
        }
    }
}