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
