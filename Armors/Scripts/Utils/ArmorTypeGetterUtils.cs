using JARL.Armor;
using JARL.Armor.Bases;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AALUND13Cards.Armors.Utils {
    public class ArmorTypeGetterUtils {
        public static ReadOnlyDictionary<string, Type> RegisterArmorType => new ReadOnlyDictionary<string, Type>(RegisterArmorType);
        private static Dictionary<string, Type> registerArmorType = new Dictionary<string, Type>();

        public static void RegiterArmorType<T>(string armorId) where T : ArmorBase, new() {
            if(registerArmorType.ContainsKey(armorId)) throw new ArgumentException($"Armor with the type '{armorId}' already register");

            ArmorFramework.RegisterArmorType<T>();
            registerArmorType.Add(armorId, typeof(T));
        }

        public static Type GetArmorType(string armorId) => registerArmorType[armorId];
    }
}
