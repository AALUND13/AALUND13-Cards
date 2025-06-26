using AALUND13Cards.Extensions;
using HarmonyLib;
using UnboundLib;

namespace AALUND13Cards.Patches {
    [HarmonyPatch(typeof(ResetBlock), "Go")]
    public class ResetBlockPatch {
        public static void Postfix(Block __instance) {
            CharacterData data = (CharacterData)__instance.GetFieldValue("data");
            if(BlockPatch.BlockRechargeAlreadyTriggered.TryGetValue(__instance, out bool alreadyTriggered) && alreadyTriggered)
                return;

            for(int i = 0; i < data.GetAdditionalData().BlocksWhenRecharge; i++) {
                float timeBetweenBlocks = (float)__instance.GetFieldValue("timeBetweenBlocks");
                float delay = i * timeBetweenBlocks;

                __instance.StartCoroutine("DelayBlock", delay);
            }

            BlockPatch.BlockRechargeAlreadyTriggered[__instance] = false;
        }

    }
}
