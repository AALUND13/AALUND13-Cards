using AALUND13Cards.Extensions;
using HarmonyLib;
using System.Collections.Generic;
using UnboundLib;

namespace AALUND13Cards.Patches {
    [HarmonyPatch(typeof(Block))]
    public class BlockPatch {
        public static Dictionary<Block, bool> BlockRechargeAlreadyTriggered = new Dictionary<Block, bool>();

        [HarmonyPatch("IDoBlock")]
        public static void Prefix(Block __instance, bool firstBlock, bool dontSetCD, BlockTrigger.BlockTriggerType triggerType) {
            if(!firstBlock || dontSetCD || triggerType != BlockTrigger.BlockTriggerType.Default)
                return;

            BlockRechargeAlreadyTriggered[__instance] = false;
        }

        [HarmonyPatch("Start")]
        public static void Postfix(Block __instance) {
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
    }
}
