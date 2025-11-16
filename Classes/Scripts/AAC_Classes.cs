using AALUND13Cards.Classes;
using AALUND13Cards.Classes.Cards;
using AALUND13Cards.Classes.MonoBehaviours.CardsEffects.Soulstreak;
using AALUND13Cards.Core;
using AALUND13Cards.Core.Cards;
using AALUND13Cards.Core.Extensions;
using AALUND13Cards.Core.Utils;
using BepInEx;
using HarmonyLib;
using JARL.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnboundLib;
using UnboundLib.GameModes;
using UnityEngine;

namespace AALUND13Cards.ExtraCards {
    [BepInDependency("AALUND13.Cards.Core")]
    [BepInDependency("AALUND13.Cards.Armors", BepInDependency.DependencyFlags.SoftDependency)]

    [BepInPlugin(ModId, ModName, Version)]
    [BepInProcess("Rounds.exe")]
    internal class AAC_Classes : BaseUnityPlugin {
        internal const string ModId = "AALUND13.Cards.Classes";
        internal const string ModName = "AALUND13 Classes Cards";
        internal const string Version = "1.1.1";

        private static AssetBundle assets;

        private void Awake() {
            assets = AssetsUtils.LoadAssetBundle("aac_classes_assets", typeof(AAC_Classes).Assembly);

            if(assets != null) {
                new Harmony(ModId).PatchAll();
            }
        }

        private void Start() {
            if(assets == null) {
                Unbound.BuildModal("AALUND13 Cards Error", $"The mod \"{ModName}\" assets failled to load, All the cards will be disable in this mod");
                throw new NullReferenceException($"Failled to load \"{ModName}\" assets");
            }

            if(AAC_Core.Plugins.Exists(plugin => plugin.Info.Metadata.GUID == "com.willuwontu.rounds.tabinfo"))
                TabinfoInterface.Setup();
            if(AAC_Core.Plugins.Exists(plugin => plugin.Info.Metadata.GUID == "AALUND13.Cards.Armors"))
                ArmorInterface.RegisterArmors();

            CardResgester cardResgester = assets.LoadAsset<GameObject>("ClassesModCards").GetComponent<CardResgester>();
            cardResgester.RegisterCards();
            AACMenu.OnMenuRegister += () => AACMenu.CreateModuleMenuWithReadmeGenerator(ModName, Version, cardResgester);

            DeathHandler.OnPlayerDeath += OnPlayerDeath;

            GameModeManager.AddHook(GameModeHooks.HookGameStart, OnGameStart);
        }

        private IEnumerator OnGameStart(IGameModeHandler gameModeHandler) {
            foreach(Player player in PlayerManager.instance.players) {
                player.data.GetAdditionalData().CustomStatsRegistry.GetOrCreate<SoulStreakStats>().Souls = 0;
            }

            yield break;
        }

        private void OnPlayerDeath(Player player, Dictionary<Player, JARL.Utils.DamageInfo> playerDamageInfos) {
            foreach(var playerDamageInfo in playerDamageInfos) {
                if(playerDamageInfo.Value.TimeSinceLastDamage <= 5 && playerDamageInfo.Key.GetComponentInChildren<SoulstreakMono>() != null && !playerDamageInfo.Key.data.dead) {
                    playerDamageInfo.Key.GetComponentInChildren<SoulstreakMono>().AddSouls();

                    if(player.GetComponentInChildren<SoulstreakMono>() != null) {
                        playerDamageInfo.Key.GetComponentInChildren<SoulstreakMono>().AddSouls((uint)(player.data.GetAdditionalData().CustomStatsRegistry.GetOrCreate<SoulStreakStats>().Souls * 0.5f));
                    }
                }
            }

            player.GetComponentInChildren<SoulstreakMono>()?.ResetSouls();
            if(player.GetComponentInChildren<SoulstreakMono>() == null) {
                player.data.GetAdditionalData().CustomStatsRegistry.GetOrCreate<SoulStreakStats>().Souls = 0;
            }
        }
    }
}