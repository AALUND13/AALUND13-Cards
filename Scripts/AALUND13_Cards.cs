using AALUND13Card.Armors;
using AALUND13Card.Extensions;
using AALUND13Card.Handlers;
using AALUND13Card.MonoBehaviours;
using AALUND13Card.Utils.RandomStatsGenerator;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using JARL.Armor;
using JARL.Utils;
using RarityLib.Utils;
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
        public static List<CardInfo> GlitchedCards = new List<CardInfo>();

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

            // Common stats
            new RandomStatHandler("GlitchedStatGeneratorCommon", new List<RandomStatGenerator> {
                new DamageStatGenerator(-0.3f, 0.3f),
                new ReloadTimeStatGenerator(-0.3f, 0.3f),
                new AttackSpeedStatGenerator(-0.3f, 0.3f),
                new MovementSpeedStatGenerator(-0.1f, 0.1f),
                new HealthStatGenerator(-0.3f, 0.3f),
                new BlockCooldownStatGenerator(-0.3f, 0.3f),
                new BulletSpeedStatGenerator(-0.3f, 0.3f),
                new AmmoStatGenerator(-2f, 2f),
                new GlitchedCardSpawnedChanceStatGenerator(-0.035f, 0.05f)
            }).OnCardGenerated += (card, context) => { 
                GlitchedCards.Add(card);
                card.gameObject.AddComponent<GlitchingTextMono>();
            };

            // Uncommon stats
            new RandomStatHandler("GlitchedStatGeneratorUncommon", new List<RandomStatGenerator> {
                new DamageStatGenerator(-0.3f, 0.5f),
                new ReloadTimeStatGenerator(-0.45f, 0.3f),
                new AttackSpeedStatGenerator(-0.45f, 0.3f),
                new MovementSpeedStatGenerator(-0.15f, 0.25f),
                new HealthStatGenerator(-0.3f, 0.5f),
                new BlockCooldownStatGenerator(-0.45f, 0.3f),
                new BulletSpeedStatGenerator(-0.3f, 0.4f),
                new AmmoStatGenerator(-2f, 4f),
                new RegenStatGenerator(0, 15),
                new AdditionalBlocksStatGenerator(0, 0.60f),
                new GlitchedCardSpawnedChanceStatGenerator(-0.05f, 0.1f)
            }).OnCardGenerated += (card, context) => { 
                GlitchedCards.Add(card);
                card.gameObject.AddComponent<GlitchingTextMono>();
                card.rarity = CardInfo.Rarity.Uncommon;
            };

            // Rare stats
            new RandomStatHandler("GlitchedStatGeneratorRare", new List<RandomStatGenerator> {
                new DamageStatGenerator(-0.3f, 0.8f),
                new ReloadTimeStatGenerator(-0.6f, 0.3f),
                new AttackSpeedStatGenerator(-0.6f, 0.3f),
                new MovementSpeedStatGenerator(-0.2f, 0.35f),
                new HealthStatGenerator(-0.3f, 0.75f),
                new BlockCooldownStatGenerator(-0.60f, 0.3f),
                new BulletSpeedStatGenerator(-0.3f, 0.60f),
                new AmmoStatGenerator(-3f, 6f),
                new RegenStatGenerator(0, 35),
                new AdditionalBlocksStatGenerator(0, 0.8f),
                new GlitchedCardSpawnedChanceStatGenerator(-0.1f, 0.2f)
            }).OnCardGenerated += (card, context) => { 
                GlitchedCards.Add(card);
                card.gameObject.AddComponent<GlitchingTextMono>();
                card.rarity = CardInfo.Rarity.Rare;
            };

            // Legendary stats
            new RandomStatHandler("GlitchedStatGeneratorLegendary", new List<RandomStatGenerator> {
                new DamageStatGenerator(-0.3f, 1f),
                new ReloadTimeStatGenerator(-0.75f, 0.3f),
                new AttackSpeedStatGenerator(-0.75f, 0.3f),
                new MovementSpeedStatGenerator(-0.25f, 0.45f),
                new HealthStatGenerator(-0.3f, 1f),
                new BlockCooldownStatGenerator(-0.75f, 0.3f),
                new BulletSpeedStatGenerator(-0.3f, 0.75f),
                new AmmoStatGenerator(-4f, 8f),
                new RegenStatGenerator(0, 50),
                new GlitchedCardSpawnedChanceStatGenerator(-0.35f, 0.35f),
                new AdditionalBlocksStatGenerator(0, 2f),
                new ExtraLiveStatGenerator(0, 1)
            }).OnCardGenerated += (card, context) => {
                GlitchedCards.Add(card);
                card.gameObject.AddComponent<GlitchingTextMono>();
                card.rarity = RarityUtils.GetRarity("Legendary");
            };

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

            System.Random random = new System.Random(Version.GetHashCode());
            for(int i = 0; i < 100; i++) {
                RandomStatManager.CreateRandomStatsCard("GlitchedStatGeneratorCommon", random.Next(), "Glitched Card", "A random description", 1, 3);
            }
            for(int i = 0; i < 75; i++) {
                RandomStatManager.CreateRandomStatsCard("GlitchedStatGeneratorUncommon", random.Next(), "Glitched Card", "A random description", 1, 4);
            }
            for(int i = 0; i < 50; i++) {
                RandomStatManager.CreateRandomStatsCard("GlitchedStatGeneratorRare", random.Next(), "Glitched Card", "A random description", 1, 5);
            }
            for(int i = 0; i < 25; i++) {
                RandomStatManager.CreateRandomStatsCard("GlitchedStatGeneratorLegendary", random.Next(), "Glitched Card", "A random description", 2, 6);
            }

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

