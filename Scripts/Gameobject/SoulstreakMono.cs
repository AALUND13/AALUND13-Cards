using AALUND13Card.Armors;
using AALUND13Card.Extensions;
using HarmonyLib;
using JARL.Armor;
using JARL.Armor.Bases;
using System;
using TMPro;
using UnityEngine;

namespace AALUND13Card.MonoBehaviours {
    [Flags]
    public enum AbilityType {
        armor = 1,
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
        public AbilityType AbilityType;

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

            AbilityType = soulStreakStats.AbilityType;
        }
    }

    public class SoulstreakMono : MonoBehaviour {
        private SoulStreakStats SoulstreakStats => Player.data.GetAdditionalData().SoulStreakStats;
        private string SoulsString => $"{(KillStreak > 1 ? "Souls" : "Soul")}: {KillStreak}";
        private uint KillStreak {
            get => Player.data.GetAdditionalData().Killstreak;
            set => Player.data.GetAdditionalData().Killstreak = value;
        }

        public bool CanResetKills = true;

        public bool AbilityActive = false;
        public float AbilityCooldown = 0;

        public Player Player = null;
        
        private bool AlreadySetBaseStats = true;
        public PlayerStats BaseCharacterData;

        public GameObject SoulsCounter = null;
        public GameObject SoulsCounterGUI = null;

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
                soulArmor.MaxArmorValue = Player.data.maxHealth * SoulstreakStats.SoulArmorPercentage * (KillStreak + 1);
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
            if(KillStreak > 0 && !AlreadySetBaseStats) {
                AlreadySetBaseStats = true;
                Player.data.weaponHandler.gun.damage *= 1f / (1 + (SoulstreakStats.DamageMultiplyPerKill - 1) * KillStreak);
                Player.data.weaponHandler.gun.attackSpeed *= 1f / (1 + (SoulstreakStats.ATKSpeedMultiplyPerKill - 1) * KillStreak);
                Player.data.stats.movementSpeed *= 1f / (1 + (SoulstreakStats.MovementSpeedMultiplyPerKill - 1) * KillStreak);
                Player.data.block.cooldown *= 1f / (1 + (SoulstreakStats.BlockCooldownMultiplyPerKill - 1) * KillStreak);
                Player.data.maxHealth *= 1f / (1 + (SoulstreakStats.HealthMultiplyPerKill - 1) * KillStreak);
                Player.data.health *= 1f / (1 + (SoulstreakStats.HealthMultiplyPerKill - 1) * KillStreak);

                AccessTools.Method(typeof(CharacterStatModifiers), "ConfigureMassAndSize").Invoke(Player.data.stats, null);
            }
        }

        public void SetStats() {
            if(KillStreak > 0 && AlreadySetBaseStats) {
                AlreadySetBaseStats = false;
                Player.data.weaponHandler.gun.damage *= 1 + (SoulstreakStats.DamageMultiplyPerKill - 1) * KillStreak;
                Player.data.weaponHandler.gun.attackSpeed *= 1 + (SoulstreakStats.ATKSpeedMultiplyPerKill - 1) * KillStreak;
                Player.data.stats.movementSpeed *= 1 + (SoulstreakStats.MovementSpeedMultiplyPerKill - 1) * KillStreak;
                Player.data.block.cooldown *= 1 + (SoulstreakStats.BlockCooldownMultiplyPerKill - 1) * KillStreak;
                Player.data.maxHealth *= 1 + (SoulstreakStats.HealthMultiplyPerKill - 1) * KillStreak;
                Player.data.health *= 1 + (SoulstreakStats.HealthMultiplyPerKill - 1) * KillStreak;

                AccessTools.Method(typeof(CharacterStatModifiers), "ConfigureMassAndSize").Invoke(Player.data.stats, null);
            }

            Player.data.healthHandler.Heal(Player.data.maxHealth * SoulstreakStats.HealPercentagePerKill);
        }

        public void ResetKill() {
            if(CanResetKills) {
                Utils.LogInfo($"Resetting kill streak of player with ID {Player.playerID}");
                SetToBaseStats();
                KillStreak = 0;
                if(Player.data.view.IsMine) {
                    SoulsCounterGUI.GetComponentInChildren<TextMeshProUGUI>().text = SoulsString;
                }
            }
        }

        public void AddKill(uint kills = 1) {
            if(CanResetKills) {
                Utils.LogInfo($"Adding {kills} kills for player with ID {Player.playerID}");
                SetToBaseStats();
                KillStreak += kills;
                SetStats();
            }
        }
    }
}