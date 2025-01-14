﻿using AALUND13_Card.Utils;
using AALUND13Card.Armors;
using AALUND13Card.Extensions;
using AALUND13Card.Handlers;
using AALUND13Card.MonoBehaviours;
using BepInEx;
using BepInEx.Logging;
using ClassesManagerReborn;
using HarmonyLib;
using JARL.Armor;
using JARL.Utils;
using System.Collections.Generic;
using System.Reflection;
using UnboundLib.GameModes;
using UnboundLib.Utils;
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
        internal const string modInitials = "AAC";
        internal const string curseInitials = "AAC (Curse)";

        internal const string ModId = "com.aalund13.rounds.aalund13_cards";
        internal const string ModName = "AALUND13 Cards";
        internal const string Version = "1.5.0"; // What version are we on (major.minor.patch)?
        internal static List<BaseUnityPlugin> plugins;

        public static AssetBundle assets = Jotunn.Utils.AssetUtils.LoadAssetBundleFromResources("aalund13_cards_assets", typeof(AALUND13_Cards).Assembly);
        public static GameObject BlankCardPrefab = assets.LoadAsset<GameObject>("__AAC__Blank");

        public static CardCategory SoulstreakClassCards;

        public static AALUND13_Cards Instance { get; private set; }
        internal static ManualLogSource logger;

        public static Material PixelateEffectMaterial = assets.LoadAsset<Material>("PixelateEffectMaterial");
        public static Material ScanEffectMaterial = assets.LoadAsset<Material>("ScanEffectMaterial");

        public void Awake() {
            Instance = this;
            logger = Logger;

            ConfigHandler.RegesterMenu(Config);

            assets.LoadAsset<GameObject>("ModCards").GetComponent<CardResgester>().RegisterCards();

            ClassesRegistry.Register(CardManager.cards["__AAC__Soulstreak"].cardInfo, CardType.Entry);

            ClassesRegistry.Register(CardManager.cards["__AAC__Eternal Resilience"].cardInfo, CardType.Card, CardManager.cards["__AAC__Soulstreak"].cardInfo, 2);
            ClassesRegistry.Register(CardManager.cards["__AAC__Soulstealer Embrace"].cardInfo, CardType.Card, CardManager.cards["__AAC__Soulstreak"].cardInfo);

            ClassesRegistry.Register(CardManager.cards["__AAC__Soul Barrier"].cardInfo, CardType.SubClass, CardManager.cards["__AAC__Soulstreak"].cardInfo);
            ClassesRegistry.Register(CardManager.cards["__AAC__Soul Barrier Enhancement"].cardInfo, CardType.Card, CardManager.cards["__AAC__Soul Barrier"].cardInfo, 2);

            ClassesRegistry.Register(CardManager.cards["__AAC__Soul Drain"].cardInfo, CardType.SubClass, CardManager.cards["__AAC__Soulstreak"].cardInfo);
            ClassesRegistry.Register(CardManager.cards["__AAC__Soul Drain Enhancement"].cardInfo, CardType.Card, CardManager.cards["__AAC__Soul Drain"].cardInfo, 2);

            NegativeStatCardGenerator.Setup();

            GameModeManager.AddHook(GameModeHooks.HookPlayerPickEnd, (gm) => ExtraCardPickHandler.HandleExtraPicks());

            var harmony = new Harmony(ModId);
            harmony.PatchAll();
        }

        public void Start() {
            plugins = (List<BaseUnityPlugin>)typeof(BepInEx.Bootstrap.Chainloader).GetField("_plugins", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null);

            DeathHandler.OnPlayerDeath += OnPlayerDeath;

            ArmorFramework.RegisterArmorType(new SoulArmor());
            ArmorFramework.RegisterArmorType(new BattleforgedArmor());

            if(plugins.Exists(plugin => plugin.Info.Metadata.GUID == "com.willuwontu.rounds.tabinfo")) {
                TabinfoInterface.Setup();
            }

            new GameObject("DamageHandler").AddComponent<DelayDamageHandler>();
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

