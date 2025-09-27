using AALUND13Cards.MonoBehaviours.CardsEffects;
using AALUND13Cards.MonoBehaviours.CardsEffects.Soulstreak;
using AALUND13Cards.Utils;
using RarityLib.Utils;
using System;
using System.Runtime.CompilerServices;

namespace AALUND13Cards.Extensions {
    public class AALUND13CardCharacterDataAdditionalData {
        // Delayed Damage
        public float secondToDealDamage = 0;
        public bool dealDamage = true;

        // Extra Cards
        public int ExtraCardPicks = 0;
        public int DuplicatesAsCorrupted = 0;

        // Armors
        public float DamageAgainstArmorPercentage = 1f;
        public float ArmorDamageReduction = 0f;

        // Blocks
        public int BlocksWhenRecharge = 0;
        public float BlockPircePercent = 0f;
        public float StunBlockTime = 0f;

        // Curses
        public Rarity MaxRarityForCurse = null;
        public bool IsBind = false;
        public bool DisableDecayTime = false;

        // Stats Classes
        public CustomStatsManager CustomStatsManager = new CustomStatsManager();

        // Uncategorized
        public float DamageReduction = 0f;
        public float ScalingPercentageDamage = 0f;
        public float ScalingPercentageDamageUnCap = 0f;
        public float ScalingPercentageDamageCap = 0f;

        public void Reset() {
            // Delayed Damage
            secondToDealDamage = 0;
            dealDamage = true;
            
            // Extra Cards
            ExtraCardPicks = 0;
            DuplicatesAsCorrupted = 0;

            // Armors
            DamageAgainstArmorPercentage = 1f;
            ArmorDamageReduction = 0f;

            // Blocks
            BlocksWhenRecharge = 0;
            BlockPircePercent = 0f;
            StunBlockTime = 0f;

            // Reset curses
            MaxRarityForCurse = null;
            DisableDecayTime = false;
            IsBind = false;

            // Reset stats classes
            CustomStatsManager.ResetAll();

            // Uncategorized
            DamageReduction = 0f;
            ScalingPercentageDamage = 0f;
            ScalingPercentageDamageUnCap = 0f;
            ScalingPercentageDamageCap = 0f;
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
