using AALUND13Cards.MonoBehaviours;
using System;
using System.Runtime.CompilerServices;

namespace AALUND13Cards.Extensions {
    public class AALUND13CardCharacterDataAdditionalData {
        // Delayed Damage
        public float secondToDealDamage = 0;
        public bool dealDamage = true;

        // Soulstreak
        public SoulStreakStats SoulStreakStats = new SoulStreakStats();
        public uint Souls = 0;

        // Extra Cards
        public int RandomCardsAtStart = 0;
        public int ExtraCardPicks = 0;

        // Armor
        public float DamageAgainstArmorPercentage = 1f;

        // Blocks
        public int BlocksWhenRecharge = 0;
        public float BlockPircePercent = 0f;

        // Uncategorized
        public float CurrentHPRegenPercentage = 0f;
        public RailgunStats RailgunStats = new RailgunStats();
        public bool isBind = false;

        public void Reset() {
            secondToDealDamage = 0;
            dealDamage = true;

            SoulStreakStats = new SoulStreakStats();

            RandomCardsAtStart = 0;
            ExtraCardPicks = 0;

            DamageAgainstArmorPercentage = 1f;

            BlocksWhenRecharge = 0;
            BlockPircePercent = 0f;

            CurrentHPRegenPercentage = 0f;
            RailgunStats = new RailgunStats();
            isBind = false;
        }
    }

    public static class CharacterDataExtensions {
        public static readonly ConditionalWeakTable<CharacterData, AALUND13CardCharacterDataAdditionalData> data = new ConditionalWeakTable<CharacterData, AALUND13CardCharacterDataAdditionalData>();

        public static AALUND13CardCharacterDataAdditionalData GetAdditionalData(this CharacterData characterData) {
            return data.GetOrCreateValue(characterData);
        }

        public static void AddData(this CharacterData characterData, AALUND13CardCharacterDataAdditionalData value) {
            try {
                data.Add(characterData, value);
            } catch(Exception) { }
        }
    }
}
