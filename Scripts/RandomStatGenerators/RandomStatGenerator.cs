using UnityEngine;

namespace AALUND13Card.RandomStatGenerators {
    public abstract class RandomStatGenerator {
        public abstract string StatName { get; }

        public float MinValue { get; private set; }
        public float MaxValue { get; private set; }

        /// <param name="minValue">The minimum value that can be generated</param>
        /// <param name="maxValue">The maximum value that can be generated</param>
        public RandomStatGenerator(float minValue, float maxValue) {
            MinValue = minValue;
            MaxValue = maxValue;
        }

        public abstract void Apply(float value, GameObject cardObj, Gun gun, CharacterStatModifiers characterStats, Block block);
        public virtual string GetStatString(float value) => GetStringValue(value * 100);

        public abstract bool IsPositive(float value);

        public virtual bool ShouldAddStat(float value) {
            return Mathf.Abs(value) > 0.1f;
        }

        protected string GetStringValue(float value, bool isPercentage = true) {
            return $"{(value > 0 ? "+" : "")}{Mathf.Round(value)}{(isPercentage ? "%" : "")}";
        }
    }
}
