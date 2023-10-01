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
    private SoulstreakStats soulstreakStats = null;
    private TextMeshPro text = null;

    private List<int> deadPlayerID = new List<int>();

    private int kills = 0;

    private bool alreadySetBaseStats = true;

    private bool playerHasDied = false;

    // Start is called before the first frame update
    private void Start()
    {
        soulstreakPlayer = gameObject.transform.parent.GetComponent<Player>();
        soulstreakStats = gameObject.GetComponent<SoulstreakStats>();
        text = gameObject.GetComponent<TextMeshPro>();
    }

    // Update is called once per frame
    private void Update()
    {
        setDeadPlay();
        text.text = "Soul : " + kills;
        if (!soulstreakPlayer.data.currentCards.Any(Card => Card.cardName == "Soulstreak"))
        {
            Object.Destroy(gameObject);
        }
    }

    private void OnDisable()
    {
        playerHasDied = true;
    }

    private void OnEnable()
    {
        if (playerHasDied)
        {
            var gameWinners = GameModeManager.CurrentHandler.GetPointWinners();
            if (gameWinners != null && gameWinners.ToList().Any(playerId => playerId != soulstreakPlayer.playerID))
            {
                UnityEngine.Debug.Log("Soulstreak player '" + soulstreakPlayer.playerID + "' died, resetting kill");
                setToBaseStats();
                kills = 0;
            }
            playerHasDied = false;
        }
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
            soulstreakPlayer.data.health = Mathf.Min(soulstreakPlayer.data.health, soulstreakPlayer.data.maxHealth);
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
            soulstreakPlayer.data.health = Mathf.Min(soulstreakPlayer.data.health, soulstreakPlayer.data.maxHealth);
            AccessTools.Method(typeof(CharacterStatModifiers), "ConfigureMassAndSize").Invoke(soulstreakPlayer.data.stats, null);
        }
    }

    private void playerDied(Player player)
    {
        if (player.data.lastSourceOfDamage == null) return;
        if (player.data.lastSourceOfDamage.playerID == soulstreakPlayer.playerID && player.playerID != soulstreakPlayer.playerID)
        {
            UnityEngine.Debug.Log("Soulstreak player '" + soulstreakPlayer.playerID + "' killed player '" + player.playerID + "' adding killed");
            setToBaseStats();
            kills++;
            setStats();
        }
    }

    private void setDeadPlay()
    {
        for (int i = 0; i < PlayerManager.instance.players.Count; i++)
        {
            Player player = PlayerManager.instance.players[i];
            if (player.data.dead && !deadPlayerID.Contains(player.playerID))
            {
                deadPlayerID.Add(player.playerID);
                playerDied(player);
                UnityEngine.Debug.Log("Added player id:'" + player.playerID + "' to the dead player id list");
            }
            else if (!player.data.dead && deadPlayerID.Contains(player.playerID))
            {
                deadPlayerID.Remove(player.playerID);
                UnityEngine.Debug.Log("Remove player id:'" + player.playerID + "' from the dead player id list");
                ModdingUtils.Utils.PlayerStatus.PlayerAlive(player);
            }
        }
    }
}
