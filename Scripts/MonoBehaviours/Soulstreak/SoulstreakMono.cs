using AALUND13Card.Armors;
using AALUND13Card.Extensions;
using AALUND13Card.MonoBehaviours.Soulstreak;
using JARL.Armor;
using JARL.Armor.Bases;
using ModdingUtils.GameModes;
using System;
using TMPro;
using UnboundLib;
using UnityEngine;

namespace AALUND13Card.MonoBehaviours {
    [Flags]
    public enum AbilityType {
        Armor = 1 << 0,
    }

    [Serializable]
    public class SoulStreakStats {
        public float
            MaxHealth = 1,
            PlayerSize = 1,
            MovementSpeed = 1,

            AttackSpeed = 1,
            Damage = 1,
            BulletSpeed = 1,

            SoulArmorPercentage = 0,
            SoulArmorPercentageRegenRate = 0,

            SoulDrainMultiply = 0;

        public AbilityType AbilityType;

        public void AddStats(SoulStreakStats soulStreakStats) {
            MaxHealth += soulStreakStats.MaxHealth;
            PlayerSize += soulStreakStats.PlayerSize;
            MovementSpeed += soulStreakStats.MovementSpeed;

            AttackSpeed += soulStreakStats.AttackSpeed;
            Damage += soulStreakStats.Damage;
            BulletSpeed += soulStreakStats.BulletSpeed;

            SoulArmorPercentage += soulStreakStats.SoulArmorPercentage;
            SoulArmorPercentageRegenRate += soulStreakStats.SoulArmorPercentageRegenRate;
            SoulDrainMultiply += soulStreakStats.SoulDrainMultiply;

            AbilityType |= soulStreakStats.AbilityType;
        }
    }

    public class SoulstreakMono : MonoBehaviour, IBattleStartHookHandler, IPointEndHookHandler {
        private Player player;
        private ArmorHandler armorHandler;

        private SoulStreakStats SoulstreakStats => player.data.GetAdditionalData().SoulStreakStats;

        private string SoulsString => $"{(Souls > 1 ? "Souls" : "Soul")}: {Souls}";
        private uint Souls {
            get => player.data.GetAdditionalData().Souls;
            set => player.data.GetAdditionalData().Souls = value;
        }

        public bool CanResetKills = true;

        public bool AbilityActive;
        public float AbilityCooldown;

        public GameObject SoulsCounter;
        public GameObject SoulsCounterGUI;

        private void Start() {
            player = gameObject.transform.parent.GetComponent<Player>();
            armorHandler = player.GetComponent<ArmorHandler>();

            SoulsCounter = Instantiate(SoulsCounter);
            if(player.data.view.IsMine && !player.GetComponent<PlayerAPI>().enabled) {
                SoulsCounterGUI = Instantiate(SoulsCounterGUI);
                SoulsCounterGUI.transform.SetParent(player.transform.parent);
            }

            player.data.SetWobbleObjectChild(SoulsCounter.transform);
            SoulsCounter.transform.localPosition = new Vector2(0, 0.3f);

            InterfaceGameModeHooksManager.instance.RegisterHooks(this);
        }

        private void OnDestroy() {
            if(player.data.view.IsMine && !player.GetComponent<PlayerAPI>().enabled) {
                Destroy(SoulsCounterGUI);
            }
            Destroy(SoulsCounter);

            if(player.gameObject.GetComponent<SoulstreakEffect>() != null) {
                Destroy(player.gameObject.GetComponent<SoulstreakEffect>());
            }

            InterfaceGameModeHooksManager.instance.RemoveHooks(this);
        }

        // Update is called once per frame
        private void Update() {
            if(player.data.isPlaying) {
                AbilityCooldown = Mathf.Max(AbilityCooldown - Time.deltaTime, 0);

                if(armorHandler.GetArmorByType(typeof(SoulArmor)).CurrentArmorValue <= 0 && armorHandler.GetArmorByType(typeof(SoulArmor)).MaxArmorValue > 0) {
                    armorHandler.GetArmorByType(typeof(SoulArmor)).MaxArmorValue = 0;
                    AbilityCooldown = 10;
                    AbilityActive = false;
                }
            }
            SoulsCounter.GetComponent<TextMeshPro>().text = SoulsString;
            if(player.data.view.IsMine && !player.GetComponent<PlayerAPI>().enabled) {
                SoulsCounterGUI.GetComponentInChildren<TextMeshProUGUI>().text = SoulsString;
            }
        }
        public void BlockAbility() {
            if(SoulstreakStats.AbilityType == AbilityType.Armor && !AbilityActive && AbilityCooldown == 0) {
                ArmorBase soulArmor = armorHandler.GetArmorByType(typeof(SoulArmor));
                soulArmor.MaxArmorValue = player.data.maxHealth * SoulstreakStats.SoulArmorPercentage * (Souls + 1);
                soulArmor.ArmorRegenerationRate = soulArmor.MaxArmorValue * SoulstreakStats.SoulArmorPercentageRegenRate;
                soulArmor.CurrentArmorValue = soulArmor.MaxArmorValue;
                AbilityActive = true;
            }
        }

        public void ResetSouls() {
            if(CanResetKills) {
                Utils.LogInfo($"Resetting kill streak of player with ID {player.playerID}");
                if(player.gameObject.GetComponent<SoulstreakEffect>() != null) {
                    Destroy(player.gameObject.GetComponent<SoulstreakEffect>());
                }
                Souls = 0;
                if(player.data.view.IsMine) {
                    SoulsCounterGUI.GetComponentInChildren<TextMeshProUGUI>().text = SoulsString;
                }
            }
        }

        public void AddSouls(uint kills = 1) {
            if(CanResetKills) {
                Utils.LogInfo($"Adding {kills} kills for player with ID {player.playerID}");

                Souls += kills;
                player.gameObject.GetOrAddComponent<SoulstreakEffect>().ApplyStats();
            }
        }

        public void OnBattleStart() {
            CanResetKills = true;
            armorHandler.GetArmorByType(typeof(SoulArmor)).MaxArmorValue = 0;
            player.gameObject.GetOrAddComponent<SoulstreakEffect>().ApplyStats();
        }

        public void OnPointEnd() {
            CanResetKills = false;
        }
    }
}