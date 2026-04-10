using AALUND13Cards.Core.Utils;
using RarityLib.Utils;
using System.Collections.Generic;

namespace AALUND13Cards.Devil.Cards {
    public class DevilCardsStats : ICustomStats {
        // Blocks
        public float FixedBlockCooldown = 0f;
        public bool DisbaleBlockTime = false;

        // Cards
        public List<Rarity> GuaranteedRarities 
            = new List<Rarity>();

        public void ResetStats() {
            // Apply Blocks Stats
            FixedBlockCooldown = 0f;
            DisbaleBlockTime = false;
            
            // Apply Cards Stats
            GuaranteedRarities.Clear();
        }
    }
}
