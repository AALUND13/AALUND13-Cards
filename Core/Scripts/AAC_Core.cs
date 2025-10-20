using AALUND13Cards.Core.Cards;
using AALUND13Cards.Core.Handlers;
using AALUND13Cards.Core.Utils;
using BepInEx;
using BepInEx.Logging;
using CardChoiceSpawnUniqueCardPatch.CustomCategories;
using HarmonyLib;
using JARL.Utils;
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

    [BepInDependency("com.willuwontu.rounds.tabinfo", BepInDependency.DependencyFlags.SoftDependency)]

    [BepInPlugin(ModId, ModName, Version)]
    [BepInProcess("Rounds.exe")]
    public class AAC_Core : BaseUnityPlugin {
        public const string ModInitials = "AAC";

        internal const string ModId = "AALUND13.Cards.Core";
        internal const string ModName = "AALUND13 Cards Core";
        internal const string Version = "1.0.0"; // What version are we on (major.minor.patch)?

        public static AAC_Core Instance { get; private set; }
        public static List<BaseUnityPlugin> Plugins;

        internal static ManualLogSource ModLogger;

        public static CardResgester CardMainResgester;

        public static CardCategory[] NoLotteryCategories;
        public static CardCategory[] NoSteelCategories;

        public void Awake() {
            Instance = this;
            ModLogger = Logger;

            new Harmony(ModId).PatchAll();

            ToggleCardsCategoriesManager.instance.RegisterCategories(ModInitials);
            ConfigHandler.RegesterMenu(Config);
        }

        public void Start() {
            Plugins = (List<BaseUnityPlugin>)typeof(BepInEx.Bootstrap.Chainloader).GetField("_plugins", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null);

            DeathHandler.OnPlayerDeath += OnPlayerDeath;

            if(Plugins.Exists(plugin => plugin.Info.Metadata.GUID == "com.willuwontu.rounds.tabinfo"))
                TabinfoInterface.Setup();

            GameModeManager.AddHook(GameModeHooks.HookPlayerPickEnd, (gm) => ExtraCardPickHandler.HandleExtraPicks(ExtraPickPhaseTrigger.TriggerInPlayerPickEnd));
            GameModeManager.AddHook(GameModeHooks.HookPickEnd, (gm) => ExtraCardPickHandler.HandleExtraPicks(ExtraPickPhaseTrigger.TriggerInPickEnd));

            gameObject.AddComponent<DelayDamageHandler>();
            gameObject.AddComponent<PickCardTracker>();
            gameObject.AddComponent<DamageEventHandler>();
            gameObject.AddComponent<ConstantDamageHandler>();

            NoLotteryCategories = new CardCategory[] { CustomCardCategories.instance.CardCategory("CardManipulation"), CustomCardCategories.instance.CardCategory("NoRandom") };
            NoSteelCategories = new CardCategory[] { CustomCardCategories.instance.CardCategory("NoRemove") };
        }

        private void OnPlayerDeath(Player player, Dictionary<Player, JARL.Utils.DamageInfo> playerDamageInfos) {
            if(player.GetComponent<DelayDamageHandler>() != null) {
                player.GetComponent<DelayDamageHandler>().StopAllCoroutines();
            }
        }
    }
}

