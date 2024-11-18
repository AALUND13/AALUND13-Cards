using AALUND13Card.Extensions;
using AALUND13Card.Handler;
using HarmonyLib;
using System.Linq;
using UnboundLib;
using UnityEngine;

namespace AALUND13Card.Patchs {
    [HarmonyPatch(typeof(HealthHandler))]
    public class HealthHandlerPatch {
        [HarmonyPatch("DoDamage")]
        [HarmonyPrefix]
        [HarmonyBefore("com.aalund13.rounds.jarl")]
        public static void DoDamage(HealthHandler __instance, ref Vector2 damage, Vector2 position, Color blinkColor, GameObject damagingWeapon, Player damagingPlayer, bool healthRemoval, bool lethal, bool ignoreBlock) {
            CharacterData data = (CharacterData)Traverse.Create(__instance).Field("data").GetValue();
            data.gameObject.GetOrAddComponent<DeathHandler>().PlayerTakeDamage(damagingPlayer);

            if(data.GetAdditionalData().secondToDealDamage > 0 && !data.GetAdditionalData().dealDamage) {
                data.GetAdditionalData().DamageDealSecond.Add(new DamageDealSecond(Time.time + data.GetAdditionalData().secondToDealDamage,
                    damage, position, blinkColor, damagingWeapon, damagingPlayer, healthRemoval, lethal, ignoreBlock));

                damage = Vector2.zero;
            } else if(data.GetAdditionalData().dealDamage) {
                data.GetAdditionalData().dealDamage = false;
            }
        }

        [HarmonyPatch("RPCA_Die")]
        [HarmonyPrefix]
        public static void RPCA_Die(Player ___player, Vector2 deathDirection) {
            if(___player.data.dead) return;
            ___player.gameObject.GetOrAddComponent<DeathHandler>().PlayerDied();
            ___player.data.GetAdditionalData().DamageDealSecond.Clear();
        }

        [HarmonyPatch("RPCA_Die_Phoenix")]
        [HarmonyPrefix]
        public static void RPCA_Die_Phoenix(HealthHandler __instance, Player ___player, Vector2 deathDirection) {
            if((___player.data.dead || __instance.isRespawning)) return;
            ___player.gameObject.GetOrAddComponent<DeathHandler>().PlayerDied();
            ___player.data.GetAdditionalData().DamageDealSecond.Clear();
        }

        [HarmonyPatch("Update")]
        [HarmonyPrefix]
        public static void Update(HealthHandler __instance) {
            CharacterData data = (CharacterData)Traverse.Create(__instance).Field("data").GetValue();
            if(data.GetAdditionalData().DamageDealSecond.Count == 0) return;
            if(data.GetAdditionalData().DamageDealSecond.First().TimeToDealDamage < Time.time) {
                DamageDealSecond lastDamagingPlayer = data.GetAdditionalData().DamageDealSecond.First();
                data.GetAdditionalData().DamageDealSecond.RemoveAt(0);
                if(data.isPlaying) {
                    data.GetAdditionalData().dealDamage = true;
                    data.healthHandler.DoDamage(lastDamagingPlayer.Damage, lastDamagingPlayer.Position, lastDamagingPlayer.BlinkColor, lastDamagingPlayer.DamagingWeapon, lastDamagingPlayer.DamagingPlayer, lastDamagingPlayer.HealthRemoval, lastDamagingPlayer.Lethal, lastDamagingPlayer.IgnoreBlock);
                }
            }
        }

        [HarmonyPatch("Revive")]
        [HarmonyPostfix]
        public static void Revive(CharacterData ___data) {
            ___data.GetAdditionalData().DamageDealSecond.Clear();
        }
    }
}