using AALUND13Cards.Core.Utils;

namespace AALUND13Cards.Armors.Cards {
    public class ArmorStats : ICustomStats {
        public float DamageAgainstArmorPercentage = 1f;
        public float ArmorDamageReduction = 0f;

        public void ResetStats() {
            DamageAgainstArmorPercentage = 1f;
            ArmorDamageReduction = 0f;
        }
    }
}
