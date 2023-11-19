using HarmonyLib;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[HarmonyPatch(typeof(HealthHandler), "DoDamage")]
public class HealthHandlerPatchDoDamage
{
    private static void Prefix(HealthHandler __instance, ref Vector2 damage, Vector2 position, Color blinkColor, GameObject damagingWeapon, Player damagingPlayer, bool healthRemoval, ref bool lethal, bool ignoreBlock)
    {
        CharacterData data = (CharacterData)Traverse.Create(__instance).Field("data").GetValue();

        if (data.GetComponentInChildren<SoulstreakObject>() != null)
        {
            SoulstreakObject soulstreakObject = data.GetComponentInChildren<SoulstreakObject>();

            if (soulstreakObject.soulShieldHealth > 0 && !soulstreakObject.soulShieldDepleted)
            {
                float damageMagnitude = damage.magnitude;

                if (soulstreakObject.soulShieldHealth - damageMagnitude > 0)
                {
                    // Shield can absorb the entire damage
                    soulstreakObject.soulShieldHealth -= damageMagnitude;
                    damage = Vector2.zero;
                }
                else
                {
                    // Shield absorbs part of the damage, some damage goes through
                    damageMagnitude -= soulstreakObject.soulShieldHealth;
                    soulstreakObject.soulShieldHealth = 0;

                    // Handle remaining damage using the direction (assuming damage is a Vector2)
                    // You might want to implement your logic for applying damage directionally
                    Vector2 remainingDamageDirection = damage.normalized * damageMagnitude;
                    damage = remainingDamageDirection;
                    // Apply the remaining damage directionally as needed
                    soulstreakObject.soulShieldDepleted = true;
                }
            }
        }

        if (!data.isPlaying)
            return;
        if (data.dead)
            return;
        if (__instance.isRespawning)
            return;
        if (data.block.IsBlocking() && !ignoreBlock)
            return;

        if (lethal && data.health - damage.magnitude < 0)
        {
            //UnityEngine.Debug.Log($"{data.player.playerID} Died to {((damagingPlayer.playerID != null) ? damagingPlayer.playerID : "Unknow")}");
            PlayerDied.PlayerDiedAction(data.player, damagingPlayer);
        }
    }
}
