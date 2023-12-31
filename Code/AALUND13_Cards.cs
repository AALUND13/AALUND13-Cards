﻿using BepInEx;
using HarmonyLib;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClassesManagerReborn;
using System.Reflection;

[BepInDependency("com.willis.rounds.unbound")]
[BepInDependency("pykess.rounds.plugins.moddingutils")]
[BepInDependency("pykess.rounds.plugins.cardchoicespawnuniquecardpatch")]
[BepInDependency("root.classes.manager.reborn")]
[BepInDependency("com.willuwontu.rounds.tabinfo", BepInDependency.DependencyFlags.SoftDependency)]
[BepInPlugin(ModId, ModName, Version)]
[BepInProcess("Rounds.exe")]

public class AALUND13_Cards : BaseUnityPlugin
{
    internal const string modInitials = "AAC";
    internal const string ModId = "com.aalund13.rounds.AALUND13_Cards";
    internal const string ModName = "AALUND13 Cards";
    internal const string Version = "0.2.0"; // What version are we on (major.minor.patch)?
    internal static List<BaseUnityPlugin> plugins;

    internal static AssetBundle assets = Jotunn.Utils.AssetUtils.LoadAssetBundleFromResources("aalund13_asset", typeof(AALUND13_Cards).Assembly);

    public static CardCategory SoulstreakClassCards;

    void Awake()
    {
        assets.LoadAsset<GameObject>("ModCards").GetComponent<CardResgester>().RegisterCards();

        RerollClassManager.instance = new GameObject("RerollClassManager").AddComponent<RerollClassManager>();

        var harmony = new Harmony(ModId);
        harmony.PatchAll();
        ClassesRegistry.Register(CardResgester.ModCards["Soulstreak"].GetComponent<CardInfo>(), CardType.Entry);
        ClassesRegistry.Register(CardResgester.ModCards["Spiritual Defense"].GetComponent<CardInfo>(), CardType.Card, CardResgester.ModCards["Soulstreak"].GetComponent<CardInfo>(), 2);
        ClassesRegistry.Register(CardResgester.ModCards["Eternal Resilience"].GetComponent<CardInfo>(), CardType.Card, CardResgester.ModCards["Soulstreak"].GetComponent<CardInfo>(), 2);
        ClassesRegistry.Register(CardResgester.ModCards["Soul Surge"].GetComponent<CardInfo>(), CardType.Card, CardResgester.ModCards["Soulstreak"].GetComponent<CardInfo>());
        ClassesRegistry.Register(CardResgester.ModCards["Soulbound Sprint"].GetComponent<CardInfo>(), CardType.Card, CardResgester.ModCards["Soulstreak"].GetComponent<CardInfo>(), 2);
        ClassesRegistry.Register(CardResgester.ModCards["Soulstealer Embrace"].GetComponent<CardInfo>(), CardType.Card, CardResgester.ModCards["Soulstreak"].GetComponent<CardInfo>());
        ClassesRegistry.Register(CardResgester.ModCards["Soulswift"].GetComponent<CardInfo>(), CardType.Card, CardResgester.ModCards["Soulstreak"].GetComponent<CardInfo>(), 2);
        ClassesRegistry.Register(CardResgester.ModCards["Soul Shield"].GetComponent<CardInfo>(), CardType.Card, CardResgester.ModCards["Soulstreak"].GetComponent<CardInfo>());
    }
    void Start()
    {
        plugins = (List<BaseUnityPlugin>)typeof(BepInEx.Bootstrap.Chainloader).GetField("_plugins", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null);

        UnboundLib.GameModes.GameModeManager.AddHook(UnboundLib.GameModes.GameModeHooks.HookPickStart, (_) => PickingStart());
        UnboundLib.GameModes.GameModeManager.AddHook(UnboundLib.GameModes.GameModeHooks.HookPickEnd, (_) => PickingEnd());
        UnboundLib.GameModes.GameModeManager.AddHook(UnboundLib.GameModes.GameModeHooks.HookPointStart, (_) => PointStart());
        if (plugins.Exists(plugin => plugin.Info.Metadata.GUID == "com.willuwontu.rounds.tabinfo"))
        {         
            TabinfoInterface.Setup();
        }
    }

    IEnumerator PointStart()
    {
        for (int i = 0; i < PlayerManager.instance.players.Count; i++)
        {
            Player player = PlayerManager.instance.players[i];
            SoulstreakObject soulstreakObject = player.GetComponentInChildren<SoulstreakObject>();
            if (soulstreakObject != null)
            {
                soulstreakObject.RegenSoulShield();
            }
        }

        yield break;
    }

    IEnumerator PickingEnd()
    {
        for (int i = 0; i < PlayerManager.instance.players.Count; i++)
        {
            Player player = PlayerManager.instance.players[i];
            SoulstreakObject soulstreakObject = player.GetComponentInChildren<SoulstreakObject>();
            if (soulstreakObject != null)
            {
                soulstreakObject.SetStats();
            }
        }

        //yield return RerollClassManager.instance.RerollPlayer();
           
        yield break;
    }

    IEnumerator PickingStart()
    {
        for (int i = 0; i < PlayerManager.instance.players.Count; i++)
        {
            Player player = PlayerManager.instance.players[i];
            SoulstreakObject soulstreakObject = player.GetComponentInChildren<SoulstreakObject>();
            if (soulstreakObject != null)
            {
                soulstreakObject.SetToBaseStats();
            }
        }
        yield break;
    }

}
