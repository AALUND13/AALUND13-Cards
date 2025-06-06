﻿using System;

namespace AALUND13Cards.Extensions {
    public static class RandomExtension {
        public static float NextFloat(this Random random, float minValue, float maxValue) {
            return (float)random.NextDouble() * (maxValue - minValue) + minValue;
        }
    }
}
