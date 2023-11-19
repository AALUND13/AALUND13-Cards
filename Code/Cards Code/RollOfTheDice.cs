using UnboundLib;
using UnityEngine;
using Photon.Pun;
using ModdingUtils.Extensions;
using ModsPlus;

public class RollOfTheDice : AACustomCard
{
    CardInfo CardInfo { get; set; }

    public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers, Block block)
    {
        CardInfo = cardInfo;
        cardInfo.GetAdditionalData().canBeReassigned = false;
        
    }

    public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
    {
        if (PhotonNetwork.OfflineMode || PhotonNetwork.IsMasterClient)
        {
            CardInfo randomCard = CardResgester.GetCardsFormString(CardInfo.GetComponent<AAStatsModifiers>().randomCardsToChoseFrom).GetRandom<GameObject>().GetComponent<CardInfo>();

            ModdingUtils.Utils.Cards.instance.AddCardToPlayer(player, randomCard, true, "", 2f, 2f, true);
            ModdingUtils.Utils.CardBarUtils.instance.ShowAtEndOfPhase(player, randomCard);
        }
    }   
}
