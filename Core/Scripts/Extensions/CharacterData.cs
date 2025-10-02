using AALUND13Cards.Core.Utils;
using System;
using System.Runtime.CompilerServices;

namespace AALUND13Cards.Core.Extensions {
    public class AALUND13CardCharacterDataAdditionalData {
        public float BlockPircePercent = 0f;
        public CustomStatsRegistry CustomStatsRegistry = new CustomStatsRegistry();

        public void Reset() {
            BlockPircePercent = 0f;
            CustomStatsRegistry.ResetAll();
        }
    }

    public static class CharacterDataExtensions {
        public static readonly ConditionalWeakTable<CharacterData, AALUND13CardCharacterDataAdditionalData> data = new ConditionalWeakTable<CharacterData, AALUND13CardCharacterDataAdditionalData>();

        public static AALUND13CardCharacterDataAdditionalData GetAdditionalData(this CharacterData characterData) {
            return data.GetOrCreateValue(characterData);
        }

        public static CustomStatsRegistry GetCustomStatsRegistry(this CharacterData characterData) {
            return data.GetOrCreateValue(characterData).CustomStatsRegistry;
        }

        public static void AddData(this CharacterData characterData, AALUND13CardCharacterDataAdditionalData value) {
            try {
                data.Add(characterData, value);
            } catch(Exception) { }
        }
    }
}
