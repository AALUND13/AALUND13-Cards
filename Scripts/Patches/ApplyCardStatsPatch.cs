using Assets.Mods._AALUND13_Card.Scripts.Cards;
using HarmonyLib;

namespace Assets.Mods._AALUND13_Card.Scripts.Patches {
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
