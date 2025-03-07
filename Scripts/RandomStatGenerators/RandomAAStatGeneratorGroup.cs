using AALUND13Card.Cards;
using UnboundLib;
using UnityEngine;

namespace AALUND13Card.RandomStatGenerators {
    public class GlitchedCardSpawnedChanceStatGenerator : RandomStatGenerator {
        public override string StatName => "Glitched Card Spawned Chance";

        public float ThresholdToZero;
        public GlitchedCardSpawnedChanceStatGenerator(float minValue, float maxValue, float thresholdToZero = 0.05f) : base(minValue, maxValue) {
            ThresholdToZero = thresholdToZero;
        }

        public override bool ShouldApply(float value) => Mathf.Abs(value) >= ThresholdToZero;
        public override void Apply(float value, GameObject cardObj, Gun gun, CharacterStatModifiers characterStats, Block block) {
            AAStatModifers statModifiers = cardObj.GetOrAddComponent<AAStatModifers>();
            statModifiers.CorruptedCardSpawnChance += value;
        }

        public override bool IsPositive(float value) => value < 0;
    }

}
