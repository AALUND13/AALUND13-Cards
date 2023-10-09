using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerDied
{
    public static List<Action<Player, Player>> playerDiedList = new List<Action<Player, Player>>();

    public static void PlayerDiedAction(Player deadPlayer, Player killingPlayer)
    {
        foreach (Action<Player, Player> playerDied in playerDiedList)
        {
            playerDied.Invoke(deadPlayer, killingPlayer);
        }
    }

    public static int CountOfAliveEnemyPlayers(Player player)
    {
        return ModdingUtils.Utils.PlayerStatus.GetEnemyPlayers(player).FindAll(__player => !__player.data.dead).Count;
    }
}
