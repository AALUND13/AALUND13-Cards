using System;
using UnityEngine;

namespace AALUND13Card.Utils.RandomStatsGenerator {
    public abstract class RandomStatGenerator {
        public abstract string StatName { get; }

        public float MinValue { get; private set; }
        public float MaxValue { get; private set; }

        public RandomStatGenerator(float minValue, float maxValue) {
            MinValue = minValue;
            MaxValue = maxValue;
        }

        public abstract string Apply(float value, Gun gun, CharacterStatModifiers characterStats, Block block);
        public abstract bool IsPositive(float value);

        protected string GetStringValue(float value) {
            return $"{(value > 0 ? "+" : "")}{Mathf.Round(value * 100)}%";
        }
    }
}
