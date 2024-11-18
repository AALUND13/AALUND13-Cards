﻿using AALUND13Card.MonoBehaviours;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace AALUND13Card.Extensions {
    public class AALUND13CardCharacterDataAdditionalData {
        // TODO: Rewrite the delay damage system.
        public float secondToDealDamage = 0;
        public List<DamageDealSecond> DamageDealSecond = new List<DamageDealSecond>();
        public bool dealDamage = true;

        public SoulStreakStats SoulStreakStats = new SoulStreakStats();
        public uint Killstreak = 0;
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