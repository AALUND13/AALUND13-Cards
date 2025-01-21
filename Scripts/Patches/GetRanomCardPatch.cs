using AALUND13Card.Extensions;
using AALUND13Card.RandomStatGenerators.Generators ;
using HarmonyLib;
using ModdingUtils.Patches;
using System;

namespace AALUND13Card.Patches {
    [HarmonyPatch(typeof(CardChoicePatchGetRanomCard), "OrignialGetRanomCard", typeof(CardInfo[]))]
    internal class GetRanomCardPatch {
        public static Random random = new Random();

        private static void Prefix(ref CardInfo[] cards) {
            Player pickingPlayer = PickingPlayer(CardChoice.instance);
            if(pickingPlayer != null) {
                bool spawnGlitchCard = random.NextFloat(0f, 1f) < pickingPlayer.data.GetAdditionalData().GlitchedCardSpawnChance;
                if(spawnGlitchCard) {
                    cards = GlitchedStatGenerator.GlitchedCards.ToArray();
                }
            }
        }

        internal static Player PickingPlayer(CardChoice cardChoice) {
            return typeof(CardChoicePatchGetRanomCard).GetMethod("PickingPlayer", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static).Invoke(null, new object[] { cardChoice }) as Player;
        }
    }
}
