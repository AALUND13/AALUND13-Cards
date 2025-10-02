using JARL.Armor;
using JARL.Armor.Bases;
using UnityEngine;

namespace AALUND13Cards.Classes.Armors {
    public class SoulArmor : ArmorBase {
        public override BarColor GetBarColor() {
            return new BarColor(Color.magenta * 0.6f, Color.magenta * 0.45f);
        }

        public SoulArmor() {
            ArmorRegenCooldownSeconds = 5;
        }
    }
}
