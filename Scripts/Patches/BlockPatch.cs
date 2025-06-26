using AALUND13Cards.Extensions;
using HarmonyLib;
using System.Collections.Generic;
using UnboundLib;
using UnityEngine;

namespace AALUND13Cards.Patches {
    [HarmonyPatch(typeof(Block))]
    public class BlockPatch {
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

                for(int i = 0; i < data.GetAdditionalData().BlocksWhenRecharge; i++) {
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

            float blockPircePercent = projectileHit.ownPlayer.data.GetAdditionalData().BlockPircePercent;
            if(blockPircePercent > 0f) {
                Vector2 damage = (projectileHit.bulletCanDealDeamage ? projectileHit.damage : 1f) * blockPircePercent * forward.normalized;
                healthHandler.TakeDamage(damage, hitPos, projectileHit.projectileColor, projectileHit.ownWeapon, projectileHit.ownPlayer, true, true);

                GameObject.Destroy(projectile);
            }
        }

    }
}
