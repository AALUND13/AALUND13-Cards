using System.Collections.Generic;

namespace AALUND13Card.RandomStatGenerators.Generators {
    internal class NegativeStatGenerator {
        public static void RegisterNegativeStatGenerators() {
            new RandomStatHandler("NegativeStatGenerator", new List<RandomStatGenerator> {
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
