using AALUND13Card.MonoBehaviours;
using RarityLib.Utils;
using System.Collections.Generic;

namespace AALUND13Card.RandomStatGenerators.Generators {
    internal class CorruptedStatGenerator {
        public static List<CardInfo> CorruptedCards = new List<CardInfo>();

        public static void RegisterCorruptedStatGenerators() {
            // Common stats
            new RandomStatHandler("CorruptedStatGeneratorCommon", new List<RandomStatGenerator> {
                new DamageStatGenerator(-0.3f, 0.3f),
                new ReloadTimeStatGenerator(-0.3f, 0.3f),
                new AttackSpeedStatGenerator(-0.3f, 0.3f),
                new MovementSpeedStatGenerator(-0.1f, 0.1f),
                new HealthStatGenerator(-0.3f, 0.3f),
                new BlockCooldownStatGenerator(-0.1f, 0.1f),
                new BulletSpeedStatGenerator(-0.3f, 0.3f),
                new AmmoStatGenerator(-2f, 2f),
                new GlitchedCardSpawnedChanceStatGenerator(-0.05f, 0.05f)
            }).OnCardGenerated += (card, context) => {
                CorruptedCards.Add(card);
                card.gameObject.AddComponent<GlitchingCardEffect>();
            };

            // Uncommon stats
            new RandomStatHandler("CorruptedStatGeneratorUncommon", new List<RandomStatGenerator> {
                new DamageStatGenerator(-0.3f, 0.5f),
                new ReloadTimeStatGenerator(-0.45f, 0.3f),
                new AttackSpeedStatGenerator(-0.45f, 0.3f),
                new MovementSpeedStatGenerator(-0.15f, 0.25f),
                new HealthStatGenerator(-0.3f, 0.5f),
                new BlockCooldownStatGenerator(-0.25f, 0.15f),
                new BulletSpeedStatGenerator(-0.3f, 0.4f),
                new AmmoStatGenerator(-2f, 4f),
                new RegenStatGenerator(0, 15),
                new AdditionalBlocksStatGenerator(0, 0.60f),
                new GlitchedCardSpawnedChanceStatGenerator(-0.1f, 0.1f)
            }).OnCardGenerated += (card, context) => {
                CorruptedCards.Add(card);
                card.gameObject.AddComponent<GlitchingCardEffect>();
                card.rarity = CardInfo.Rarity.Uncommon;
            };

            // Rare stats
            new RandomStatHandler("CorruptedStatGeneratorRare", new List<RandomStatGenerator> {
                new DamageStatGenerator(-0.3f, 0.8f),
                new ReloadTimeStatGenerator(-0.6f, 0.3f),
                new AttackSpeedStatGenerator(-0.6f, 0.3f),
                new MovementSpeedStatGenerator(-0.2f, 0.35f),
                new HealthStatGenerator(-0.3f, 0.75f),
                new BlockCooldownStatGenerator(-0.3f, 0.1f),
                new BulletSpeedStatGenerator(-0.3f, 0.60f),
                new AmmoStatGenerator(-3f, 6f),
                new RegenStatGenerator(0, 35),
                new AdditionalBlocksStatGenerator(0, 0.6f),
                new GlitchedCardSpawnedChanceStatGenerator(-0.2f, 0.2f)
            }).OnCardGenerated += (card, context) => {
                CorruptedCards.Add(card);
                card.gameObject.AddComponent<GlitchingCardEffect>();
                card.rarity = CardInfo.Rarity.Rare;
            };

            // Legendary stats
            new RandomStatHandler("CorruptedStatGeneratorLegendary", new List<RandomStatGenerator> {
                new DamageStatGenerator(-0.3f, 1f),
                new ReloadTimeStatGenerator(-0.75f, 0.3f),
                new AttackSpeedStatGenerator(-0.75f, 0.3f),
                new MovementSpeedStatGenerator(-0.25f, 0.45f),
                new HealthStatGenerator(-0.3f, 1f),
                new BlockCooldownStatGenerator(-0.5f, 0.25f),
                new BulletSpeedStatGenerator(-0.3f, 0.75f),
                new AmmoStatGenerator(-4f, 8f),
                new RegenStatGenerator(0, 50),
                new GlitchedCardSpawnedChanceStatGenerator(-0.35f, 0.35f),
                new AdditionalBlocksStatGenerator(0, 1.6f),
                new ExtraLiveStatGenerator(0, 0.8f)
            }).OnCardGenerated += (card, context) => {
                CorruptedCards.Add(card);
                card.gameObject.AddComponent<GlitchingCardEffect>();
                card.rarity = RarityUtils.GetRarity("Legendary");
            };
        }
        public static void BuildGlitchedCard() {
            System.Random random = new System.Random(AALUND13_Cards.Version.GetHashCode());
            for(int i = 0; i < 100; i++) {
                RandomStatManager.CreateRandomStatsCard("CorruptedStatGeneratorCommon", random.Next(), "Corrupted Card", "A random description", 1, 3);
            }
            for(int i = 0; i < 75; i++) {
                RandomStatManager.CreateRandomStatsCard("CorruptedStatGeneratorUncommon", random.Next(), "Corrupted Card", "A random description", 1, 4);
            }
            for(int i = 0; i < 50; i++) {
                RandomStatManager.CreateRandomStatsCard("CorruptedStatGeneratorRare", random.Next(), "Corrupted Card", "A random description", 1, 5);
            }
            for(int i = 0; i < 25; i++) {
                RandomStatManager.CreateRandomStatsCard("CorruptedStatGeneratorLegendary", random.Next(), "Corrupted Card", "A random description", 2, 6);
            }

        }
    }
}
