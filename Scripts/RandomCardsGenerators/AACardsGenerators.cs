using System.Collections.Generic;
using RandomCardsGenerators;
using RandomCardsGenerators.StatsGroup;

namespace AALUND13Card {
    public enum AACardsGeneratorType {
        CardFactoryGenerator,
    }

    internal class AACardsGenerators {
        public static ModRandomCardsGenerators<AACardsGeneratorType> Generators;

        public static void RegisterGenerators() {
            var generators = new Dictionary<AACardsGeneratorType, RandomCardsGenerator>();
            generators.Add(AACardsGeneratorType.CardFactoryGenerator, CreateCardFactoryGenerator());

            Generators = new ModRandomCardsGenerators<AACardsGeneratorType>(generators);
        }

        private static RandomCardsGenerator CreateCardFactoryGenerator() {
            var randomCardOption = new RandomCardOption(
                "Defective Card",
                AALUND13_Cards.ModInitials,
                "This card that come out the factory is defective, it has some negative stats.",
                "Dc",
                1, 4,
                CardInfo.Rarity.Common,
                CardThemeColor.CardThemeColorType.DestructiveRed
            );

            return new RandomCardsGenerator("DefectiveCardGenerators", randomCardOption, new List<RandomStatGenerator> {
              new DamageStatGenerator(-0.5f, 0),
              new ReloadTimeStatGenerator(0, 0.5f),
              new AttackSpeedStatGenerator(0, 0.5f),
              new MovementSpeedStatGenerator(-0.5f, 0f),
              new HealthStatGenerator(-0.5f, 0),
              new BlockCooldownStatGenerator(0, 0.5f),
              new BulletSpeedStatGenerator(-0.5f, 0)
          });
        }
    }
}
