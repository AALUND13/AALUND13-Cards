using AALUND13Card.Armors;
using HarmonyLib;
using JARL.Armor;
using JARL.Armor.Bases;
using System;
using TMPro;
using UnityEngine;

namespace AALUND13Card.MonoBehaviours {
    public enum AbilityType {
        none,
        armor
    }

    [Serializable]
    public class SoulStreakStats {
        [Header("Health")]
        public float HealthMultiplyPerKill = 1;
        public float HealPercentagePerKill = 0;

        [Header("Soul Armor")]
        public float SoulArmorPercentage = 0;
        public float SoulArmorPercentageRegenRate = 0;

        [Header("Gun")]
        public float DamageMultiplyPerKill = 1;
        public float ATKSpeedMultiplyPerKill = 1;

        [Header("Other")]
        public float MovementSpeedMultiplyPerKill = 1;
        public float BlockCooldownMultiplyPerKill = 1;
        public AbilityType AbilityType = AbilityType.none;

        public float SoulDrainMultiply = 0;

        public void AddStats(SoulStreakStats soulStreakStats) {
            ATKSpeedMultiplyPerKill = Mathf.Max(ATKSpeedMultiplyPerKill + soulStreakStats.ATKSpeedMultiplyPerKill, 0.5f);
            BlockCooldownMultiplyPerKill = Mathf.Max(BlockCooldownMultiplyPerKill + soulStreakStats.BlockCooldownMultiplyPerKill, 0.5f);
            DamageMultiplyPerKill = Mathf.Max(DamageMultiplyPerKill + soulStreakStats.DamageMultiplyPerKill, 0.5f);
            MovementSpeedMultiplyPerKill = Mathf.Max(MovementSpeedMultiplyPerKill + soulStreakStats.MovementSpeedMultiplyPerKill, 0.5f);
            HealthMultiplyPerKill = Mathf.Max(HealthMultiplyPerKill + soulStreakStats.HealthMultiplyPerKill, 0.5f);
            HealPercentagePerKill += soulStreakStats.HealPercentagePerKill;
            SoulArmorPercentage += soulStreakStats.SoulArmorPercentage;
            SoulArmorPercentageRegenRate += soulStreakStats.SoulArmorPercentageRegenRate;
            SoulDrainMultiply += soulStreakStats.SoulDrainMultiply;

            if(soulStreakStats.AbilityType != AbilityType.none) {
                AbilityType = soulStreakStats.AbilityType;
            }
        }
    }

    public class SoulstreakMono : MonoBehaviour {
        public SoulStreakStats SoulstreakStats = new SoulStreakStats();
        public float AbilityCooldown = 0;
        public int KillsStreak = 0;

        public Player Player = null;

        private bool AlreadySetBaseStats = true;

        public bool AbilityActive = false;

        public bool CanResetKills = true;

        public PlayerStats BaseCharacterData;


        public GameObject SoulsCounter = null;
        public GameObject SoulsCounterGUI = null;

        public string SoulsString => $"{(KillsStreak > 1 ? "Souls" : "Soul")}: {KillsStreak}";

        // Start is called before the first frame update
        private void Start() {
            Player = gameObject.transform.parent.GetComponent<Player>();
            BaseCharacterData = new PlayerStats(Player.data);
            SoulsCounter = Instantiate(SoulsCounter);
            if(Player.data.view.IsMine && !Player.GetComponent<PlayerAPI>().enabled) {
                SoulsCounterGUI = Instantiate(SoulsCounterGUI);
                SoulsCounterGUI.transform.SetParent(Player.transform.parent);
            }
            Player.data.SetWobbleObjectChild(SoulsCounter.transform);
            SoulsCounter.transform.localPosition = new Vector2(0, 0.3f);
        }

        private void OnDestroy() {
            if(Player.data.view.IsMine && !Player.GetComponent<PlayerAPI>().enabled) {
                Destroy(SoulsCounterGUI);
            }
            Destroy(SoulsCounter);
        }

        // Update is called once per frame
        private void Update() {
            if(Player.data.isPlaying) {
                AbilityCooldown = Mathf.Max(AbilityCooldown - Time.deltaTime, 0);

                if(GetComponentInParent<ArmorHandler>().GetArmorByType(typeof(SoulArmor)).CurrentArmorValue <= 0 && GetComponentInParent<ArmorHandler>().GetArmorByType(typeof(SoulArmor)).MaxArmorValue > 0) {
                    GetComponentInParent<ArmorHandler>().GetArmorByType(typeof(SoulArmor)).MaxArmorValue = 0;
                    AbilityCooldown = 10;
                    AbilityActive = false;
                }

                // Soul Armor
                //ArmorBase soulArmor = GetComponentInParent<ArmorHandler>().GetArmorByType("Soul");
                //soulArmor.maxArmorValue = player.data.maxHealth * soulstreakStats.soulArmorPercentage;
                //soulArmor.armorRegenerationRate = player.data.maxHealth * soulstreakStats.soulArmorPercentageRegen;
            }
            SoulsCounter.GetComponent<TextMeshPro>().text = SoulsString;
            if(Player.data.view.IsMine && !Player.GetComponent<PlayerAPI>().enabled) {
                SoulsCounterGUI.GetComponentInChildren<TextMeshProUGUI>().text = SoulsString;
            }
        }
        public void BlockAbility() {
            if(SoulstreakStats.AbilityType == AbilityType.armor && !AbilityActive && AbilityCooldown == 0) {
                ArmorBase soulArmor = GetComponentInParent<ArmorHandler>().GetArmorByType(typeof(SoulArmor));
                soulArmor.MaxArmorValue = Player.data.maxHealth * SoulstreakStats.SoulArmorPercentage * (KillsStreak + 1);
                soulArmor.ArmorRegenerationRate = soulArmor.MaxArmorValue * SoulstreakStats.SoulArmorPercentageRegenRate;
                soulArmor.CurrentArmorValue = soulArmor.MaxArmorValue;
                AbilityActive = true;
            }
        }

        public void ResetToBase() {
            AlreadySetBaseStats = true;

            Player.data.weaponHandler.gun.damage = BaseCharacterData.Damage;
            Player.data.weaponHandler.gun.attackSpeed = BaseCharacterData.AttackSpeed;
            Player.data.stats.movementSpeed = BaseCharacterData.MovementSpeed;
            Player.data.block.cooldown = BaseCharacterData.Cooldown;
            Player.data.maxHealth = BaseCharacterData.MaxHealth;
            Player.data.health = BaseCharacterData.Health;

            AccessTools.Method(typeof(CharacterStatModifiers), "ConfigureMassAndSize").Invoke(Player.data.stats, null);
        }

        public void SetToBaseStats() {
            if(KillsStreak > 0 && !AlreadySetBaseStats) {
                AlreadySetBaseStats = true;
                Player.data.weaponHandler.gun.damage *= 1f / (1 + (SoulstreakStats.DamageMultiplyPerKill - 1) * KillsStreak);
                Player.data.weaponHandler.gun.attackSpeed *= 1f / (1 + (SoulstreakStats.ATKSpeedMultiplyPerKill - 1) * KillsStreak);
                Player.data.stats.movementSpeed *= 1f / (1 + (SoulstreakStats.MovementSpeedMultiplyPerKill - 1) * KillsStreak);
                Player.data.block.cooldown *= 1f / (1 + (SoulstreakStats.BlockCooldownMultiplyPerKill - 1) * KillsStreak);
                Player.data.maxHealth *= 1f / (1 + (SoulstreakStats.HealthMultiplyPerKill - 1) * KillsStreak);
                Player.data.health *= 1f / (1 + (SoulstreakStats.HealthMultiplyPerKill - 1) * KillsStreak);

                AccessTools.Method(typeof(CharacterStatModifiers), "ConfigureMassAndSize").Invoke(Player.data.stats, null);
            }
        }

        public void SetStats() {
            if(KillsStreak > 0 && AlreadySetBaseStats) {
                AlreadySetBaseStats = false;
                Player.data.weaponHandler.gun.damage *= 1 + (SoulstreakStats.DamageMultiplyPerKill - 1) * KillsStreak;
                Player.data.weaponHandler.gun.attackSpeed *= 1 + (SoulstreakStats.ATKSpeedMultiplyPerKill - 1) * KillsStreak;
                Player.data.stats.movementSpeed *= 1 + (SoulstreakStats.MovementSpeedMultiplyPerKill - 1) * KillsStreak;
                Player.data.block.cooldown *= 1 + (SoulstreakStats.BlockCooldownMultiplyPerKill - 1) * KillsStreak;
                Player.data.maxHealth *= 1 + (SoulstreakStats.HealthMultiplyPerKill - 1) * KillsStreak;
                Player.data.health *= 1 + (SoulstreakStats.HealthMultiplyPerKill - 1) * KillsStreak;

                AccessTools.Method(typeof(CharacterStatModifiers), "ConfigureMassAndSize").Invoke(Player.data.stats, null);
            }

            Player.data.healthHandler.Heal(Player.data.maxHealth * SoulstreakStats.HealPercentagePerKill);
        }

        public void ResetKill() {
            if(CanResetKills) {
                Utils.LogInfo($"Resetting kill streak of player with ID {Player.playerID}");
                SetToBaseStats();
                KillsStreak = 0;
                if(Player.data.view.IsMine) {
                    SoulsCounterGUI.GetComponentInChildren<TextMeshProUGUI>().text = SoulsString;
                }
            }
        }

        public void AddKill(int kills = 1) {
            if(CanResetKills) {
                Utils.LogInfo($"Adding {kills} kills for player with ID {Player.playerID}");
                SetToBaseStats();
                KillsStreak += kills;
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