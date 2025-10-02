using AALUND13Cards.Core.Extensions;
using AALUND13Cards.Standard.Cards;
using HarmonyLib;
using UnboundLib;

namespace AALUND13Cards.Standard.Patches {
    [HarmonyPatch(typeof(ResetBlock), "Go")]
    internal class ResetBlockPatch {
        public static void Postfix(Block __instance) {
            CharacterData data = (CharacterData)__instance.GetFieldValue("data");
            if(BlockPatch.BlockRechargeAlreadyTriggered.TryGetValue(__instance, out bool alreadyTriggered) && alreadyTriggered)
                return;

            for(int i = 0; i < data.GetCustomStatsRegistry().GetOrCreate<StandardStats>().BlocksWhenRecharge; i++) {
                float timeBetweenBlocks = (float)__instance.GetFieldValue("timeBetweenBlocks");
                float delay = i * timeBetweenBlocks;

                __instance.StartCoroutine("DelayBlock", delay);
            }

            BlockPatch.BlockRechargeAlreadyTriggered[__instance] = false;
        }

    }
}
