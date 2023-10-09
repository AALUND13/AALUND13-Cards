using HarmonyLib;

[HarmonyPatch(typeof(ApplyCardStats), "ApplyStats")]
class ApplyCardStatsPatch
{
    static void Postfix(ApplyCardStats __instance, Player ___playerToUpgrade)
    {
        var player = ___playerToUpgrade.GetComponent<Player>();
        var gun = ___playerToUpgrade.GetComponent<Holding>().holdable.GetComponent<Gun>();
        var characterData = ___playerToUpgrade.GetComponent<CharacterData>();
        var healthHandler = ___playerToUpgrade.GetComponent<HealthHandler>();
        var gravity = ___playerToUpgrade.GetComponent<Gravity>();
        var block = ___playerToUpgrade.GetComponent<Block>();
        var gunAmmo = gun.GetComponentInChildren<GunAmmo>();
        var characterStatModifiers = player.GetComponent<CharacterStatModifiers>();

        AACustomCard customAbility = __instance.gameObject.GetComponent<AACustomCard>();
        if (customAbility != null)
        {
            customAbility.OnAddCard(player, gun, gunAmmo, characterData, healthHandler, gravity, block, characterStatModifiers, __instance.GetComponent<CardInfo>());
        }
    }
}