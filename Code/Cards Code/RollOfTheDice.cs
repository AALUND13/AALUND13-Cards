using UnboundLib;
using UnityEngine;
using Photon.Pun;

public class RollOfTheDice : AACustomCard
{
    public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats, CardInfo card)
    {
        if (PhotonNetwork.OfflineMode || PhotonNetwork.IsMasterClient)
        {
            CardInfo randomCard = CardResgester.GetCardsFormString(card.GetComponent<AAStatsModifiers>().randomCardsToChoseFrom).GetRandom<GameObject>().GetComponent<CardInfo>();

            ModdingUtils.Utils.Cards.instance.AddCardToPlayer(player, randomCard, true, "", 2f, 2f, true);
            ModdingUtils.Utils.CardBarUtils.instance.ShowAtEndOfPhase(player, randomCard);
        }
    }   
}
