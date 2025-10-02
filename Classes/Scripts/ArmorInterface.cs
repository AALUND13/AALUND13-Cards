using AALUND13Cards.Armors.Utils;
using AALUND13Cards.Classes.Armors;
using JARL.Armor;

namespace AALUND13Cards.Classes {
    public class ArmorInterface {
        public static void RegisterArmors() {
            ArmorFramework.RegisterArmorType<SoulArmor>();
            ArmorTypeGetterUtils.RegiterArmorType<ExoArmor>("Exo-Armor");
        }
    }
}
