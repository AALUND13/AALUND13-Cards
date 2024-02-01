using AALUND13Card.Extensions;
using AALUND13Card.Handler;
using HarmonyLib;
using Photon.Realtime;
using System.Collections.Generic;
using System.Linq;
using UnboundLib;
using UnityEngine;
using static AALUND13Card.Classes;

namespace AALUND13Card.Patchs
{
    [HarmonyPatch(typeof(CharacterStatModifiers))]
    public class CharacterStatModifiersPatch
    {
        [HarmonyPatch("ResetStats")]
        [HarmonyPrefix]
        public static void ResetStats(CharacterStatModifiers __instance)
        {
            CharacterData data = (CharacterData)Traverse.Create(__instance).Field("data").GetValue();
            data.GetAdditionalData().secondToDealDamage = 0;
            data.GetAdditionalData().DamageDealSecond = new List<DamageDealSecond>();
            data.GetAdditionalData().dealDamage = true;
        }
    }

}