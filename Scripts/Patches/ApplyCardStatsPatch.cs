using AALUND13Cards.Cards;
using HarmonyLib;

namespace AALUND13Cards.Patches {
    [HarmonyPatch(typeof(ApplyCardStats), "ApplyStats")]
    public class ApplyCardStatsPatch {
        public static void Postfix(ApplyCardStats __instance, Player ___playerToUpgrade) {
            CustomStatModifers[] customStatModifers = __instance.gameObject.GetComponents<CustomStatModifers>();
            foreach (var statModifers in customStatModifers) {
                statModifers.Apply(___playerToUpgrade);
            }
        }
    }
}
