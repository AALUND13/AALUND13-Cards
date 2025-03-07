using AALUND13Card.Cards;
using AALUND13Card.Extensions;
using AALUND13Card.Scripts;
using HarmonyLib;
using ModdingUtils.Patches;
using Photon.Pun;
using System.Linq;
using UnboundLib;
using UnityEngine;

namespace AALUND13Card.Patches {
    [HarmonyPatch(typeof(CardChoice), "SpawnUniqueCard")]
    [HarmonyAfter("com.Root.Null")]
    internal class SpawnUniqueCardPatch {
        [HarmonyPriority(Priority.Last)]
        private static void Postfix(int ___pickrID, ref GameObject __result) {
            Player player = PlayerManager.instance.players.Find(p => p.playerID == ___pickrID);
            CardInfo cardInfo = __result.GetComponent<CardInfo>();
            if(player == null) return;

            // Check thew object to see they have a Ccmponents name `NullCard` if so skip 
            if(cardInfo.GetComponents<MonoBehaviour>().Any(x => x.GetType().Name == "NullCard")) return;

            bool spawnGlitchCard = Random.Range(0f, 1f) < player.data.GetAdditionalData().CorruptedCardSpawnChance;
            if (!spawnGlitchCard) return;

            GameObject pickCard = CardChoicePatchGetRanomCard.OrignialGetRanomCard(CorruptedCardManager.Instance.CorruptedCardPrefabs);

            GameObject old = __result;
            Unbound.Instance.ExecuteAfterFrames(3, () => PhotonNetwork.Destroy(old));
    
            Vector2 randomStatRange = pickCard.GetComponent<CorruptedCard>().RandomStatRange;
            __result = PhotonNetwork.Instantiate(pickCard.name, __result.transform.transform.position, __result.transform.transform.rotation, 0, new object[] { __result.transform.localScale, Random.Range(0, 1000000), pickCard.GetComponent<CorruptedCard>().Rarity.ToString(), randomStatRange });
        }
    }
}
