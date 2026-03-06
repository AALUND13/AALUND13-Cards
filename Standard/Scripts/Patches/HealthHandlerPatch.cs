using AALUND13Cards.Core.Extensions;
using AALUND13Cards.Core.Handlers;
using AALUND13Cards.Standard.Cards;
using HarmonyLib;
using UnboundLib;
using UnityEngine;

namespace AALUND13Cards.Standard.Patches {
    [HarmonyPatch(typeof(HealthHandler))]
    internal class HealthHandlerPatch {
        [HarmonyPatch(nameof(HealthHandler.DoDamage))]
        [HarmonyPrefix]
        public static void DoDamagePrefix(HealthHandler __instance, ref Vector2 damage, Vector2 position, Color blinkColor, GameObject damagingWeapon, Player damagingPlayer, bool healthRemoval, bool lethal, bool ignoreBlock) {
            CharacterData data = (CharacterData)Traverse.Create(__instance).Field("data").GetValue();
            var characterAdditionalData = data.GetCustomStatsRegistry().GetOrCreate<StandardStats>();

            if(characterAdditionalData.DamageReduction != 0) {
                damage = new Vector2(damage.x * (1 - characterAdditionalData.DamageReduction), damage.y * (1 - characterAdditionalData.DamageReduction));
            }

            if(characterAdditionalData.secondToDealDamage > 0 && !characterAdditionalData.dealDamage) {
                Vector2 delayedDamage = new Vector2(damage.x, damage.y);
                __instance.gameObject.GetOrAddComponent<DelayDamageHandler>().DelayDamage(new DelayDamageInfo(delayedDamage, position, blinkColor, damagingWeapon, damagingPlayer, healthRemoval, lethal, ignoreBlock), 
                    characterAdditionalData.secondToDealDamage,
                    () => { characterAdditionalData.dealDamage = true; });

                damage = Vector2.zero;
            } else if(characterAdditionalData.dealDamage) {
                characterAdditionalData.dealDamage = false;
            }
        }


        [HarmonyPatch(nameof(HealthHandler.Revive))]
        [HarmonyPrefix]
        public static void RevivePrefix(HealthHandler __instance) {
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