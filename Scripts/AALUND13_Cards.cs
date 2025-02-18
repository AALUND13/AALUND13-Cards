using AALUND13Card.Armors;
using AALUND13Card.Extensions;
using AALUND13Card.Handlers;
using AALUND13Card.MonoBehaviours;
using AALUND13Card.RandomStatGenerators.Generators;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using JARL;
using JARL.Armor;
using JARL.Utils;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnboundLib.GameModes;
using UnityEngine;

namespace AALUND13Card {
    [BepInDependency("com.willis.rounds.unbound")]
    [BepInDependency("pykess.rounds.plugins.moddingutils")]
    [BepInDependency("pykess.rounds.plugins.cardchoicespawnuniquecardpatch")]
    [BepInDependency("root.classes.manager.reborn")]
    [BepInDependency("com.aalund13.rounds.jarl")]
    [BepInDependency("com.willuwontu.rounds.managers")]
    [BepInDependency("com.Root.Null")]

    [BepInDependency("com.willuwontu.rounds.tabinfo", BepInDependency.DependencyFlags.SoftDependency)]

    [BepInPlugin(ModId, ModName, Version)]
    [BepInProcess("Rounds.exe")]

    public class AALUND13_Cards : BaseUnityPlugin {
        internal const string ModInitials = "AAC";
        internal const string CurseInitials = "AAC (Curse)";

        internal const string ModId = "com.aalund13.rounds.aalund13_cards";
        internal const string ModName = "AALUND13 Cards";
        internal const string Version = "1.6.3"; // What version are we on (major.minor.patch)?

        public static AALUND13_Cards Instance { get; private set; }

        internal static List<BaseUnityPlugin> Plugins;
        internal static ManualLogSource ModLogger;

        internal static AssetBundle Assets;
        public static GameObject BlankCardPrefab;

        public static CardCategory SoulstreakClassCards;

        public static Material PixelateEffectMaterial;
        public static Material ScanEffectMaterial;

        public void Awake() {
            Instance = this;
            ModLogger = Logger;

            ConfigHandler.RegesterMenu(Config);

            Assets = Jotunn.Utils.AssetUtils.LoadAssetBundleFromResources("aacardassets", typeof(AALUND13_Cards).Assembly);
            if(Assets == null) {
                throw new System.Exception("Failed to load asset bundle");
            }

            BlankCardPrefab = Assets.LoadAsset<GameObject>("__AAC__Blank");
            PixelateEffectMaterial = Assets.LoadAsset<Material>("PixelateEffectMaterial");
            ScanEffectMaterial = Assets.LoadAsset<Material>("ScanEffectMaterial");

            NegativeStatGenerator.RegisterNegativeStatGenerators();
            CorruptedStatGenerator.RegisterCorruptedStatGenerators();

            var harmony = new Harmony(ModId);
            harmony.PatchAll();
        }

        public void Start() {
            Plugins = (List<BaseUnityPlugin>)typeof(BepInEx.Bootstrap.Chainloader).GetField("_plugins", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null);

            Assets.LoadAsset<GameObject>("ModCards").GetComponent<CardResgester>().RegisterCards<AALUND13_Cards>("AAC");

            DeathHandler.OnPlayerDeath += OnPlayerDeath;

            ArmorFramework.RegisterArmorType<SoulArmor>();
            ArmorFramework.RegisterArmorType<BattleforgedArmor>();

            GameModeManager.AddHook(GameModeHooks.HookPlayerPickEnd, (gm) => ExtraCardPickHandler.HandleExtraPicks());
            GameModeManager.AddHook(GameModeHooks.HookGameStart, OnGameStart);
            GameModeManager.AddHook(GameModeHooks.HookPickStart, OnPickStart);

            if(Plugins.Exists(plugin => plugin.Info.Metadata.GUID == "com.willuwontu.rounds.tabinfo")) {
                TabinfoInterface.Setup();
            }

            CorruptedStatGenerator.BuildGlitchedCard();

            gameObject.AddComponent<DelayDamageHandler>();
        }

        IEnumerator OnGameStart(IGameModeHandler gameModeHandler) {
            foreach(Player player in PlayerManager.instance.players) {
                if(PhotonNetwork.IsMasterClient || PhotonNetwork.OfflineMode) {
                    player.data.GetAdditionalData().Souls = 0;
                    player.data.GetAdditionalData().CorruptedCardSpawnChance = 0;
                }
            }
            yield break;
        }

        IEnumerator OnPickStart(IGameModeHandler gameModeHandler) {
            foreach(Player player in PlayerManager.instance.players) {
                if(PhotonNetwork.IsMasterClient || PhotonNetwork.OfflineMode) {
                    bool isWinner = gameModeHandler.GetRoundWinners().Contains(player.teamID);
                    if(player.data.GetAdditionalData().ExtraCardPicks > 0 && !isWinner) ExtraCardPickHandler.AddExtraPick<ExtraPickHandler>(player, player.data.GetAdditionalData().ExtraCardPicks);
                }
            }
            yield break;
        }
        void OnPlayerDeath(Player player, Dictionary<Player, DamageInfo> playerDamageInfos) {
            if(player.GetComponent<DelayDamageHandler>() != null) {
                player.GetComponent<DelayDamageHandler>().StopAllCoroutines();
            }
            foreach(var playerDamageInfo in playerDamageInfos) {
                if(playerDamageInfo.Value.TimeSinceLastDamage <= 5 && playerDamageInfo.Key.GetComponentInChildren<SoulstreakMono>() != null && !playerDamageInfo.Key.data.dead) {
                    playerDamageInfo.Key.GetComponentInChildren<SoulstreakMono>().AddSouls();

                    if(player.GetComponentInChildren<SoulstreakMono>() != null) {
                        playerDamageInfo.Key.GetComponentInChildren<SoulstreakMono>().AddSouls((uint)(player.data.GetAdditionalData().Souls * 0.5f));
                    }
                }
            }

            player.GetComponentInChildren<SoulstreakMono>()?.ResetSouls();
            if(player.GetComponentInChildren<SoulstreakMono>() == null) {
                player.data.GetAdditionalData().Souls = 0;
            }
        }
    }
}

