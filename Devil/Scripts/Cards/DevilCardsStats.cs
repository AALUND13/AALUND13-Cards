using AALUND13Cards.Core.Utils;
using RarityLib.Utils;

namespace AALUND13Cards.Devil.Cards {
    public class DevilCardsStats : ICustomStats {
        // Blocks
        public float FixedBlockCooldown = 0f;
        public bool DisbaleBlockTime = false;

        // Cards
        public Rarity GuaranteedRarity = null;

        public void ResetStats() {
            // Apply Blocks Stats
            FixedBlockCooldown = 0f;
            DisbaleBlockTime = false;
            
            // Apply Cards Stats
            GuaranteedRarity = null;
        }
    }
}
