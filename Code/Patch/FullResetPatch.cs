using HarmonyLib;
using System;

[HarmonyPatch(typeof(Player), "FullReset")]
class FullResetPatch
{

    static void Postfix(Player __instance)
    {
        UnityEngine.Debug.Log(__instance.ToString() + "Stats reset");
        foreach (CardInfo currentCard in __instance.data.currentCards)
        {
            if (currentCard.GetComponent<AAStatsModifiers>() is AAStatsModifiers customCard)
            {
                try
                {
                    Gun gun = __instance.GetComponent<Holding>().holdable.GetComponent<Gun>();
                    CharacterData characterData = __instance.GetComponent<CharacterData>();
                    HealthHandler healthHandler = __instance.GetComponent<HealthHandler>();
                    Gravity gravity = __instance.GetComponent<Gravity>();
                    Block block = __instance.GetComponent<Block>();
                    GunAmmo gunAmmo = gun.GetComponentInChildren<GunAmmo>();
                    CharacterStatModifiers characterStatModifiers = __instance.GetComponent<CharacterStatModifiers>();
                    customCard.OnRemoveCard(__instance, gun, gunAmmo, characterData, healthHandler, gravity, block, characterStatModifiers, currentCard);
                }
                catch (NotImplementedException)
                { }
                catch (Exception exception)
                {
                    UnityEngine.Debug.LogException(exception);
                }
            }
        }
    }
}