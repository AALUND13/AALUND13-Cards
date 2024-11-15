using AALUND13Card;
using AALUND13Card.Armors;
using AALUND13Card.MonoBehaviours;
using BepInEx;
using ClassesManagerReborn;
using HarmonyLib;
using JARL.Armor;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnboundLib.Utils;
using UnityEngine;

[BepInDependency("com.willis.rounds.unbound")]
[BepInDependency("pykess.rounds.plugins.moddingutils")]
[BepInDependency("pykess.rounds.plugins.cardchoicespawnuniquecardpatch")]
[BepInDependency("root.classes.manager.reborn")]
[BepInDependency("com.aalund13.rounds.jarl")]
[BepInDependency("com.willuwontu.rounds.tabinfo", BepInDependency.DependencyFlags.SoftDependency)]
[BepInPlugin(ModId, ModName, Version)]
[BepInProcess("Rounds.exe")]

public class AALUND13_Cards : BaseUnityPlugin {
    internal const string modInitials = "AAC";
    internal const string ModId = "com.aalund13.rounds.aalund13_cards";
    internal const string ModName = "AALUND13 Cards";
    internal const string Version = "1.0.0"; // What version are we on (major.minor.patch)?
    internal static List<BaseUnityPlugin> plugins;

    public static AssetBundle assets = Jotunn.Utils.AssetUtils.LoadAssetBundleFromResources("aalund13_cards_assets", typeof(AALUND13_Cards).Assembly);

    public static CardCategory SoulstreakClassCards;

    public static AALUND13_Cards Instance { get; private set; }

    void Awake() {
        Instance = this;

        ConfigHandler.RegesterMenu(Config);

        assets.LoadAsset<GameObject>("ModCards").GetComponent<CardResgester>().RegisterCards();

        ClassesRegistry.Register(CardManager.cards["__AAC__Soulstreak"].cardInfo, CardType.Entry);

        ClassesRegistry.Register(CardManager.cards["__AAC__Eternal Resilience"].cardInfo, CardType.Card, CardManager.cards["__AAC__Soulstreak"].cardInfo, 2);
        ClassesRegistry.Register(CardManager.cards["__AAC__Soulstealer Embrace"].cardInfo, CardType.Card, CardManager.cards["__AAC__Soulstreak"].cardInfo);

        ClassesRegistry.Register(CardManager.cards["__AAC__Soul Barrier"].cardInfo, CardType.SubClass, CardManager.cards["__AAC__Soulstreak"].cardInfo);
        ClassesRegistry.Register(CardManager.cards["__AAC__Soul Barrier Enhancement"].cardInfo, CardType.Card, CardManager.cards["__AAC__Soul Barrier"].cardInfo, 2);

        ClassesRegistry.Register(CardManager.cards["__AAC__Soul Drain"].cardInfo, CardType.SubClass, CardManager.cards["__AAC__Soulstreak"].cardInfo);
        ClassesRegistry.Register(CardManager.cards["__AAC__Soul Drain Enhancement"].cardInfo, CardType.Card, CardManager.cards["__AAC__Soul Drain"].cardInfo, 2);



        var harmony = new Harmony(ModId);
        harmony.PatchAll();
    }
    void Start() {
        plugins = (List<BaseUnityPlugin>)typeof(BepInEx.Bootstrap.Chainloader).GetField("_plugins", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null);

        UnboundLib.GameModes.GameModeManager.AddHook(UnboundLib.GameModes.GameModeHooks.HookPickStart, (_) => PickingStart());
        UnboundLib.GameModes.GameModeManager.AddHook(UnboundLib.GameModes.GameModeHooks.HookPickEnd, (_) => PickingEnd());

        UnboundLib.GameModes.GameModeManager.AddHook(UnboundLib.GameModes.GameModeHooks.HookPointStart, (_) => PointStart());
        UnboundLib.GameModes.GameModeManager.AddHook(UnboundLib.GameModes.GameModeHooks.HookPointEnd, (_) => PointEnd());

        ArmorFramework.RegisterArmorType(new SoulArmor());
        if(plugins.Exists(plugin => plugin.Info.Metadata.GUID == "com.willuwontu.rounds.tabinfo")) {
            TabinfoInterface.Setup();
        }
    }


    public IEnumerator PointStart() {
        for(int i = 0; i < PlayerManager.instance.players.Count; i++) {
            Player player = PlayerManager.instance.players[i];

            SoulstreakMono soulstreakMono = player.GetComponentInChildren<SoulstreakMono>();
            if(soulstreakMono != null) {
                soulstreakMono.CanResetKills = true;
            }
        }
        yield break;
    }

    public IEnumerator PointEnd() {
        for(int i = 0; i < PlayerManager.instance.players.Count; i++) {
            Player player = PlayerManager.instance.players[i];
            SoulstreakMono soulstreakMono = player.GetComponentInChildren<SoulstreakMono>();
            if(soulstreakMono != null) {
                soulstreakMono.CanResetKills = false;
            }
        }
        yield break;
    }

    public IEnumerator PickingEnd() {
        for(int i = 0; i < PlayerManager.instance.players.Count; i++) {
            Player player = PlayerManager.instance.players[i];
            SoulstreakMono soulstreakMono = player.GetComponentInChildren<SoulstreakMono>();
            if(soulstreakMono != null) {
                soulstreakMono.BaseCharacterData.Copy(player.data);
                soulstreakMono.SetStats();
                player.GetComponent<ArmorHandler>().GetArmorByType(typeof(SoulArmor)).MaxArmorValue = 0;
            }
        }
        yield break;
    }

    public IEnumerator PickingStart() {
        for(int i = 0; i < PlayerManager.instance.players.Count; i++) {
            Player player = PlayerManager.instance.players[i];
            SoulstreakMono soulstreakMono = player.GetComponentInChildren<SoulstreakMono>();
            if(soulstreakMono != null) {
                soulstreakMono.ResetToBase();
            }
        }
        yield break;
    }

}
