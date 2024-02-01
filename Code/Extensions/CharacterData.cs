using JARL.ArmorFramework;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using static AALUND13Card.Classes;

namespace AALUND13Card.Extensions
{
    public class AALUND13CardCharacterDataAdditionalData
    {
        public float secondToDealDamage;
        public List<DamageDealSecond> DamageDealSecond;
        public bool dealDamage;

        public AALUND13CardCharacterDataAdditionalData()
        {
            secondToDealDamage = 0;
            DamageDealSecond = new List<DamageDealSecond>();
            dealDamage = true;
        }
    }

    public static class CharacterDataExtensions
    {
        public static readonly ConditionalWeakTable<CharacterData, AALUND13CardCharacterDataAdditionalData> data = new ConditionalWeakTable<CharacterData, AALUND13CardCharacterDataAdditionalData>();

        public static AALUND13CardCharacterDataAdditionalData GetAdditionalData(this CharacterData block)
        {
            return data.GetOrCreateValue(block);
        }

        public static void AddData(this CharacterData block, AALUND13CardCharacterDataAdditionalData value)
        {
            try
            {
                data.Add(block, value);
            }
            catch (Exception) { }
        }
    }
}
