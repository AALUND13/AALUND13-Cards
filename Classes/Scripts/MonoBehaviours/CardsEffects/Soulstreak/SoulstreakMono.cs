using AALUND13Cards.Classes.Cards;
using AALUND13Cards.Classes.MonoBehaviours.CardsEffects.Soulstreak.Abilities;
using AALUND13Cards.Core;
using AALUND13Cards.Core.Extensions;
using AALUND13Cards.Core.Utils;
using ModdingUtils.GameModes;
using System;
using System.Collections.Generic;
using TMPro;
using UnboundLib;
using UnityEngine;

namespace AALUND13Cards.Classes.MonoBehaviours.CardsEffects.Soulstreak {
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