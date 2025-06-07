using AALUND13Cards.Cards;
using HarmonyLib;

namespace AALUND13Cards.Patches {
    [HarmonyPatch(typeof(ApplyCardStats), "ApplyStats")]
    public class ApplyCardStatsPatch {
        public static void Postfix(ApplyCardStats __instance, Player ___playerToUpgrade) {
            AAStatModifers aaStatModifers = __instance.gameObject.GetComponent<AAStatModifers>();
            if(aaStatModifers != null) {
                aaStatModifers.Apply(___playerToUpgrade);
            }
        }
    }
}
