using AALUND13Cards.Core.Utils;
using RarityLib.Utils;

namespace AALUND13Cards.Standard.Cards {
    public class StandardStats : ICustomStats {
        // Delayed Damage
        public float secondToDealDamage = 0;
        public bool dealDamage = true;

        // Blocks
        public int BlocksWhenRecharge = 0;
        public float StunBlockTime = 0f;

        // Curses
        public Rarity MaxRarityForCurse = null;

        // Uncategorized
        public float DamageReduction = 0f;

        public void ResetStats() {
            // Delayed Damage
            secondToDealDamage = 0;
            dealDamage = true;

            // Blocks
            BlocksWhenRecharge = 0;
            StunBlockTime = 0f;

            // curses
            MaxRarityForCurse = null;

            // Uncategorized
            DamageReduction = 0f;
        }
    }
}
