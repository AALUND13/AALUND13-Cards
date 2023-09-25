using BepInEx;
using HarmonyLib;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClassesManagerReborn;
using UnboundLib.Cards;

[BepInDependency("com.willis.rounds.unbound")]
[BepInDependency("pykess.rounds.plugins.moddingutils")]
[BepInDependency("pykess.rounds.plugins.cardchoicespawnuniquecardpatch")]
[BepInPlugin(ModId, ModName, Version)]
[BepInProcess("Rounds.exe")]
public class AALUND13_Cards : BaseUnityPlugin
{
    internal const string modInitials = "AAC";
    internal const string ModId = "com.aalund13.rounds.AALUND13_Cards";
    internal const string ModName = "AALUND13 Cards";
    internal const string Version = "1.0.0"; // What version are we on (major.minor.patch)?

    internal static AssetBundle assets = Jotunn.Utils.AssetUtils.LoadAssetBundleFromResources("aalund13_asset", typeof(AALUND13_Cards).Assembly);
    internal static Dictionary<int, CardInfo> cardToShow = new Dictionary<int, CardInfo>();

    public static CardCategory SoulstreakClassCards;

    void Awake()
    {
        assets.LoadAsset<GameObject>("ModCards").GetComponent<CardResgester>().RegisterCards();
        var harmony = new Harmony(ModId);
        harmony.PatchAll();
        ClassesRegistry.Register(UnboundLib.Utils.CardManager.GetCardInfoWithName("__AAC__Soulstreak"), CardType.Entry);

        SoulstreakClassCards = CardChoiceSpawnUniqueCardPatch.CustomCategories.CustomCardCategories.instance.CardCategory("SoulstreakClassCards");
    }
    void Start()
    {
        UnboundLib.GameModes.GameModeManager.AddHook(UnboundLib.GameModes.GameModeHooks.HookRoundStart, (_) => showCardOnStart());

        UnboundLib.GameModes.GameModeManager.AddHook(UnboundLib.GameModes.GameModeHooks.HookPointStart, (_) => SoulstreakPlayers.omRoundStart());
        UnboundLib.GameModes.GameModeManager.AddHook(UnboundLib.GameModes.GameModeHooks.HookPointEnd, (_) => SoulstreakPlayers.omRoundEnd());
        UnboundLib.GameModes.GameModeManager.AddHook(UnboundLib.GameModes.GameModeHooks.HookGameEnd, (_) => SoulstreakPlayers.omGameEnd());
    }

    void Update()
    {
        SoulstreakPlayers.Update();
    }

    IEnumerator showCardOnStart()
    {
        
        foreach (KeyValuePair<int, CardInfo> pair in cardToShow)
        {
            int playerId = pair.Key;
            CardInfo card = pair.Value;

            yield return ModdingUtils.Utils.CardBarUtils.instance.ShowImmediate(playerId, card);
        }
        cardToShow.Clear();
        yield break;
    }
}