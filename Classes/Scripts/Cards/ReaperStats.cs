using AALUND13Cards.Core.Utils;
using System;

namespace AALUND13Cards.Classes.Cards {
    public class ReaperStats : ICustomStats {
        public float ScalingPercentageDamage = 0f;
        public float ScalingPercentageDamageUnCap = 0f;
        public float ScalingPercentageDamageCap = 0f;

        public void ResetStats() {
            ScalingPercentageDamage = 0f;
            ScalingPercentageDamageUnCap = 0f;
            ScalingPercentageDamageCap = 0f;
        }
    }
}
