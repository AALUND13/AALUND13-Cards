using AALUND13Cards.Core.Cards;
using AALUND13Cards.Core.Extensions;
using UnityEngine;

namespace AALUND13Cards.Classes.Cards.StatModifers {
    public class ReaperStatModifers : CustomStatModifers {
        [Header("Percentage Damage")]
        public float ScalingPercentageDamage = 0;
        public float ScalingPercentageDamageCap = 0;

        public override void Apply(Player player) {
            var additionalData = player.data.GetAdditionalData().CustomStatsRegistry.GetOrCreate<ReaperStats>();

            additionalData.ScalingPercentageDamageCap += ScalingPercentageDamageCap;
            additionalData.ScalingPercentageDamage += ScalingPercentageDamage;
        }
    }
}
