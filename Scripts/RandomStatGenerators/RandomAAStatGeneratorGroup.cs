using AALUND13Card.Cards;
using UnboundLib;
using UnityEngine;

namespace AALUND13Card.RandomStatGenerators {
    public class GlitchedCardSpawnedChanceStatGenerator : RandomStatGenerator {
        public override string StatName => "Glitched Card Spawned Chance";
        public GlitchedCardSpawnedChanceStatGenerator(float minValue, float maxValue) : base(minValue, maxValue) { }

        public override void Apply(float value, GameObject cardObj, Gun gun, CharacterStatModifiers characterStats, Block block) {
            AAStatModifers statModifiers = cardObj.GetOrAddComponent<AAStatModifers>();
            statModifiers.CorruptedCardSpawnChance += value;
        }
        public override bool ShouldAddStat(float value) => Mathf.Abs(value) > 0.01f;
        public override string GetStatString(float value) => GetStringValue(value * 100);
        public override bool IsPositive(float value) => value < 0;
    }

}
