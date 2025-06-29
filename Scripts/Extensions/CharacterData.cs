﻿using AALUND13Cards.MonoBehaviours.CardsEffects;
using AALUND13Cards.MonoBehaviours.CardsEffects.Soulstreak;
using RarityLib.Utils;
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
        public int DuplicatesAsCorrupted = 0;

        // Armor
        public float DamageAgainstArmorPercentage = 1f;

        // Blocks
        public int BlocksWhenRecharge = 0;
        public float BlockPircePercent = 0f;

        // Curses
        public Rarity MaxRarityForCurse = null;
        public bool isBind = false;

        // Uncategorized
        public float CurrentHPRegenPercentage = 0f;
        public RailgunStats RailgunStats = new RailgunStats();

        public void Reset() {
            secondToDealDamage = 0;
            dealDamage = true;

            SoulStreakStats = new SoulStreakStats();

            RandomCardsAtStart = 0;
            ExtraCardPicks = 0;
            DuplicatesAsCorrupted = 0;

            DamageAgainstArmorPercentage = 1f;

            BlocksWhenRecharge = 0;
            BlockPircePercent = 0f;

            MaxRarityForCurse = null;
            isBind = false;

            CurrentHPRegenPercentage = 0f;
            RailgunStats = new RailgunStats();
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
