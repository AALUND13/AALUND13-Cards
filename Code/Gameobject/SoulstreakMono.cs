using HarmonyLib;
using JARL.ArmorFramework;
using JARL.ArmorFramework.Abstract;
using JARL.ArmorFramework.Utlis;
using System;
using TMPro;
using UnityEngine;
using static AALUND13Card.Classes;

namespace AALUND13Card.MonoBehaviours
{
    public enum AbilityType
    {
        none,
        armor
    }

    [Serializable]
    public class SoulStreakStats
    {
        [Header("Health")]
        public float HealthMultiplyPerKill = 1;
        public float HealPercentagePerKill = 0;

        [Header("Soul Armor")]
        public float soulArmorPercentage = 0;
        public float soulArmorPercentageRegenRate = 0;

        [Header("Gun")]
        public float DamageMultiplyPerKill = 1;
        public float ATKSpeedMultiplyPerKill = 1;

        [Header("Other")]
        public float MovementSpeedMultiplyPerKill = 1;
        public float BlockCooldownMultiplyPerKill = 1;
        public AbilityType abilityType = AbilityType.none;

        public float SoulDrainMultiply = 0;

        public void AddStats(SoulStreakStats soulStreakStats)
        {
            ATKSpeedMultiplyPerKill = Mathf.Max(ATKSpeedMultiplyPerKill + soulStreakStats.ATKSpeedMultiplyPerKill, 0.5f);
            BlockCooldownMultiplyPerKill = Mathf.Max(BlockCooldownMultiplyPerKill + soulStreakStats.BlockCooldownMultiplyPerKill, 0.5f);
            DamageMultiplyPerKill = Mathf.Max(DamageMultiplyPerKill + soulStreakStats.DamageMultiplyPerKill, 0.5f);
            MovementSpeedMultiplyPerKill = Mathf.Max(MovementSpeedMultiplyPerKill + soulStreakStats.MovementSpeedMultiplyPerKill, 0.5f);
            HealthMultiplyPerKill = Mathf.Max(HealthMultiplyPerKill + soulStreakStats.HealthMultiplyPerKill, 0.5f);
            HealPercentagePerKill += soulStreakStats.HealPercentagePerKill;
            soulArmorPercentage += soulStreakStats.soulArmorPercentage;
            soulArmorPercentageRegenRate += soulStreakStats.soulArmorPercentageRegenRate;
            SoulDrainMultiply += soulStreakStats.SoulDrainMultiply;

            if (soulStreakStats.abilityType != AbilityType.none)
            {
                abilityType = soulStreakStats.abilityType;
            }
        }
    }

    public class SoulstreakMono : MonoBehaviour
    {
        public SoulStreakStats soulstreakStats = new SoulStreakStats();
        public float abilityCooldown = 0;
        public int killsStreak = 0;

        public Player player = null;

        private bool alreadySetBaseStats = true;

        public bool abilityActive = false;

        public bool canResetKills = true;

        public PlayerStats baseCharacterData;

     
        public GameObject soulsCounter = null;
        public GameObject soulsCounterGUI = null;

        // Start is called before the first frame update
        private void Start()
        {
            player = gameObject.transform.parent.GetComponent<Player>();
            baseCharacterData = new PlayerStats(player.data);
            soulsCounter = Instantiate(soulsCounter);
            if (player.data.view.IsMine && !player.GetComponent<PlayerAPI>().enabled)
            {
                soulsCounterGUI = Instantiate(soulsCounterGUI);
                soulsCounterGUI.transform.SetParent(player.transform.parent);
            }
            player.data.SetWobbleObjectChild(soulsCounter.transform);
            soulsCounter.transform.localPosition = new Vector2(0, 0.3f);
        }

        private void OnDestroy()
        {
            if (player.data.view.IsMine && !player.GetComponent<PlayerAPI>().enabled)
            {
                Destroy(soulsCounterGUI);
            }
            Destroy(soulsCounter);
        }

        // Update is called once per frame
        private void Update()
        {
            if (player.data.isPlaying)
            {
                abilityCooldown = Mathf.Max(abilityCooldown - Time.deltaTime, 0);

                if (GetComponentInParent<ArmorHandler>().GetArmorByType("Soul").currentArmorValue <= 0 && GetComponentInParent<ArmorHandler>().GetArmorByType("Soul").maxArmorValue > 0)
                {
                    GetComponentInParent<ArmorHandler>().GetArmorByType("Soul").maxArmorValue = 0;
                    abilityCooldown = 10;
                    abilityActive = false;
                }

                // Soul Armor
                //ArmorBase soulArmor = GetComponentInParent<ArmorHandler>().GetArmorByType("Soul");
                //soulArmor.maxArmorValue = player.data.maxHealth * soulstreakStats.soulArmorPercentage;
                //soulArmor.armorRegenerationRate = player.data.maxHealth * soulstreakStats.soulArmorPercentageRegen;
            }
            soulsCounter.GetComponent<TextMeshPro>().text = "Soul : " + killsStreak;
            if (player.data.view.IsMine && !player.GetComponent<PlayerAPI>().enabled)
            {
                soulsCounterGUI.GetComponentInChildren<TextMeshProUGUI>().text = "Soul : " + killsStreak;
            }
        }
        public void BlockAbility()
        {
            if (soulstreakStats.abilityType == AbilityType.armor && !abilityActive && abilityCooldown == 0)
            {
                ArmorBase soulArmor = GetComponentInParent<ArmorHandler>().GetArmorByType("Soul");
                soulArmor.maxArmorValue = player.data.maxHealth * soulstreakStats.soulArmorPercentage * (killsStreak + 1);
                soulArmor.armorRegenerationRate = soulArmor.maxArmorValue * soulstreakStats.soulArmorPercentageRegenRate;
                soulArmor.currentArmorValue = soulArmor.maxArmorValue;
                abilityActive = true;
            }
        }

        public void ResetToBase()
        {
            alreadySetBaseStats = true;

            player.data.weaponHandler.gun.damage = baseCharacterData.damage;
            player.data.weaponHandler.gun.attackSpeed = baseCharacterData.attackSpeed;
            player.data.stats.movementSpeed = baseCharacterData.movementSpeed;
            player.data.block.cooldown = baseCharacterData.cooldown;
            player.data.maxHealth = baseCharacterData.maxHealth;
            player.data.health = baseCharacterData.health;

            AccessTools.Method(typeof(CharacterStatModifiers), "ConfigureMassAndSize").Invoke(player.data.stats, null);
        }

        public void SetToBaseStats()
        {
            if (killsStreak > 0 && !alreadySetBaseStats)
            {
                alreadySetBaseStats = true;
                player.data.weaponHandler.gun.damage *= 1f / (1 + (soulstreakStats.DamageMultiplyPerKill - 1) * killsStreak);
                player.data.weaponHandler.gun.attackSpeed *= 1f / (1 + (soulstreakStats.ATKSpeedMultiplyPerKill - 1) * killsStreak);
                player.data.stats.movementSpeed *= 1f / (1 + (soulstreakStats.MovementSpeedMultiplyPerKill - 1) * killsStreak);
                player.data.block.cooldown *= 1f / (1 + (soulstreakStats.BlockCooldownMultiplyPerKill - 1) * killsStreak);
                player.data.maxHealth *= 1f / (1 + (soulstreakStats.HealthMultiplyPerKill - 1) * killsStreak);
                player.data.health *= 1f / (1 + (soulstreakStats.HealthMultiplyPerKill - 1) * killsStreak);

                AccessTools.Method(typeof(CharacterStatModifiers), "ConfigureMassAndSize").Invoke(player.data.stats, null);
            }
        }

        public void SetStats()
        {
            if (killsStreak > 0 && alreadySetBaseStats)
            {
                alreadySetBaseStats = false;
                player.data.weaponHandler.gun.damage *= 1 + (soulstreakStats.DamageMultiplyPerKill - 1) * killsStreak;
                player.data.weaponHandler.gun.attackSpeed *= 1 + (soulstreakStats.ATKSpeedMultiplyPerKill - 1) * killsStreak;
                player.data.stats.movementSpeed *= 1 + (soulstreakStats.MovementSpeedMultiplyPerKill - 1) * killsStreak;
                player.data.block.cooldown *= 1 + (soulstreakStats.BlockCooldownMultiplyPerKill - 1) * killsStreak;
                player.data.maxHealth *= 1 + (soulstreakStats.HealthMultiplyPerKill - 1) * killsStreak;
                player.data.health *= 1 + (soulstreakStats.HealthMultiplyPerKill - 1) * killsStreak;

                AccessTools.Method(typeof(CharacterStatModifiers), "ConfigureMassAndSize").Invoke(player.data.stats, null);
            }

            player.data.healthHandler.Heal(player.data.maxHealth * soulstreakStats.HealPercentagePerKill);
        }

        public void ResetKill()
        {
            if (canResetKills)
            {
                Utils.LogInfo($"Resetting kill streak of player with ID {player.playerID}");
                SetToBaseStats();
                killsStreak = 0;
                if (player.data.view.IsMine)
                {
                    soulsCounterGUI.GetComponentInChildren<TextMeshProUGUI>().text = "Soul: 0";
                }
            }
        }

        public void AddKill(int kills = 1)
        {
            if (canResetKills)
            {
                Utils.LogInfo($"Adding {kills} kills for player with ID {player.playerID}");
                SetToBaseStats();
                killsStreak += kills;
                SetStats();
            }
        }

        //private void PlayerDied(Player deadPlayer, Player killingPlayer)
        //{
        //    if (killingPlayer != null && killingPlayer.playerID == soulstreakPlayer.playerID && deadPlayer.playerID != soulstreakPlayer.playerID)
        //    {
        //        UnityEngine.Debug.Log("Soulstreak player '" + soulstreakPlayer.playerID + "' killed player '" + deadPlayer.playerID + "' adding killed");
        //        SetToBaseStats();
        //        killsStreak++;
        //        SetStats();
        //    }
        //    else if (deadPlayer.playerID == soulstreakPlayer.playerID)
        //    {
        //        if (global::PlayerDied.CountOfAliveEnemyPlayers(soulstreakPlayer) == 0) return;

        //        if (killingPlayer != null && killingPlayer.GetComponentInChildren<SoulstreakObject>() != null && killingPlayer != deadPlayer)
        //        {
        //            SoulstreakObject deadPlayerSoulstreakObject = deadPlayer.GetComponentInChildren<SoulstreakObject>();
        //            killingPlayer.GetComponentInChildren<SoulstreakObject>().killsToGive += (int)Mathf.Round((float)(deadPlayerSoulstreakObject.killsStreak * 0.5));
        //        }

        //        UnityEngine.Debug.Log("Soulstreak player '" + soulstreakPlayer.playerID + "' died, resetting kill");
        //        SetToBaseStats();
        //        killsStreak = 0;
        //    }
        //}
    }
}