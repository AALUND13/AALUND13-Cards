using HarmonyLib;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnboundLib;
using UnityEngine;
using CardChoiceSpawnUniqueCardPatch.CustomCategories;
using ClassesManagerReborn;

public class RollOfTheDice : AAStatsModifiers
{
    public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
    {
        StartCoroutine(giveRandomCard(player));
    }

    IEnumerator giveRandomCard(Player player)
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
        
        yield return null;
        ModdingUtils.Utils.Cards.instance.AddCardToPlayer(player, diceResultCardToPickForm.GetRandom<GameObject>().GetComponent<CardInfo>(), false, "", 2f, 2f);
        AALUND13_Cards.cardToShow.Add(player.teamID, player.data.currentCards.Last());
    }
}
