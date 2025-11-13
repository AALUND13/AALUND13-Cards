using UnityEngine;

namespace AALUND13Cards.Classes.Utils {
    public static class MathUtils {
        public const float PERCENT_CAP = 0.8f;
        public const float EXPONENT = 0.75f;

        public static float GetEffectivePercentCap(float sps, float basePercent, float percentCap = 0.8f) {
            float safeSps = Mathf.Max(0.0001f, sps);
            float capPercentCap = Mathf.Min(percentCap, PERCENT_CAP);
            float scaled = basePercent * Mathf.Pow(1f / safeSps, EXPONENT);
            return Mathf.Min(scaled, capPercentCap);
        }

        public static float GetEffectivePercent(float sps, float basePercent) {
            float safeSps = Mathf.Max(0.0001f, sps);
            return basePercent * Mathf.Pow(1f / safeSps, EXPONENT);
        }
    }
}
