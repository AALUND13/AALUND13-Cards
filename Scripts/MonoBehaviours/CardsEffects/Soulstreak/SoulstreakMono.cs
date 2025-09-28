using AALUND13Cards.Extensions;
using AALUND13Cards.MonoBehaviours.CardsEffects.Soulstreak.Abilities;
using AALUND13Cards.Utils;
using ModdingUtils.GameModes;
using Sonigon.Internal;
using System;
using System.Collections.Generic;
using TMPro;
using UnboundLib;
using UnityEngine;

namespace AALUND13Cards.MonoBehaviours.CardsEffects.Soulstreak {
    [Serializable]
    public class SoulStreakStats : ICustomStats {
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

        public void ResetStats() {
            // Character Stats
            MaxHealth = 1;
            PlayerSize = 1;
            MovementSpeed = 1;
            AttackSpeed = 1;
            Damage = 1;
            BulletSpeed = 1;

            // Soul Armor Stats
            SoulArmorPercentage = 0;
            SoulArmorPercentageRegenRate= 0;

            // Abilities
            Abilities.Clear();
        }
    }

    public class SoulstreakMono : MonoBehaviour, IBattleStartHookHandler {
        public GameObject SoulsCounter;
        public GameObject SoulsCounterGUI;
        public SoulStreakStats SoulstreakStats;

        public CharacterData Data => data;
        private CharacterData data;

        private string SoulsString => $"{(SoulstreakStats.Souls > 1 ? "Souls" : "Soul")}: {SoulstreakStats.Souls}";

        public void BlockAbility() {
            foreach(ISoulstreakAbility ability in SoulstreakStats.Abilities) {
                ability.OnBlock(this);
            }
        }

        public void ResetSouls() {
            if(GameManager.instance.battleOngoing) {
                LoggerUtils.LogInfo($"Resetting kill streak of player with ID {data.player.playerID}");
                if(data.gameObject.GetComponent<SoulstreakEffect>() != null) {
                    Destroy(data.gameObject.GetComponent<SoulstreakEffect>());
                }
                SoulstreakStats.Souls = 0;
                if(data.view.IsMine) {
                    SoulsCounterGUI.GetComponentInChildren<TextMeshProUGUI>().text = SoulsString;
                }
            }
        }

        public void AddSouls(uint kills = 1) {
            if(GameManager.instance.battleOngoing) {
                LoggerUtils.LogInfo($"Adding {kills} kills for player with ID {data.player.playerID}");

                SoulstreakStats.Souls += kills;
                data.gameObject.GetOrAddComponent<SoulstreakEffect>().ApplyStats();
            }
        }

        public void OnBattleStart() {
            data.gameObject.GetOrAddComponent<SoulstreakEffect>().ApplyStats();
        }



        private void OnRevive() {
            foreach(ISoulstreakAbility ability in SoulstreakStats.Abilities) {
                ability.OnReset(this);
            }
        }



        private void Start() {
            data = GetComponentInParent<Player>().data;
            SoulstreakStats = data.GetAdditionalData().CustomStatsRegistry.GetOrCreate<SoulStreakStats>();

            SoulsCounter = Instantiate(SoulsCounter);
            if(data.view.IsMine && !data.GetComponent<PlayerAPI>().enabled) {
                SoulsCounterGUI = Instantiate(SoulsCounterGUI);
                SoulsCounterGUI.transform.SetParent(data.transform.parent);
            }

            data.SetWobbleObjectChild(SoulsCounter.transform);
            SoulsCounter.transform.localPosition = new Vector2(0, 0.3f);

            InterfaceGameModeHooksManager.instance.RegisterHooks(this);
            data.healthHandler.reviveAction += OnRevive;
        }

        private void OnDestroy() {
            if(data.view.IsMine && !data.GetComponent<PlayerAPI>().enabled) {
                Destroy(SoulsCounterGUI);
            }
            Destroy(SoulsCounter);
            
            if(data.gameObject.GetComponent<SoulstreakEffect>() != null) {
                Destroy(data.gameObject.GetComponent<SoulstreakEffect>());
            }

            InterfaceGameModeHooksManager.instance.RemoveHooks(this);
            data.healthHandler.reviveAction -= OnRevive;
        }

        private void Update() {
            if(data.isPlaying) {
                foreach(ISoulstreakAbility ability in SoulstreakStats.Abilities) {
                    ability.OnUpdate(this);
                }
            }

            SoulsCounter.GetComponent<TextMeshPro>().text = SoulsString;
            if(data.view.IsMine && !data.GetComponent<PlayerAPI>().enabled) {
                SoulsCounterGUI.GetComponentInChildren<TextMeshProUGUI>().text = SoulsString;
            }
        }
    }
}