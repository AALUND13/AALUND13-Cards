using AALUND13Card.Cards;
using AALUND13Card.MonoBehaviours;
using FancyCardBar;
using RarityLib.Utils;
using System.Collections.Generic;
using System.Linq;

namespace AALUND13Card.RandomStatGenerators.Generators {
    internal class CorruptedStatGenerator {
        public static List<CardInfo> CorruptedCards => AALUND13_Cards.CardResgester != null ? AALUND13_Cards.CardResgester.Cards
            .FindAll(c => c.GetComponent<CorruptedCard>() != null)
            .Select(c => c.GetComponent<CardInfo>())
            .ToList() : new List<CardInfo>();

        public static void RegisterCorruptedStatGenerators() {
            // Trinket stats
            new RandomStatHandler("CorruptedStatGenerator_Trinket", new List<RandomStatGenerator> {
                new DamageStatGenerator(-0.3f, 0.1f),
                new ReloadTimeStatGenerator(-0.1f, 0.1f),
                new AttackSpeedStatGenerator(-0.1f, 0.1f),
                new MovementSpeedStatGenerator(-0.05f, 0.05f, 0.01f),
                new HealthStatGenerator(-0.1f, 0.1f),
                new BulletSpeedStatGenerator(-0.1f, 0.1f),
                new AmmoStatGenerator(-1f, 1f),
            }).OnCardGenerated += (card, random) => {
                card.gameObject.AddComponent<FancyIcon>().fancyIcon = AALUND13_Cards.CorruptedCardFancyIconPrefab;
                card.gameObject.AddComponent<GlitchingCardEffect>();
                card.rarity = RarityUtils.GetRarity("Trinket");
            };

            // Common stats
            new RandomStatHandler("CorruptedStatGenerator_Common", new List<RandomStatGenerator> {
                new DamageStatGenerator(-0.3f, 0.3f),
                new ReloadTimeStatGenerator(-0.3f, 0.3f),
                new AttackSpeedStatGenerator(-0.3f, 0.3f),
                new MovementSpeedStatGenerator(-0.1f, 0.1f),
                new HealthStatGenerator(-0.3f, 0.3f),
                new BlockCooldownStatGenerator(-0.05f, 0.05f),
                new BulletSpeedStatGenerator(-0.3f, 0.3f),
                new AmmoStatGenerator(-2f, 2f),
                new GlitchedCardSpawnedChanceStatGenerator(-0.05f, 0.05f)
            }).OnCardGenerated += (card, random) => {
                card.gameObject.AddComponent<FancyIcon>().fancyIcon = AALUND13_Cards.CorruptedCardFancyIconPrefab;
                card.gameObject.AddComponent<GlitchingCardEffect>();
            };

            // Scarce stats
            new RandomStatHandler("CorruptedStatGenerator_Scarce", new List<RandomStatGenerator> {
                new DamageStatGenerator(-0.3f, 0.4f),
                new ReloadTimeStatGenerator(-0.4f, 0.3f),
                new AttackSpeedStatGenerator(-0.4f, 0.3f),
                new MovementSpeedStatGenerator(-0.15f, 0.2f),
                new HealthStatGenerator(-0.3f, 0.4f),
                new BlockCooldownStatGenerator(-0.1f, 0.1f),
                new BulletSpeedStatGenerator(-0.3f, 0.35f),
                new AmmoStatGenerator(-2f, 3f),
                new RegenStatGenerator(0, 10),
                new GlitchedCardSpawnedChanceStatGenerator(-0.075f, 0.075f)
            }).OnCardGenerated += (card, random) => {
                card.gameObject.AddComponent<FancyIcon>().fancyIcon = AALUND13_Cards.CorruptedCardFancyIconPrefab;
                card.gameObject.AddComponent<GlitchingCardEffect>();
                card.rarity = RarityUtils.GetRarity("Scarce");
            };

            // Uncommon stats
            new RandomStatHandler("CorruptedStatGenerator_Uncommon", new List<RandomStatGenerator> {
                new DamageStatGenerator(-0.3f, 0.5f),
                new ReloadTimeStatGenerator(-0.45f, 0.3f),
                new AttackSpeedStatGenerator(-0.45f, 0.3f),
                new MovementSpeedStatGenerator(-0.15f, 0.25f),
                new HealthStatGenerator(-0.3f, 0.5f),
                new BlockCooldownStatGenerator(-0.15f, 0.15f),
                new BulletSpeedStatGenerator(-0.3f, 0.4f),
                new AmmoStatGenerator(-2f, 4f),
                new RegenStatGenerator(0, 15),
                new AdditionalBlocksStatGenerator(0, 0.588235294118f), // '0.588235294118' is '15%' chance to gain an extra block
                new GlitchedCardSpawnedChanceStatGenerator(-0.1f, 0.1f)
            }).OnCardGenerated += (card, random) => {
                card.gameObject.AddComponent<FancyIcon>().fancyIcon = AALUND13_Cards.CorruptedCardFancyIconPrefab;
                card.gameObject.AddComponent<GlitchingCardEffect>();
                card.rarity = CardInfo.Rarity.Uncommon;
            };

            // Exotic stats
            new RandomStatHandler("CorruptedStatGenerator_Exotic", new List<RandomStatGenerator> {
                new DamageStatGenerator(-0.3f, 0.6f),
                new ReloadTimeStatGenerator(-0.5f, 0.3f),
                new AttackSpeedStatGenerator(-0.5f, 0.3f),
                new MovementSpeedStatGenerator(-0.2f, 0.3f),
                new HealthStatGenerator(-0.3f, 0.6f),
                new BlockCooldownStatGenerator(-0.20f, 0.20f),
                new BulletSpeedStatGenerator(-0.3f, 0.5f),
                new AmmoStatGenerator(-3f, 5f),
                new RegenStatGenerator(0, 20),
                new AdditionalBlocksStatGenerator(0, 0.625f), // '0.625' is '20%' chance to gain an extra block
                new GlitchedCardSpawnedChanceStatGenerator(-0.15f, 0.15f)
            }).OnCardGenerated += (card, random) => {
                card.gameObject.AddComponent<FancyIcon>().fancyIcon = AALUND13_Cards.CorruptedCardFancyIconPrefab;
                card.gameObject.AddComponent<GlitchingCardEffect>();
                card.rarity = RarityUtils.GetRarity("Exotic");
            };

            // Rare stats
            new RandomStatHandler("CorruptedStatGenerator_Rare", new List<RandomStatGenerator> {
                new DamageStatGenerator(-0.3f, 0.8f),
                new ReloadTimeStatGenerator(-0.6f, 0.3f),
                new AttackSpeedStatGenerator(-0.6f, 0.3f),
                new MovementSpeedStatGenerator(-0.2f, 0.35f),
                new HealthStatGenerator(-0.3f, 0.75f),
                new BlockCooldownStatGenerator(-0.25f, 0.20f),
                new BulletSpeedStatGenerator(-0.3f, 0.60f),
                new AmmoStatGenerator(-3f, 6f),
                new RegenStatGenerator(0, 35),
                new JumpStatGenerator(0f, 1f),
                new AdditionalBlocksStatGenerator(0, 0.666666666667f), // '0.666666666667' is '25%' chance to gain an extra block
                new GlitchedCardSpawnedChanceStatGenerator(-0.2f, 0.2f)
            }).OnCardGenerated += (card, random) => {
                card.gameObject.AddComponent<FancyIcon>().fancyIcon = AALUND13_Cards.CorruptedCardFancyIconPrefab;
                card.gameObject.AddComponent<GlitchingCardEffect>();
                card.rarity = CardInfo.Rarity.Rare;
            };

            // Epic stats
            new RandomStatHandler("CorruptedStatGenerator_Epic", new List<RandomStatGenerator> {
                new DamageStatGenerator(-0.3f, 0.9f),
                new ReloadTimeStatGenerator(-0.7f, 0.3f),
                new AttackSpeedStatGenerator(-0.7f, 0.3f),
                new MovementSpeedStatGenerator(-0.25f, 0.4f),
                new HealthStatGenerator(-0.3f, 0.9f),
                new BlockCooldownStatGenerator(-0.3f, 0.25f),
                new BulletSpeedStatGenerator(-0.3f, 0.65f),
                new AmmoStatGenerator(-4f, 7f),
                new RegenStatGenerator(0, 40),
                new AdditionalBlocksStatGenerator(0, 0.714285714286f), // '0.714285714286' is '30%' chance to gain an extra block
                new JumpStatGenerator(0f, 1.5f),
                new ExtraLiveStatGenerator(0, 0.689655172414f), // '0.689655172414' is '27.5%' chance to gain an extra life
                new GlitchedCardSpawnedChanceStatGenerator(-0.25f, 0.25f)
            }).OnCardGenerated += (card, random) => {
                card.gameObject.AddComponent<FancyIcon>().fancyIcon = AALUND13_Cards.CorruptedCardFancyIconPrefab;
                card.gameObject.AddComponent<GlitchingCardEffect>();
                card.rarity = RarityUtils.GetRarity("Epic");
            };

            // Legendary stats
            new RandomStatHandler("CorruptedStatGenerator_Legendary", new List<RandomStatGenerator> {
                new DamageStatGenerator(-0.3f, 1f),
                new ReloadTimeStatGenerator(-0.75f, 0.3f),
                new AttackSpeedStatGenerator(-0.75f, 0.3f),
                new MovementSpeedStatGenerator(-0.25f, 0.45f),
                new HealthStatGenerator(-0.3f, 1f),
                new BlockCooldownStatGenerator(-0.35f, 0.25f),
                new BulletSpeedStatGenerator(-0.3f, 0.75f),
                new AmmoStatGenerator(-4f, 8f),
                new RegenStatGenerator(0, 50),
                new AdditionalBlocksStatGenerator(0, 0.833333333333f), // '0.833333333333' is '40%' chance to gain an extra block
                new JumpStatGenerator(0f, 2f),
                new ExtraLiveStatGenerator(0, 0.769230769231f), // '0.769230769231' is '35%' chance to gain an extra life
                new GlitchedCardSpawnedChanceStatGenerator(-0.35f, 0.35f)
            }).OnCardGenerated += (card, random) => {
                card.gameObject.AddComponent<FancyIcon>().fancyIcon = AALUND13_Cards.CorruptedCardFancyIconPrefab;
                card.gameObject.AddComponent<GlitchingCardEffect>();
                card.rarity = RarityUtils.GetRarity("Legendary");
            };

            // Mythical stats
            new RandomStatHandler("CorruptedStatGenerator_Mythical", new List<RandomStatGenerator> {
                new DamageStatGenerator(-0.3f, 1.125f),
                new ReloadTimeStatGenerator(-0.8f, 0.3f),
                new AttackSpeedStatGenerator(-0.8f, 0.3f),
                new MovementSpeedStatGenerator(-0.25f, 0.5f),
                new HealthStatGenerator(-0.3f, 1.25f),
                new BlockCooldownStatGenerator(-0.4f, 0.25f),
                new BulletSpeedStatGenerator(-0.3f, 0.8f),
                new AmmoStatGenerator(-5f, 10f),
                new RegenStatGenerator(0, 65),
                new AdditionalBlocksStatGenerator(0, 1f),
                new JumpStatGenerator(0f, 2.5f),
                new ExtraLiveStatGenerator(0, 0.909090909091f), // '0.909090909091' is '45%' chance to gain an extra life
                new GlitchedCardSpawnedChanceStatGenerator(-0.4f, 0.4f)
            }).OnCardGenerated += (card, random) => {
                card.gameObject.AddComponent<FancyIcon>().fancyIcon = AALUND13_Cards.CorruptedCardFancyIconPrefab;
                card.gameObject.AddComponent<GlitchingCardEffect>();
                card.rarity = RarityUtils.GetRarity("Mythical");
            };

            // Divine stats
            new RandomStatHandler("CorruptedStatGenerator_Divine", new List<RandomStatGenerator> {
                new DamageStatGenerator(-0.3f, 1.5f),
                new ReloadTimeStatGenerator(-0.85f, 0.3f),
                new AttackSpeedStatGenerator(-0.85f, 0.3f),
                new MovementSpeedStatGenerator(-0.25f, 0.55f),
                new HealthStatGenerator(-0.3f, 1.5f),
                new BlockCooldownStatGenerator(-0.45f, 0.25f),
                new BulletSpeedStatGenerator(-0.3f, 0.85f),
                new AmmoStatGenerator(-6f, 15f),
                new RegenStatGenerator(0, 100),
                new AdditionalBlocksStatGenerator(0, 1.25f),
                new JumpStatGenerator(0f, 3f),
                new ExtraLiveStatGenerator(0, 2f),
                new GlitchedCardSpawnedChanceStatGenerator(-0.5f, 0.5f)
            }).OnCardGenerated += (card, random) => {
                card.gameObject.AddComponent<FancyIcon>().fancyIcon = AALUND13_Cards.CorruptedCardFancyIconPrefab;
                card.gameObject.AddComponent<GlitchingCardEffect>();
                card.rarity = RarityUtils.GetRarity("Divine");
            };
        }
        //public static void BuildGlitchedCard() {
        //    System.Random random = new System.Random(AALUND13_Cards.Version.GetHashCode());
        //    for(int i = 0; i < 100; i++) {
        //        RandomStatManager.CreateRandomStatsCard("CorruptedStatGenerator_Common", random.Next(), "Corrupted Card", "A random description", 1, 3);
        //    }
        //    for(int i = 0; i < 75; i++) {
        //        RandomStatManager.CreateRandomStatsCard("CorruptedStatGenerator_Uncommon", random.Next(), "Corrupted Card", "A random description", 1, 4);
        //    }
        //    for(int i = 0; i < 50; i++) {
        //        RandomStatManager.CreateRandomStatsCard("CorruptedStatGenerator_Rare", random.Next(), "Corrupted Card", "A random description", 1, 5);
        //    }
        //    for(int i = 0; i < 25; i++) {
        //        RandomStatManager.CreateRandomStatsCard("CorruptedStatGenerator_Legendary", random.Next(), "Corrupted Card", "A random description", 2, 6);
        //    }

        //}
    }
}
