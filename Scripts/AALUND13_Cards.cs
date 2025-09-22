using AALUND13Cards.Armors;
using AALUND13Cards.Armors.Processors;
using AALUND13Cards.Cards;
using AALUND13Cards.Extensions;
using AALUND13Cards.Handlers;
using AALUND13Cards.MonoBehaviours.CardsEffects.Soulstreak;
using AALUND13Cards.MonoBehaviours.UI;
using AALUND13Cards.Utils;
using BepInEx;
using BepInEx.Logging;
using CardChoiceSpawnUniqueCardPatch.CustomCategories;
using HarmonyLib;
using JARL.Armor;
using JARL.Utils;
using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ToggleCardsCategories;
using UnboundLib;
using UnboundLib.GameModes;
using UnboundLib.Utils.UI;
using UnityEngine;

namespace AALUND13Cards {
    [BepInDependency("com.willis.rounds.unbound")]
    [BepInDependency("pykess.rounds.plugins.moddingutils")]
    [BepInDependency("pykess.rounds.plugins.cardchoicespawnuniquecardpatch")]
    [BepInDependency("root.classes.manager.reborn")]
    [BepInDependency("com.aalund13.rounds.jarl")]
    [BepInDependency("com.willuwontu.rounds.managers")]
    [BepInDependency("com.aalund13.rounds.toggle_cards_categories")]

    [BepInDependency("com.willuwontu.rounds.tabinfo", BepInDependency.DependencyFlags.SoftDependency)]

    [BepInPlugin(ModId, ModName, Version)]
    [BepInProcess("Rounds.exe")]
    public class AALUND13_Cards : BaseUnityPlugin {
        internal const string ModInitials = "AAC";
        internal const string CurseInitials = "AAC (Curse)";

        internal const string ModId = "com.aalund13.rounds.aalund13_cards";
        internal const string ModName = "AALUND13 Cards";
        internal const string Version = "2.0.0"; // What version are we on (major.minor.patch)?
        
        public static AALUND13_Cards Instance { get; private set; }

        internal static List<BaseUnityPlugin> Plugins;
        internal static ManualLogSource ModLogger;
        
        internal static AssetBundle MainAssets;
        internal static AssetBundle ShaderAssets;

        public static CardResgester CardMainResgester;
        public static CardResgester CardShaderResgester;

        public static CardCategory SoulstreakClassCards;

        public static CardCategory[] NoLotteryCategories;
        public static CardCategory[] NoSteelCategories;

        public void Awake() {
            Instance = this;
            ModLogger = Logger;

            ConfigHandler.RegesterMenu(Config);

            MainAssets = Jotunn.Utils.AssetUtils.LoadAssetBundleFromResources("aacard_main_assets", typeof(AALUND13_Cards).Assembly);
            ShaderAssets = Jotunn.Utils.AssetUtils.LoadAssetBundleFromResources("aacard_shader_assets", typeof(AALUND13_Cards).Assembly);
            
            if(MainAssets != null) {
                AACardsGenerators.RegisterGenerators();
                ToggleCardsCategoriesManager.instance.RegisterCategories(ModInitials);

                var harmony = new Harmony(ModId);
                harmony.PatchAll();
            }
        }

        public void Start() {
            Plugins = (List<BaseUnityPlugin>)typeof(BepInEx.Bootstrap.Chainloader).GetField("_plugins", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null);

            if(MainAssets == null) {
                Unbound.BuildModal("AALUND13 Cards Error", "The mod \"AALUND13 Cards\" main asset failled to load, All the cards will be disable");
                throw new NullReferenceException("Failled to load \"AALUND13 Cards\" main assets");
            } else if(ShaderAssets == null) {
                Unbound.BuildModal("AALUND13 Cards Error", "The mod \"AALUND13 Cards\" shader asset failled to load, Some the cards will be disable\nBut not all.");
                Logger.LogWarning("Failled to load \"AALUND13 Cards\" shader assets");
            }
            
            CardMainResgester = MainAssets.LoadAsset<GameObject>("ModMainCards").GetComponent<CardResgester>();
            CardMainResgester.RegisterCards();

            MainAssets.LoadAsset<GameObject>("PhotonPrefabPool").GetComponent<PhotonPrefabPool>().RegisterPrefabs();
            
            if(ShaderAssets != null) {
                CardShaderResgester = ShaderAssets.LoadAsset<GameObject>("ModShaderCards").GetComponent<CardResgester>();
                CardShaderResgester.RegisterCards();
                
                GameObject flashlightMaskHandler = GameObject.Instantiate(ShaderAssets.LoadAsset<GameObject>("FlashlightMaskHandler"));
                DontDestroyOnLoad(flashlightMaskHandler);
            }

            DeathHandler.OnPlayerDeath += OnPlayerDeath;

            ArmorFramework.RegisterArmorType<SoulArmor>();
            ArmorFramework.RegisterArmorType<TitaniumArmor>();
            ArmorFramework.RegisterArmorType<BattleforgedArmor>();
            ArmorFramework.RegisterArmorType<ExoArmor>();

            ArmorFramework.RegisterArmorProcessor<DamageAgainstArmorPercentageProcessor>();
            ArmorFramework.RegisterArmorProcessor<ArmorDamageReductionProcessor>();

            GameModeManager.AddHook(GameModeHooks.HookPlayerPickEnd, (gm) => ExtraCardPickHandler.HandleExtraPicks(ExtraPickPhaseTrigger.TriggerInPlayerPickEnd));
            GameModeManager.AddHook(GameModeHooks.HookPickEnd, (gm) => ExtraCardPickHandler.HandleExtraPicks(ExtraPickPhaseTrigger.TriggerInPickEnd));
            GameModeManager.AddHook(GameModeHooks.HookGameStart, OnGameStart);
            GameModeManager.AddHook(GameModeHooks.HookPickStart, OnPickStart);

            if(Plugins.Exists(plugin => plugin.Info.Metadata.GUID == "com.willuwontu.rounds.tabinfo")) {
                TabinfoInterface.Setup();
            }

            gameObject.AddComponent<DelayDamageHandler>();
            gameObject.AddComponent<PickCardTracker>();
            gameObject.AddComponent<DamageEventHandler>();
            gameObject.AddComponent<ConstantDamageHandler>();

            NoLotteryCategories = new CardCategory[] { CustomCardCategories.instance.CardCategory("CardManipulation"), CustomCardCategories.instance.CardCategory("NoRandom") };
            NoSteelCategories = new CardCategory[] { CustomCardCategories.instance.CardCategory("NoRemove") };
        }

        IEnumerator OnGameStart(IGameModeHandler gameModeHandler) {
            foreach(Player player in PlayerManager.instance.players) {
                if(PhotonNetwork.IsMasterClient || PhotonNetwork.OfflineMode) {
                    player.data.GetAdditionalData().SoulStreakStats.Souls = 0;
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

        void OnPlayerDeath(Player player, Dictionary<Player, JARL.Utils.DamageInfo> playerDamageInfos) {
            if(player.GetComponent<DelayDamageHandler>() != null) {
                player.GetComponent<DelayDamageHandler>().StopAllCoroutines();
            }
            foreach(var playerDamageInfo in playerDamageInfos) {
                if(playerDamageInfo.Value.TimeSinceLastDamage <= 5 && playerDamageInfo.Key.GetComponentInChildren<SoulstreakMono>() != null && !playerDamageInfo.Key.data.dead) {
                    playerDamageInfo.Key.GetComponentInChildren<SoulstreakMono>().AddSouls();
                    
                    if(player.GetComponentInChildren<SoulstreakMono>() != null) {
                        playerDamageInfo.Key.GetComponentInChildren<SoulstreakMono>().AddSouls((uint)(player.data.GetAdditionalData().SoulStreakStats.Souls * 0.5f));
                    }
                }
            }

            player.GetComponentInChildren<SoulstreakMono>()?.ResetSouls();
            if(player.GetComponentInChildren<SoulstreakMono>() == null) {
                player.data.GetAdditionalData().SoulStreakStats.Souls = 0;
            }
        }
    }
}

