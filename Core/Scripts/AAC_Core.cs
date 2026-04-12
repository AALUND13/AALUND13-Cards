using AALUND13Cards.Core.Cards;
using AALUND13Cards.Core.Handlers;
using AALUND13Cards.Core.Patches;
using AALUND13Cards.Core.Utils;
using BepInEx;
using BepInEx.Logging;
using CardChoiceSpawnUniqueCardPatch.CustomCategories;
using HarmonyLib;
using JARL.Utils;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using ToggleCardsCategories;
using UnboundLib.GameModes;

namespace AALUND13Cards.Core {
    [BepInDependency("com.willis.rounds.unbound")]
    [BepInDependency("pykess.rounds.plugins.moddingutils")]
    [BepInDependency("pykess.rounds.plugins.cardchoicespawnuniquecardpatch")]
    [BepInDependency("root.classes.manager.reborn")]
    [BepInDependency("com.aalund13.rounds.jarl")]
    [BepInDependency("com.willuwontu.rounds.managers")]
    [BepInDependency("com.aalund13.rounds.toggle_cards_categories")]
    [BepInDependency("Systems.R00t.PickPhaseImprovements")]

    [BepInDependency("com.willuwontu.rounds.tabinfo", BepInDependency.DependencyFlags.SoftDependency)]
    [BepInDependency("io.olavim.rounds.rwf", BepInDependency.DependencyFlags.SoftDependency)]

    [BepInPlugin(ModId, ModName, Version)]
    [BepInProcess("Rounds.exe")]
    public class AAC_Core : BaseUnityPlugin {
        public const string ModInitials = "AAC";

        internal const string ModId = "AALUND13.Cards.Core";
        internal const string ModName = "AALUND13 Cards Core";
        internal const string Version = "1.2.1"; // What version are we on (major.minor.patch)?
        internal const string FullVersion = "2.2.0"; // What version are we on (major.minor.patch)?
        internal const bool IsBeta = false;

        public static AAC_Core Instance { get; private set; }
        public static List<BaseUnityPlugin> Plugins;

        internal static ManualLogSource ModLogger;
        internal static Harmony Harmony;

        public static CardResgester CardMainResgester;

        public static CardCategory[] NoLotteryCategories;
        public static CardCategory[] NoSteelCategories;

        public void Awake() {
            Instance = this;
            ModLogger = Logger;

            Harmony = new Harmony(ModId);
            Harmony.PatchAll();

            ToggleCardsCategoriesManager.instance.RegisterCategories(ModInitials);
            AACMenu.RegesterMenu(Config);
        }

        public void Start() {
            Plugins = (List<BaseUnityPlugin>)typeof(BepInEx.Bootstrap.Chainloader).GetField("_plugins", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null);

            DeathHandler.OnPlayerDeath += OnPlayerDeath;

            if(Plugins.Exists(plugin => plugin.Info.Metadata.GUID == "com.willuwontu.rounds.tabinfo"))
                TabinfoInterface.Setup();

            CardBarHandlerExtensionsPatch.Patch(Harmony);

            GameModeManager.AddHook(GameModeHooks.HookGameStart, OnGameStart);

            gameObject.AddComponent<DelayDamageHandler>();
            gameObject.AddComponent<PickCardTracker>();
            gameObject.AddComponent<DamageEventHandler>();
            gameObject.AddComponent<DeathActionHandler>();
            gameObject.AddComponent<ConstantDamageHandler>();

            NoLotteryCategories = new CardCategory[] { CustomCardCategories.instance.CardCategory("CardManipulation"), CustomCardCategories.instance.CardCategory("NoRandom") };
            NoSteelCategories = new CardCategory[] { CustomCardCategories.instance.CardCategory("NoRemove") };
        }

        private IEnumerator OnGameStart(IGameModeHandler gm) {
            ConstantDamageHandler.Instance.Reset();
            yield break;
        }

        private void OnPlayerDeath(Player player, Dictionary<Player, JARL.Utils.DamageInfo> playerDamageInfos) {
            if(player.GetComponent<DelayDamageHandler>() != null) {
                player.GetComponent<DelayDamageHandler>().StopAllCoroutines();
            }
        }
    }
}

