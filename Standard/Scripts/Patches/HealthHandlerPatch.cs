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
        public static void DoDamage(HealthHandler __instance, ref Vector2 damage, Vector2 position, Color blinkColor, GameObject damagingWeapon, Player damagingPlayer, bool healthRemoval, bool lethal, bool ignoreBlock) {
            CharacterData data = (CharacterData)Traverse.Create(__instance).Field("data").GetValue();
            var characterAdditionalData = data.GetCustomStatsRegistry().GetOrCreate<StandardStats>();

            if(characterAdditionalData.DamageReduction > 0) {
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
    }
}