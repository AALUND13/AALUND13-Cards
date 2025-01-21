using AALUND13Card.Cards;
using UnboundLib;
using UnityEngine;

namespace AALUND13Card.RandomStatGenerators {
    public class GlitchedCardSpawnedChanceStatGenerator : RandomStatGenerator {
        public override string StatName => "Glitched Card Spawned Chance";
        public GlitchedCardSpawnedChanceStatGenerator(float minValue, float maxValue) : base(minValue, maxValue) { }

        public override string Apply(float value, GameObject cardObj, Gun gun, CharacterStatModifiers characterStats, Block block) {
            AAStatModifers statModifiers = cardObj.GetOrAddComponent<AAStatModifers>();
            statModifiers.GlitchedCardSpawnChance += value;
            return GetStringValue(value * 100);
        }
        public override bool IsPositive(float value) => value < 0;
    }

}
