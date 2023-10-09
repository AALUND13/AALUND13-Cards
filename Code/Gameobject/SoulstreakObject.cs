using HarmonyLib;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnboundLib.GameModes;
using UnityEngine;

public class SoulstreakObject : MonoBehaviour
{
    private Player soulstreakPlayer = null;
    private AAStatsModifiers soulstreakStats = null;
    private TextMeshPro text = null;

    private int kills = 0;

    private bool alreadySetBaseStats = true;

    // Start is called before the first frame update
    private void Start()
    {
        soulstreakPlayer = gameObject.transform.parent.GetComponent<Player>();
        soulstreakStats = gameObject.GetComponent<AAStatsModifiers>();
        text = gameObject.GetComponent<TextMeshPro>();
        global::PlayerDied.playerDiedList.Add(PlayerDied);
    }

    // Update is called once per frame
    private void Update()
    {
        resetSoulstreakStats();
        foreach (CardInfo card in soulstreakPlayer.data.currentCards)
        {
            if (card.GetComponent<AAStatsModifiers>() != null)
            {
                AAStatsModifiers soulstreakStatsFromCard = card.GetComponent<AAStatsModifiers>();
                soulstreakStats.ATkSpeedMultiplyPerKill = Mathf.Max(soulstreakStats.ATkSpeedMultiplyPerKill + soulstreakStatsFromCard.ATkSpeedMultiplyPerKill, 0.5f);
                soulstreakStats.BlockCooldownMultiplyPerKill = Mathf.Max(soulstreakStats.BlockCooldownMultiplyPerKill + soulstreakStatsFromCard.BlockCooldownMultiplyPerKill, 0.5f);
                soulstreakStats.DamageMultiplyPerKill = Mathf.Max(soulstreakStats.DamageMultiplyPerKill + soulstreakStatsFromCard.DamageMultiplyPerKill, 0.5f);
                soulstreakStats.MovementSpeedMultiplyPerKill = Mathf.Max(soulstreakStats.MovementSpeedMultiplyPerKill + soulstreakStatsFromCard.MovementSpeedMultiplyPerKill, 0.5f);
                soulstreakStats.MaxMultiplyPerKill = Mathf.Max(soulstreakStats.MaxMultiplyPerKill + soulstreakStatsFromCard.MaxMultiplyPerKill, 0.5f);
                soulstreakStats.HealthMultiplyPerKill = Mathf.Max(soulstreakStats.HealthMultiplyPerKill + soulstreakStatsFromCard.HealthMultiplyPerKill, 0.5f);
                soulstreakStats.HealPercentagePerKill += soulstreakStatsFromCard.HealPercentagePerKill;
            }
        }
        text.text = "Soul : " + kills;
    }

    private void OnDestroy()
    {
        global::PlayerDied.playerDiedList.Remove(PlayerDied);
    }

    public void resetSoulstreakStats()
    {
        soulstreakStats.ATkSpeedMultiplyPerKill = 1;
        soulstreakStats.BlockCooldownMultiplyPerKill = 1;
        soulstreakStats.DamageMultiplyPerKill = 1;
        soulstreakStats.MovementSpeedMultiplyPerKill = 1;
        soulstreakStats.MaxMultiplyPerKill = 1;
        soulstreakStats.HealthMultiplyPerKill = 1;
        soulstreakStats.HealPercentagePerKill = 0;
    }

    public void setToBaseStats()
    {
        if (kills > 0 && !alreadySetBaseStats)
        {
            alreadySetBaseStats = true;
            soulstreakPlayer.data.weaponHandler.gun.damage /= Mathf.Pow(soulstreakStats.DamageMultiplyPerKill, kills);
            soulstreakPlayer.data.weaponHandler.gun.attackSpeed /= Mathf.Pow(soulstreakStats.ATkSpeedMultiplyPerKill, kills);
            soulstreakPlayer.data.stats.movementSpeed /= Mathf.Pow(soulstreakStats.MovementSpeedMultiplyPerKill, kills);
            soulstreakPlayer.data.block.cooldown /= Mathf.Pow(soulstreakStats.BlockCooldownMultiplyPerKill, kills);
            soulstreakPlayer.data.maxHealth /= Mathf.Pow(soulstreakStats.MaxMultiplyPerKill, kills);
            soulstreakPlayer.data.health /= Mathf.Pow(soulstreakStats.HealthMultiplyPerKill, kills);
            AccessTools.Method(typeof(CharacterStatModifiers), "ConfigureMassAndSize").Invoke(soulstreakPlayer.data.stats, null);
        }
    }
    
    public void setStats()
    {
        if (kills > 0 && alreadySetBaseStats)
        {
            alreadySetBaseStats = false;
            soulstreakPlayer.data.weaponHandler.gun.damage *= Mathf.Pow(soulstreakStats.DamageMultiplyPerKill, kills);
            soulstreakPlayer.data.weaponHandler.gun.attackSpeed *= Mathf.Pow(soulstreakStats.ATkSpeedMultiplyPerKill, kills);
            soulstreakPlayer.data.stats.movementSpeed *= Mathf.Pow(soulstreakStats.MovementSpeedMultiplyPerKill, kills);
            soulstreakPlayer.data.block.cooldown *= Mathf.Pow(soulstreakStats.BlockCooldownMultiplyPerKill, kills);
            soulstreakPlayer.data.maxHealth *= Mathf.Pow(soulstreakStats.MaxMultiplyPerKill, kills);
            soulstreakPlayer.data.health *= Mathf.Pow(soulstreakStats.HealthMultiplyPerKill, kills);

            AccessTools.Method(typeof(CharacterStatModifiers), "ConfigureMassAndSize").Invoke(soulstreakPlayer.data.stats, null);
        }

        soulstreakPlayer.data.healthHandler.Heal(soulstreakPlayer.data.maxHealth * soulstreakStats.HealPercentagePerKill);
    }

    private void PlayerDied(Player deadPlayer, Player killingPlayer)
    {
        if (killingPlayer != null && killingPlayer.playerID == soulstreakPlayer.playerID && deadPlayer.playerID != soulstreakPlayer.playerID)
        {
            UnityEngine.Debug.Log("Soulstreak player '" + soulstreakPlayer.playerID + "' killed player '" + deadPlayer.playerID + "' adding killed");
            setToBaseStats();
            kills++;
            setStats();
        }
        else if (deadPlayer.playerID == soulstreakPlayer.playerID)
        {
            if (global::PlayerDied.CountOfAliveEnemyPlayers(soulstreakPlayer) == 0) return;

            UnityEngine.Debug.Log("Soulstreak player '" + soulstreakPlayer.playerID + "' died, resetting kill");
            setToBaseStats();
            kills = 0;
        }
    }

    //private void setDeadPlay()
    //{
    //    for (int i = 0; i < PlayerManager.instance.players.Count; i++)
    //    {
    //        Player player = PlayerManager.instance.players[i];
    //        if (player.data.dead && !deadPlayerID.Contains(player.playerID))
    //        {
    //            deadPlayerID.Add(player.playerID);
    //            playerDied(player);
    //            UnityEngine.Debug.Log("Added player id:'" + player.playerID + "' to the dead player id list");
    //        }
    //        else if (!player.data.dead && deadPlayerID.Contains(player.playerID))
    //        {
    //            deadPlayerID.Remove(player.playerID);
    //            UnityEngine.Debug.Log("Remove player id:'" + player.playerID + "' from the dead player id list");
    //            ModdingUtils.Utils.PlayerStatus.PlayerAlive(player);
    //        }
    //    }
    //}
}
