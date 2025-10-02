using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AALUND13Cards.Classes.Utils {
    public static class MathUtils {
        public const float PERCENT_CAP = 0.8f;

        public static float GetEffectivePercentCap(float sps, float basePercent, float percentCap = 0.8f) {
            float safeSps = Mathf.Max(0.0001f, sps);
            float capPercentCap = Mathf.Min(percentCap, PERCENT_CAP);
            return Mathf.Min(Mathf.Min(basePercent, capPercentCap) * (1f / Mathf.Sqrt(safeSps)), capPercentCap);
        }

        public static float GetEffectivePercent(float sps, float basePercent) {
            float safeSps = Mathf.Max(0.0001f, sps);
            return basePercent * (1f / Mathf.Sqrt(safeSps));
        }
    }
}
