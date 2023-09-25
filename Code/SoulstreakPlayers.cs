using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ModdingUtils.Extensions;
using UnityEngine;
using UnityEngine.Windows;

public class SoulstreakPlayer
{
    public Player Player { get; set; }
    public int PlayerKills { get; set; }

    public Dictionary<string, float> BaseDictionary { get; set; }

    public SoulstreakPlayer(Player player, int kills, CharacterData baseData, Dictionary<string, float> baseDictionary)
    {
        Player = player;
        PlayerKills = kills;
        BaseDictionary = baseDictionary;
    }
}

public static class SoulstreakPlayers
{
    public static List<SoulstreakPlayer> SoulstreakPlayersList = new List<SoulstreakPlayer>();
    public static List<int> deadPlayerID = new List<int>();
    public static Dictionary<string, float> OldStatsDictionary = new Dictionary<string, float>();

    public static void AddPlayerToSoulstreak(Player player)
    {
        SoulstreakPlayersList.Add(new SoulstreakPlayer(player, 0, null, null));
    }

    public static void RemovePlayerFromSoulstreak(Player player)
    {
        SoulstreakPlayersList.RemoveAll(soulstreakPlayer => soulstreakPlayer.Player == player);
    }

    public static bool PlayerAlreadyAddedInList(Player player)
    {
        return SoulstreakPlayersList.Any(soulstreakPlayer => soulstreakPlayer.Player == player);
    }

    public static SoulstreakPlayer getSoulstreakPlayer(Player player)
    {
        return SoulstreakPlayersList.Find(soulstreakPlayer => soulstreakPlayer.Player == player);
    }

    public static void SetKills(Player player, int kills)
    {
        SoulstreakPlayer soulstreakPlayer = SoulstreakPlayersList.Find(sp => sp.Player == player);
        soulstreakPlayer.PlayerKills = kills;
        
    }

    public static int GetKills(Player player)
    {
        SoulstreakPlayer soulstreakPlayer = SoulstreakPlayersList.Find(sp => sp.Player == player);
        return soulstreakPlayer.PlayerKills;
    }

    public static void SetBaseStats(Player player, Dictionary<string, float> data)
    {
        SoulstreakPlayer soulstreakPlayer = SoulstreakPlayersList.Find(sp => sp.Player == player);
        soulstreakPlayer.BaseDictionary = data;
    }

    public static Dictionary<string, float> getBaseStats(Player player)
    {
        SoulstreakPlayer soulstreakPlayer = SoulstreakPlayersList.Find(sp => sp.Player == player);
        return soulstreakPlayer.BaseDictionary;
    }

    public static Dictionary<string, float> GetStatsMultiplyPerKill(Player player)
    {
        Dictionary<string, float> statsDictionary = new Dictionary<string, float>
        {
            { "DamageMultiplyPerKill", 0f },
            { "MovementSpeedMultiplyPerKill", 0f },
            { "ATkSpeedMultiplyPerKill", 0f },
            { "BlockCooldownMultiplyPerKill", 0f }
        };

        bool foundSoulstreakStatsModifier = false;

        foreach (var card in player.data.currentCards)
        {
            GameObject cardGameObject = card.gameObject;
            SoulstreakStatsModifiers soulstreakStatsModifiers = cardGameObject.GetComponent<SoulstreakStatsModifiers>();

            // Check if the card has SoulstreakStatsModifiers component
            if (soulstreakStatsModifiers != null)
            {
                foundSoulstreakStatsModifier = true;

                statsDictionary["DamageMultiplyPerKill"] += soulstreakStatsModifiers.DamageMultiplyPerKill;
                statsDictionary["MovementSpeedMultiplyPerKill"] += soulstreakStatsModifiers.MovementSpeedMultiplyPerKill;
                statsDictionary["ATkSpeedMultiplyPerKill"] += soulstreakStatsModifiers.ATkSpeedMultiplyPerKill;
                statsDictionary["BlockCooldownMultiplyPerKill"] += soulstreakStatsModifiers.BlockCooldownMultiplyPerKill;
            }
        }
        if (!foundSoulstreakStatsModifier)
        {
            statsDictionary["DamageMultiplyPerKill"] = 1;
            statsDictionary["MovementSpeedMultiplyPerKill"] = 1;
            statsDictionary["ATkSpeedMultiplyPerKill"] = 1;
            statsDictionary["BlockCooldownMultiplyPerKill"] = 1;
        }
        return statsDictionary;
    }

    public static IEnumerator omRoundStart()
    {
        yield return null;
        for (int i = 0; i < PlayerManager.instance.players.Count; i++)
        {
            Player player = PlayerManager.instance.players[i];
            if (PlayerAlreadyAddedInList(player))
            {
                Dictionary<string, float> statsDictionary = GetStatsMultiplyPerKill(player);
                SetBaseStats(player, statsDictionary);
                player.data.weaponHandler.gun.damage *= (GetKills(player) != 0) ? statsDictionary["DamageMultiplyPerKill"] * GetKills(player) : 1;
                player.data.weaponHandler.gun.attackSpeed *= (GetKills(player) != 0) ? statsDictionary["ATkSpeedMultiplyPerKill"] * GetKills(player) : 1;
                player.data.stats.movementSpeed *= (GetKills(player) != 0) ? statsDictionary["MovementSpeedMultiplyPerKill"] * GetKills(player) : 1;
                player.data.block.cooldown *= (GetKills(player) != 0) ? statsDictionary["BlockCooldownMultiplyPerKill"] * GetKills(player) : 1;
            }
        }
        yield break;
    }
    public static IEnumerator omGameEnd()
    {
        SoulstreakPlayersList.Clear();
        yield break;
    }



    public static void resetPlayerSoulstreakStats(Player player)
    {

        Dictionary<string, float> baseStats = getBaseStats(player);
        player.data.weaponHandler.gun.damage /= baseStats["DamageMultiplyPerKill"];
        player.data.weaponHandler.gun.attackSpeed /= baseStats["ATkSpeedMultiplyPerKill"];
        player.data.stats.movementSpeed /= baseStats["MovementSpeedMultiplyPerKill"];
        player.data.block.cooldown /= baseStats["BlockCooldownMultiplyPerKill"];

        Dictionary<string, float> statsDictionary = GetStatsMultiplyPerKill(player);
        SetBaseStats(player, statsDictionary);
        player.data.weaponHandler.gun.damage *= (GetKills(player) != 0) ? statsDictionary["DamageMultiplyPerKill"] * GetKills(player) : 1;
        player.data.weaponHandler.gun.attackSpeed *= (GetKills(player) != 0) ? statsDictionary["ATkSpeedMultiplyPerKill"] * GetKills(player) : 1;
        player.data.stats.movementSpeed *= (GetKills(player) != 0) ? statsDictionary["MovementSpeedMultiplyPerKill"] * GetKills(player) : 1;
        player.data.block.cooldown *= (GetKills(player) != 0) ? statsDictionary["BlockCooldownMultiplyPerKill"] * GetKills(player) : 1;
    }

    public static IEnumerator omRoundEnd()
    {
        yield return null;
        for (int i = 0; i < PlayerManager.instance.players.Count; i++)
        {
            Player player = PlayerManager.instance.players[i];
            if (PlayerAlreadyAddedInList(player))
            {
                Dictionary<string, float> baseStats = getBaseStats(player);
                player.data.weaponHandler.gun.damage /= baseStats["DamageMultiplyPerKill"];
                player.data.weaponHandler.gun.attackSpeed /= baseStats["ATkSpeedMultiplyPerKill"];
                player.data.stats.movementSpeed /= baseStats["MovementSpeedMultiplyPerKill"];
                player.data.block.cooldown /= baseStats["BlockCooldownMultiplyPerKill"];

            }
        }
        deadPlayerID.Clear();
        yield break;
    }



    public static void GetDeadPlayer()
    {
        Player[] allPlayers = Resources.FindObjectsOfTypeAll<Player>();

        foreach (Player player in allPlayers)
        {
            if (player != null && player.data != null && player.data.dead && !deadPlayerID.Contains(player.playerID))
            {
                Player playerThatDamageThePlayer = player.data.lastSourceOfDamage;

                if (playerThatDamageThePlayer != null)
                {
                    bool playerThatKillThePlayerIsSoulstreakPlayers = SoulstreakPlayersList.Any(soulstreakPlayer =>
                        soulstreakPlayer.Player != null &&
                        soulstreakPlayer.Player.playerID == playerThatDamageThePlayer.playerID);

                    if (playerThatKillThePlayerIsSoulstreakPlayers)
                    {
                        SetKills(playerThatDamageThePlayer, GetKills(player) + 1);
                        resetPlayerSoulstreakStats(player);
                        deadPlayerID.Add(player.playerID);
                    }
                }
            }
        }
    }

    public static void Update()
    {
        GetDeadPlayer();
        for (int i = 0; i < PlayerManager.instance.players.Count; i++)
        {
            Player player = PlayerManager.instance.players[i];
            bool hasCardWithName = player.data.currentCards.Any(card => card.name == "__AAC__Soulstreak");

            if (hasCardWithName && !PlayerAlreadyAddedInList(player))
            {
                UnityEngine.Debug.Log("Adding player to SoulstreakPlayersList: " + player.playerID); // Add this line for debugging
                AddPlayerToSoulstreak(player);
                player.data.stats.GetAdditionalData().blacklistedCategories.Add(AALUND13_Cards.SoulstreakClassCards);
            }
            else if (!hasCardWithName && PlayerAlreadyAddedInList(player))
            {
                UnityEngine.Debug.Log("Removing player from SoulstreakPlayersList: " + player.playerID); // Add this line for debugging
                RemovePlayerFromSoulstreak(player);
                player.data.stats.GetAdditionalData().blacklistedCategories.Remove(AALUND13_Cards.SoulstreakClassCards);
            }
        }
    }

}
