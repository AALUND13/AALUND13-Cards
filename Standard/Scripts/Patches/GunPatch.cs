using AALUND13Cards.Standard.Handler;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AALUND13Cards.Standard.Patches {
    [HarmonyPatch(typeof(Gun), nameof(Gun.BulletInit))]
    public class GunPatch {
        public static void Postfix(Gun __instance, GameObject bullet) {
            PlayerGunActions.InvokeShootAction(__instance.player, __instance, bullet);
        }
    }
}
