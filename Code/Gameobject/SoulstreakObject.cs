using HarmonyLib;
using TMPro;
using UnityEngine;
using ModsPlus;
using UnboundLib;
public class SoulstreakObject : MonoBehaviour
{
    private Player soulstreakPlayer = null;
    private AAStatsModifiers soulstreakStats = null;
    private TextMeshPro text = null;

    private int kills = 0;
    private int killsToGive = 0;

    public float soulShieldHealth = 0;
    public float soulShieldMaxHealth = 0;

    private bool alreadySetBaseStats = true;

    public bool soulShieldDepleted = false;

    CustomHealthBar soulShieldBar;

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
        ResetSoulstreakStats();
        foreach (CardInfo card in soulstreakPlayer.data.currentCards)
        {
            if (card.GetComponent<AAStatsModifiers>() != null)
            {
                AAStatsModifiers soulstreakStatsFromCard = card.GetComponent<AAStatsModifiers>();
                soulstreakStats.ATKSpeedMultiplyPerKill = Mathf.Max(soulstreakStats.ATKSpeedMultiplyPerKill + soulstreakStatsFromCard.ATKSpeedMultiplyPerKill, 0.5f);
                soulstreakStats.BlockCooldownMultiplyPerKill = Mathf.Max(soulstreakStats.BlockCooldownMultiplyPerKill + soulstreakStatsFromCard.BlockCooldownMultiplyPerKill, 0.5f);
                soulstreakStats.DamageMultiplyPerKill = Mathf.Max(soulstreakStats.DamageMultiplyPerKill + soulstreakStatsFromCard.DamageMultiplyPerKill, 0.5f);
                soulstreakStats.MovementSpeedMultiplyPerKill = Mathf.Max(soulstreakStats.MovementSpeedMultiplyPerKill + soulstreakStatsFromCard.MovementSpeedMultiplyPerKill, 0.5f);
                soulstreakStats.MaxMultiplyPerKill = Mathf.Max(soulstreakStats.MaxMultiplyPerKill + soulstreakStatsFromCard.MaxMultiplyPerKill, 0.5f);
                soulstreakStats.HealthMultiplyPerKill = Mathf.Max(soulstreakStats.HealthMultiplyPerKill + soulstreakStatsFromCard.HealthMultiplyPerKill, 0.5f);
                soulstreakStats.HealPercentagePerKill += soulstreakStatsFromCard.HealPercentagePerKill;
                soulstreakStats.soulShieldPercentage += soulstreakStatsFromCard.soulShieldPercentage;
                soulstreakStats.soulShieldPercentageRegen += soulstreakStatsFromCard.soulShieldPercentageRegen;
            }
        }
        if (killsToGive != 0)
        {
            SetToBaseStats();
            kills += killsToGive;
            killsToGive = 0;
            SetStats();
        }

        // Soul Shield Regen
        if (!soulstreakPlayer.data.dead)
        {
            soulShieldHealth = Mathf.Min(soulShieldHealth + (soulstreakPlayer.data.maxHealth * soulstreakStats.soulShieldPercentageRegen) * Time.deltaTime, soulShieldMaxHealth);
        }
        // Soul Shield Depleted
        if (soulShieldDepleted)
        {
            if (soulShieldHealth/soulShieldMaxHealth >= 0.3)
            {
                soulShieldDepleted = false;
            }
        }
        // Soul Shield Bar
        if (soulstreakStats.soulShieldPercentage > 0)
        {
            if (soulShieldBar == null)
            {
                Transform parent = soulstreakPlayer.GetComponentInChildren<PlayerWobblePosition>().transform;

                var obj = new GameObject("Soul Shield Bar");
                obj.transform.SetParent(parent);
                soulShieldBar = obj.AddComponent<CustomHealthBar>();

                soulShieldBar.transform.localPosition = Vector3.up * 0.25f;
                soulShieldBar.transform.localScale = Vector3.one;
                RegenSoulShield();
            }

            soulShieldBar.SetValues(soulShieldHealth, soulShieldMaxHealth);

            AccessTools.Field(typeof(HealthBar), "sinceDamage").SetValue(soulShieldBar.GetBaseHealthBar(), 100);

            if (!soulShieldDepleted)
            {
                soulShieldBar.SetColor(new Color(1, 0, 1));
            }
            else
            {
                soulShieldBar.SetColor(new Color(0.45f, 0, 0.45f));
            }
        }
        text.text = "Soul : " + kills;
    }

    private void OnDestroy()
    {
        global::PlayerDied.playerDiedList.Remove(PlayerDied);
        Destroy(soulShieldBar);
    }

    public void ResetSoulstreakStats()
    {
        soulstreakStats.ATKSpeedMultiplyPerKill = 1;
        soulstreakStats.BlockCooldownMultiplyPerKill = 1;
        soulstreakStats.DamageMultiplyPerKill = 1;
        soulstreakStats.MovementSpeedMultiplyPerKill = 1;
        soulstreakStats.MaxMultiplyPerKill = 1;
        soulstreakStats.HealthMultiplyPerKill = 1;
        soulstreakStats.HealPercentagePerKill = 0;
        soulstreakStats.soulShieldPercentage = 0;
        soulstreakStats.soulShieldPercentageRegen = 0;
    }

    public void SetToBaseStats()
    {
        if (kills > 0 && !alreadySetBaseStats)
        {
            alreadySetBaseStats = true;
            soulstreakPlayer.data.weaponHandler.gun.damage /= Mathf.Pow(soulstreakStats.DamageMultiplyPerKill, kills);
            soulstreakPlayer.data.weaponHandler.gun.attackSpeed /= Mathf.Pow(soulstreakStats.ATKSpeedMultiplyPerKill, kills);
            soulstreakPlayer.data.stats.movementSpeed /= Mathf.Pow(soulstreakStats.MovementSpeedMultiplyPerKill, kills);
            soulstreakPlayer.data.block.cooldown /= Mathf.Pow(soulstreakStats.BlockCooldownMultiplyPerKill, kills);
            soulstreakPlayer.data.maxHealth /= Mathf.Pow(soulstreakStats.MaxMultiplyPerKill, kills);
            soulstreakPlayer.data.health /= Mathf.Pow(soulstreakStats.HealthMultiplyPerKill, kills);

            soulShieldMaxHealth = soulstreakPlayer.data.maxHealth * soulstreakStats.soulShieldPercentage;

            AccessTools.Method(typeof(CharacterStatModifiers), "ConfigureMassAndSize").Invoke(soulstreakPlayer.data.stats, null);
        }
    }
    
    public void RegenSoulShield()
    {
        soulShieldMaxHealth = soulstreakPlayer.data.maxHealth * soulstreakStats.soulShieldPercentage;
        soulShieldHealth = soulShieldMaxHealth;
        soulShieldDepleted = false;
    }

    public void SetStats()
    {
        if (kills > 0 && alreadySetBaseStats)
        {
            alreadySetBaseStats = false;
            soulstreakPlayer.data.weaponHandler.gun.damage *= Mathf.Pow(soulstreakStats.DamageMultiplyPerKill, kills);
            soulstreakPlayer.data.weaponHandler.gun.attackSpeed *= Mathf.Pow(soulstreakStats.ATKSpeedMultiplyPerKill, kills);
            soulstreakPlayer.data.stats.movementSpeed *= Mathf.Pow(soulstreakStats.MovementSpeedMultiplyPerKill, kills);
            soulstreakPlayer.data.block.cooldown *= Mathf.Pow(soulstreakStats.BlockCooldownMultiplyPerKill, kills);
            soulstreakPlayer.data.maxHealth *= Mathf.Pow(soulstreakStats.MaxMultiplyPerKill, kills);
            soulstreakPlayer.data.health *= Mathf.Pow(soulstreakStats.HealthMultiplyPerKill, kills);

            soulShieldMaxHealth = soulstreakPlayer.data.maxHealth * soulstreakStats.soulShieldPercentage;

            AccessTools.Method(typeof(CharacterStatModifiers), "ConfigureMassAndSize").Invoke(soulstreakPlayer.data.stats, null);
        }

        soulstreakPlayer.data.healthHandler.Heal(soulstreakPlayer.data.maxHealth * soulstreakStats.HealPercentagePerKill);
    }

    private void PlayerDied(Player deadPlayer, Player killingPlayer)
    {
        if (killingPlayer != null && killingPlayer.playerID == soulstreakPlayer.playerID && deadPlayer.playerID != soulstreakPlayer.playerID)
        {
            UnityEngine.Debug.Log("Soulstreak player '" + soulstreakPlayer.playerID + "' killed player '" + deadPlayer.playerID + "' adding killed");
            SetToBaseStats();
            kills++;
            SetStats();
        }
        else if (deadPlayer.playerID == soulstreakPlayer.playerID)
        {
            if (global::PlayerDied.CountOfAliveEnemyPlayers(soulstreakPlayer) == 0) return;

            if (killingPlayer != null && killingPlayer.GetComponentInChildren<SoulstreakObject>() != null && killingPlayer != deadPlayer)
            {
                SoulstreakObject deadPlayerSoulstreakObject = deadPlayer.GetComponentInChildren<SoulstreakObject>();
                killingPlayer.GetComponentInChildren<SoulstreakObject>().killsToGive += (int)Mathf.Round((float)(deadPlayerSoulstreakObject.kills * 0.5));
            }

            UnityEngine.Debug.Log("Soulstreak player '" + soulstreakPlayer.playerID + "' died, resetting kill");
            SetToBaseStats();
            kills = 0;
        }
    }
}
