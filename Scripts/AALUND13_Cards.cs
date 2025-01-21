using AALUND13Card.Armors;
using AALUND13Card.Extensions;
using AALUND13Card.Handlers;
using AALUND13Card.MonoBehaviours;
using AALUND13Card.RandomStatGenerators.Generators;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using JARL.Armor;
using JARL.Utils;
using System.Collections.Generic;
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

    [BepInDependency("com.willuwontu.rounds.tabinfo", BepInDependency.DependencyFlags.SoftDependency)]
    [BepInPlugin(ModId, ModName, Version)]
    [BepInProcess("Rounds.exe")]

    public class AALUND13_Cards : BaseUnityPlugin {
        internal const string ModInitials = "AAC";
        internal const string CurseInitials = "AAC (Curse)";

        internal const string ModId = "com.aalund13.rounds.aalund13_cards";
        internal const string ModName = "AALUND13 Cards";
        internal const string Version = "1.5.1"; // What version are we on (major.minor.patch)?

        public static AALUND13_Cards Instance { get; private set; }

        internal static List<BaseUnityPlugin> Plugins;
        internal static ManualLogSource ModLogger;

        public static AssetBundle Assets;
        public static GameObject BlankCardPrefab;

        public static CardCategory SoulstreakClassCards;

        public static Material PixelateEffectMaterial;
        public static Material ScanEffectMaterial;

        public void Awake() {
            Instance = this;
            ModLogger = Logger;

            ConfigHandler.RegesterMenu(Config);

            Assets = Jotunn.Utils.AssetUtils.LoadAssetBundleFromResources("aalund13_cards_assets", typeof(AALUND13_Cards).Assembly);

            BlankCardPrefab = Assets.LoadAsset<GameObject>("__AAC__Blank");
            PixelateEffectMaterial = Assets.LoadAsset<Material>("PixelateEffectMaterial");
            ScanEffectMaterial = Assets.LoadAsset<Material>("ScanEffectMaterial");

            Assets.LoadAsset<GameObject>("ModCards").GetComponent<CardResgester>().RegisterCards();

            NegativeStatGenerator.RegisterNegativeStatGenerators();
            GlitchedStatGenerator.RegisterGlitchedStatGenerators();

            var harmony = new Harmony(ModId);
            harmony.PatchAll();
        }

        public void Start() {
            Plugins = (List<BaseUnityPlugin>)typeof(BepInEx.Bootstrap.Chainloader).GetField("_plugins", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null);

            DeathHandler.OnPlayerDeath += OnPlayerDeath;

            ArmorFramework.RegisterArmorType(new SoulArmor());
            ArmorFramework.RegisterArmorType(new BattleforgedArmor());

            GameModeManager.AddHook(GameModeHooks.HookPlayerPickEnd, (gm) => ExtraCardPickHandler.HandleExtraPicks());

            if(Plugins.Exists(plugin => plugin.Info.Metadata.GUID == "com.willuwontu.rounds.tabinfo")) {
                TabinfoInterface.Setup();
            }

            GlitchedStatGenerator.BuildGlitchedCard();

            gameObject.AddComponent<DelayDamageHandler>();
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
        }
    }
}

