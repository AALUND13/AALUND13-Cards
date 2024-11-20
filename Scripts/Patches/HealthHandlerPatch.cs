using AALUND13Card.Extensions;
using HarmonyLib;
using UnboundLib;
using UnityEngine;

namespace AALUND13Card.Patches {
    [HarmonyPatch(typeof(HealthHandler))]
    public class HealthHandlerPatch {
        [HarmonyPatch("DoDamage")]
        [HarmonyPrefix]
        [HarmonyBefore("com.aalund13.rounds.jarl")]
        public static void DoDamage(HealthHandler __instance, ref Vector2 damage, Vector2 position, Color blinkColor, GameObject damagingWeapon, Player damagingPlayer, bool healthRemoval, bool lethal, bool ignoreBlock) {
            CharacterData data = (CharacterData)Traverse.Create(__instance).Field("data").GetValue();
            if(data.GetAdditionalData().secondToDealDamage > 0 && !data.GetAdditionalData().dealDamage) {
                Vector2 damageCopy = new Vector2(damage.x, damage.y);
                AALUND13_Cards.Instance.ExecuteAfterSeconds(data.GetAdditionalData().secondToDealDamage, () => {
                    data.GetAdditionalData().dealDamage = true;
                    __instance.DoDamage(damageCopy, position, blinkColor, damagingWeapon, damagingPlayer, healthRemoval, lethal, ignoreBlock);
                });

                damage = Vector2.zero;
            } else if(data.GetAdditionalData().dealDamage) {
                data.GetAdditionalData().dealDamage = false;
            }
        }
    }
}