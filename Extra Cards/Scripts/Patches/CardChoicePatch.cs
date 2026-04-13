using AALUND13Cards.Core.Extensions;
using AALUND13Cards.ExtraCards;
using AALUND13Cards.ExtraCards.Cards;
using CorruptedCardsManager;
using HarmonyLib;
using ModsPlus;
using Photon.Pun;
using RandomCardsGenerators.Cards;
using System;
using System.Collections.Generic;
using UnboundLib;
using UnboundLib.Networking;
using UnityEngine;
using WillsWackyManagers.Utils;

namespace AALUND13Cards.Core.Patches {
    [HarmonyPatch(typeof(CardChoice))]
    public class CardChoicePatch {
        [HarmonyPatch("IDoEndPick")]
        [HarmonyPostfix]
        private static void IDoEndPickPostfix(GameObject pickedCard, int theInt, int pickId) {
            var player = PlayerManager.instance.GetPlayerWithID(pickId);
            if(player == null) return;

            var extraCardsStats = player.data.GetCustomStatsRegistry().GetOrCreate<ExtraCardsStats>();

            if(extraCardsStats.DuplicatesAsCorrupted != 0 && (pickedCard.GetComponent<RandomCard>() == null || !pickedCard.GetComponent<RandomCard>().StatGenName.StartsWith("CCM_CorruptedCardsGenerator")) && (PhotonNetwork.IsMasterClient || PhotonNetwork.OfflineMode)) {
                CardInfo.Rarity rarity = pickedCard.GetComponent<CardInfo>().rarity;
                if(Enum.IsDefined(typeof(CorruptedCardRarity), rarity.ToString())) {
                    CorruptedCardRarity corruptedRarity = (CorruptedCardRarity)Enum.Parse(typeof(CorruptedCardRarity), rarity.ToString());
                    for(int i = 0; i < extraCardsStats.DuplicatesAsCorrupted; i++) {
                        CorruptedCardsManager.CorruptedCardsManager.CorruptedCardsGenerators.CreateRandomCard(corruptedRarity, player);
                    }
                }
            }
        }

        [HarmonyPatch("SpawnUniqueCard")]
        [HarmonyPostfix]
        private static void SpawnPostfix(CardChoice __instance, GameObject __result) {
            var spawnedCards = (List<GameObject>)CardChoice.instance.GetFieldValue("spawnedCards");
            var player = PlayerManager.instance.GetPlayerWithID(__instance.pickrID);
            if(player == null) return;

            if(
               spawnedCards.Count >= Math.Max(DrawNCards.DrawNCards.GetPickerDraws(__instance.pickrID) - player.data.GetCustomStatsRegistry().GetOrCreate<ExtraCardsStats>().CurseCardDraws, 0)
               || player.data.GetCustomStatsRegistry().GetOrCreate<ExtraCardsStats>().FullCurseDraws
            ) {
                AAC_Core.Instance.ExecuteAfterFrames(5, () => {
                    NetworkingManager.RPC(typeof(CardChoicePatch), nameof(SpawnCurseDraw), __result.GetComponent<PhotonView>().ViewID);
                });
            }
        }

        [HarmonyPatch("RPCA_DoEndPick")]
        [HarmonyPostfix]
        private static void RPCA_DoEndPickpOSTFIX(CardChoice __instance, int targetCardID, int theInt, int pickId) {
            var spawnedCards = (List<GameObject>)CardChoice.instance.GetFieldValue("spawnedCards");
            var player = PlayerManager.instance.GetPlayerWithID(pickId);

            if(PhotonNetwork.GetPhotonView(targetCardID).gameObject.GetComponent<CursedCard>() != null) {
                CurseManager.instance.CursePlayer(player);
            }
        }


        [UnboundRPC]
        private static void SpawnCurseDraw(int viewId) {
            try {
                PhotonView obj = PhotonNetwork.GetPhotonView(viewId);
                GameObject CurseCardDraw = GameObject.Instantiate(AAC_ExtraCards.CurseDrawObject, obj.transform.GetComponentInChildren<CardVisuals>().transform.GetChild(0));
                CurseCardDraw.transform.SetAsFirstSibling();
                obj.gameObject.AddComponent<CursedCard>();
            } catch(Exception e) {
                LoggerUtils.LogError(e.Message);
            }
        }
    }
}
