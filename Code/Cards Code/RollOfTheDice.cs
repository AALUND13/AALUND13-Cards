using HarmonyLib;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnboundLib;
using UnityEngine;
using CardChoiceSpawnUniqueCardPatch.CustomCategories;
using ClassesManagerReborn;
using ModdingUtils;
using Photon.Pun;

public class RollOfTheDice : AAStatsModifiers
{
    public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats, CardInfo card)
    {
        
        StartCoroutine(giveRandomCard(player));
    }

    IEnumerator giveRandomCard(Player player)
    {
        if (PhotonNetwork.OfflineMode || PhotonNetwork.IsMasterClient)
        {
            List<GameObject> diceResultCardToPickForm = new List<GameObject>
            {
                CardResgester.ModCards["Misfortune Strikes"],
                CardResgester.ModCards["Struggle for Survival"],
                CardResgester.ModCards["Steady Gains"],
                CardResgester.ModCards["Fortunate Turn"],
                CardResgester.ModCards["Excellent Fortune"],
                CardResgester.ModCards["Unprecedented Luck"]
            };
            CardInfo randomCard = diceResultCardToPickForm.GetRandom<GameObject>().GetComponent<CardInfo>();

            ModdingUtils.Utils.Cards.instance.AddCardToPlayer(player, randomCard, true, "", 2f, 2f, true);
            ModdingUtils.Utils.CardBarUtils.instance.ShowAtEndOfPhase(player, randomCard);

        }
        yield break;
    }
        
}
