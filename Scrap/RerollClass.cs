using Photon.Pun;
using ModdingUtils.Extensions;
public class RerollClass : AACustomCard
{
    public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers, Block block)
    {
        cardInfo.GetAdditionalData().canBeReassigned = false;
    }


    public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
    {
        if (PhotonNetwork.OfflineMode || PhotonNetwork.IsMasterClient)
        {
            RerollClassManager.instance.AddPlayerToRerollClassPlayer(player);
        }
    }
}

