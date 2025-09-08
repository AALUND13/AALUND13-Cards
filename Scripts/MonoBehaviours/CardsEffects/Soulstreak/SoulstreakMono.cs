using AALUND13Cards.Extensions;
using AALUND13Cards.MonoBehaviours.CardsEffects.Soulstreak.Abilities;
using ModdingUtils.GameModes;
using Sonigon.Internal;
using System;
using System.Collections.Generic;
using TMPro;
using UnboundLib;
using UnityEngine;

namespace AALUND13Cards.MonoBehaviours.CardsEffects.Soulstreak {
    [Serializable]
    public class SoulStreakStats {
        // Character Stats
        public float MaxHealth = 1;
        public float PlayerSize = 1;
        public float MovementSpeed = 1;
        public float AttackSpeed = 1;
        public float Damage = 1;
        public float BulletSpeed = 1;

        // Soul Armor Stats
        public float SoulArmorPercentage = 0;
        public float SoulArmorPercentageRegenRate = 0;

        // Soul Drain Stats
        public float SoulDrainDPSFactor = 0;
        public float SoulDrainLifestealMultiply = 0;

        // Abilities
        public List<ISoulstreakAbility> Abilities = new List<ISoulstreakAbility>();


        public uint Souls = 0;
    }

    public class SoulstreakMono : MonoBehaviour, IBattleStartHookHandler, IPointEndHookHandler {
        public Player Player;
        
        public GameObject SoulsCounter;
        public GameObject SoulsCounterGUI;


        public SoulStreakStats SoulstreakStats => Player.data.GetAdditionalData().SoulStreakStats;

        private string SoulsString => $"{(Souls > 1 ? "Souls" : "Soul")}: {Souls}";
        private uint Souls {
            get => SoulstreakStats.Souls;
            set => SoulstreakStats.Souls = value;
        }
        
        private bool CanResetKills;

        public void BlockAbility() {
            foreach(ISoulstreakAbility ability in SoulstreakStats.Abilities) {
                ability.OnBlock(this);
            }
        }



        public void ResetSouls() {
            if(CanResetKills) {
                LoggerUtils.LogInfo($"Resetting kill streak of player with ID {Player.playerID}");
                if(Player.gameObject.GetComponent<SoulstreakEffect>() != null) {
                    Destroy(Player.gameObject.GetComponent<SoulstreakEffect>());
                }
                Souls = 0;
                if(Player.data.view.IsMine) {
                    SoulsCounterGUI.GetComponentInChildren<TextMeshProUGUI>().text = SoulsString;
                }
            }
        }

        public void AddSouls(uint kills = 1) {
            if(CanResetKills) {
                LoggerUtils.LogInfo($"Adding {kills} kills for player with ID {Player.playerID}");

                Souls += kills;
                Player.gameObject.GetOrAddComponent<SoulstreakEffect>().ApplyStats();
            }
        }



        public void OnBattleStart() {
            CanResetKills = true;
            Player.gameObject.GetOrAddComponent<SoulstreakEffect>().ApplyStats();
        }

        public void OnPointEnd() {
            CanResetKills = false;
        }



        private void Start() {
            Player = gameObject.transform.parent.GetComponent<Player>();

            SoulsCounter = Instantiate(SoulsCounter);
            if(Player.data.view.IsMine && !Player.GetComponent<PlayerAPI>().enabled) {
                SoulsCounterGUI = Instantiate(SoulsCounterGUI);
                SoulsCounterGUI.transform.SetParent(Player.transform.parent);
            }

            Player.data.SetWobbleObjectChild(SoulsCounter.transform);
            SoulsCounter.transform.localPosition = new Vector2(0, 0.3f);

            InterfaceGameModeHooksManager.instance.RegisterHooks(this);
            Player.data.healthHandler.reviveAction += OnRevive;
        }

        private void OnDestroy() {
            if(Player.data.view.IsMine && !Player.GetComponent<PlayerAPI>().enabled) {
                Destroy(SoulsCounterGUI);
            }
            Destroy(SoulsCounter);

            if(Player.gameObject.GetComponent<SoulstreakEffect>() != null) {
                Destroy(Player.gameObject.GetComponent<SoulstreakEffect>());
            }

            InterfaceGameModeHooksManager.instance.RemoveHooks(this);
            Player.data.healthHandler.reviveAction -= OnRevive;
        }

        private void Update() {
            if(Player.data.isPlaying) {
                foreach(ISoulstreakAbility ability in SoulstreakStats.Abilities) {
                    ability.OnUpdate(this);
                }
            }

            SoulsCounter.GetComponent<TextMeshPro>().text = SoulsString;
            if(Player.data.view.IsMine && !Player.GetComponent<PlayerAPI>().enabled) {
                SoulsCounterGUI.GetComponentInChildren<TextMeshProUGUI>().text = SoulsString;
            }
        }



        private void OnRevive() {
            foreach(ISoulstreakAbility ability in SoulstreakStats.Abilities) {
                ability.OnReset(this);
            }
        }
    }
}