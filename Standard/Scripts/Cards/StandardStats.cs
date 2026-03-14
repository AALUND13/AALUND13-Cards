using AALUND13Cards.Core.Utils;
using RarityLib.Utils;
using System;

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
        public float BleedingDamage = 0f;
        public int QuickDashes = 0;

        // Freeze Damage
        public float FrozenTime = 0f;
        public float OldFrozenTime = 0f;

        // Bullets Damage Multiplier
        public float GravityDamageMultiplier = 0f;

        // Berserk Mode
        public int BerserkModeAmount = 0;
        public int RemainingBerserkModeAmount = 0;
        public Action OnBerserkMode;

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
            BleedingDamage = 0f;
            QuickDashes = 0;

            // Freeze Damage
            FrozenTime = 0f;
            OldFrozenTime = 0f;

            // Bullets Damage Multiplier
            GravityDamageMultiplier = 0f;

            // Berserk Mode
            BerserkModeAmount = 0;
            RemainingBerserkModeAmount = 0;
            OnBerserkMode = null;
        }
    }
}
