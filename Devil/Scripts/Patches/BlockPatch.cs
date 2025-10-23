using AALUND13Cards.Core;
using AALUND13Cards.Core.Extensions;
using AALUND13Cards.Devil.Cards;
using HarmonyLib;

namespace AALUND13Cards.Devil.Patches {
    [HarmonyPatch(typeof(Block))]
    internal class BlockPatch {
        [HarmonyPostfix]
        [HarmonyPriority(Priority.Last)]
        [HarmonyPatch(nameof(Block.IsBlocking))]
        public static void DisableBlockTime(Block __instance, ref bool __result, CharacterData ___data) {
            if(___data.GetCustomStatsRegistry().GetOrCreate<DevilCardsStats>().DisbaleBlockTime) {
                __result = false;
            }
        }

        [HarmonyPrefix]
        [HarmonyPatch(nameof(Block.Cooldown))]
        public static bool FixedBlockCooldown(Block __instance, ref float __result, CharacterData ___data) {
            if(___data != null && ___data.GetCustomStatsRegistry().GetOrCreate<DevilCardsStats>().FixedBlockCooldown != 0) {
                __result = ___data.GetCustomStatsRegistry().GetOrCreate<DevilCardsStats>().FixedBlockCooldown;
                return false;
            }
            return true;
        }
    }
}
