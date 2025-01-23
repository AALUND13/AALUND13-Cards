using AALUND13Card.MonoBehaviours;
using System;
using System.Runtime.CompilerServices;

namespace AALUND13Card.Extensions {
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

        // Glitched Cards
        public float CorruptedCardSpawnChance = 0;
        public float CorruptedCardSpawnChancePerPick = 0;

        public void Reset() {
            secondToDealDamage = 0;
            dealDamage = true;
            SoulStreakStats = new SoulStreakStats();
            RandomCardsAtStart = 0;
            ExtraCardPicks = 0;
            CorruptedCardSpawnChancePerPick = 0;
        }
    }

    public static class CharacterDataExtensions {
        public static readonly ConditionalWeakTable<CharacterData, AALUND13CardCharacterDataAdditionalData> data = new ConditionalWeakTable<CharacterData, AALUND13CardCharacterDataAdditionalData>();

        public static AALUND13CardCharacterDataAdditionalData GetAdditionalData(this CharacterData block) {
            return data.GetOrCreateValue(block);
        }

        public static void AddData(this CharacterData block, AALUND13CardCharacterDataAdditionalData value) {
            try {
                data.Add(block, value);
            } catch(Exception) { }
        }
    }
}
