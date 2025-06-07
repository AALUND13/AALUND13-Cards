using AALUND13Cards.Extensions;
using AALUND13Cards.Handlers;
using HarmonyLib;
using UnboundLib;
using UnityEngine;

namespace AALUND13Cards.Patches {
    [HarmonyPatch(typeof(HealthHandler))]
    public class HealthHandlerPatch {
        [HarmonyPatch("DoDamage")]
        [HarmonyPrefix]
        public static void DoDamage(HealthHandler __instance, ref Vector2 damage, Vector2 position, Color blinkColor, GameObject damagingWeapon, Player damagingPlayer, bool healthRemoval, bool lethal, bool ignoreBlock) {
            CharacterData data = (CharacterData)Traverse.Create(__instance).Field("data").GetValue();
            var characterAdditionalData = data.GetAdditionalData();

            

            if(characterAdditionalData.secondToDealDamage > 0 && !characterAdditionalData.dealDamage) {
                Vector2 damageCopy = new Vector2(damage.x, damage.y);
                __instance.gameObject.GetOrAddComponent<DelayDamageHandler>().DelayDamage(damageCopy, position, blinkColor, damagingWeapon, damagingPlayer, healthRemoval, lethal, ignoreBlock);

                damage = Vector2.zero;
            } else if(characterAdditionalData.dealDamage) {
                characterAdditionalData.dealDamage = false;
            }
        }
    }
}