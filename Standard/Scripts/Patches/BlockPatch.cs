using AALUND13Cards.Core.Extensions;
using AALUND13Cards.Standard.Cards;
using HarmonyLib;
using System.Collections.Generic;
using UnboundLib;
using UnityEngine;

namespace AALUND13Cards.Standard.Patches {
    [HarmonyPatch(typeof(Block))]
    internal class BlockPatch {
        public static Dictionary<Block, bool> BlockRechargeAlreadyTriggered = new Dictionary<Block, bool>();

        [HarmonyPatch("IDoBlock")]
        [HarmonyPrefix]
        public static void IDoBlockPrefix(Block __instance, bool firstBlock, bool dontSetCD, BlockTrigger.BlockTriggerType triggerType) {
            if(!firstBlock || dontSetCD || triggerType != BlockTrigger.BlockTriggerType.Default)
                return;

            BlockRechargeAlreadyTriggered[__instance] = false;
        }

        [HarmonyPatch("Start")]
        [HarmonyPostfix]
        public static void StartPostfix(Block __instance) {
            CharacterData data = (CharacterData)__instance.GetFieldValue("data");
            __instance.BlockRechargeAction = () => {
                if(BlockRechargeAlreadyTriggered.TryGetValue(__instance, out bool alreadyTriggered) && alreadyTriggered)
                    return;

                for(int i = 0; i < data.GetCustomStatsRegistry().GetOrCreate<StandardStats>().BlocksWhenRecharge; i++) {
                    float timeBetweenBlocks = (float)__instance.GetFieldValue("timeBetweenBlocks");
                    float delay = i * timeBetweenBlocks;

                    __instance.StartCoroutine("DelayBlock", delay);
                }

                BlockRechargeAlreadyTriggered[__instance] = true;
            };
        }
        
        [HarmonyPatch("blocked")]
        [HarmonyPrefix]
        public static void BlockedPrefix(Block __instance, GameObject projectile, Vector3 forward, Vector3 hitPos) {
            ProjectileHit projectileHit = projectile.GetComponent<ProjectileHit>();
            HealthHandler healthHandler = (HealthHandler)__instance.GetFieldValue("health");
            CharacterData projectileOwner = projectileHit.ownPlayer.data;
            CharacterData data = (CharacterData)__instance.GetFieldValue("data");

            bool shouldDestroy = false;

            if(projectileOwner != null && projectileOwner.GetCustomStatsRegistry().GetOrCreate<StandardStats>().StunBlockTime > 0f) {
                data.block.StopAllCoroutines();
                data.block.sinceBlock = float.PositiveInfinity;

                data.stunHandler.AddStun(data.stunTime + projectileOwner.GetCustomStatsRegistry().GetOrCreate<StandardStats>().StunBlockTime);
                shouldDestroy = true;
            }

            if(shouldDestroy) GameObject.Destroy(projectile);
        }
    }
}
