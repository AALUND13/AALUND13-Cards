using AALUND13Cards.Core.Cards;
using HarmonyLib;

namespace AALUND13Cards.Core.Patches {
    [HarmonyPatch(typeof(ApplyCardStats), "ApplyStats")]
    internal class ApplyCardStatsPatch {
        public static void Postfix(ApplyCardStats __instance, Player ___playerToUpgrade) {
            CustomStatModifers[] customStatModifers = __instance.gameObject.GetComponents<CustomStatModifers>();
            foreach (var statModifers in customStatModifers) {
                statModifers.Apply(___playerToUpgrade);
            }
        }
    }
}
