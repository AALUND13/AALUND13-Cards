using AALUND13Card.MonoBehaviours;
using System;
using System.Runtime.CompilerServices;

namespace AALUND13Card.Extensions {
    public class AALUND13CardCharacterDataAdditionalData {
        public float secondToDealDamage = 0;
        public bool dealDamage = true;

        public SoulStreakStats SoulStreakStats = new SoulStreakStats();
        public uint Souls = 0;

        public int RandomCardsAtStart = 0;
        public float GlitchedCardSpawnChance = 0.3f;

        public void Reset() {
            secondToDealDamage = 0;
            dealDamage = true;
            SoulStreakStats = new SoulStreakStats();
            Souls = 0;
            RandomCardsAtStart = 0;
            GlitchedCardSpawnChance = 0.3f;
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
