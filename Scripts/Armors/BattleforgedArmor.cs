using JARL.Armor;
using JARL.Armor.Bases;
using JARL.Armor.Utlis;
using UnityEngine;

namespace AALUND13Cards.Armors {
    public class BattleforgedArmor : ArmorBase {
        public override BarColor GetBarColor() {
            return new BarColor(Color.yellow * 0.6f, Color.yellow * 0.45f);
        }

        public override DamageArmorInfo OnDamage(float damage, Player DamagingPlayer, ArmorDamagePatchType? armorDamagePatchType) {
            DamageArmorInfo damageArmorInfo = ArmorUtils.ApplyDamage(CurrentArmorValue, damage);
            float armorLost = CurrentArmorValue - damageArmorInfo.Armor;

            MaxArmorValue += armorLost * 0.1f;
            return damageArmorInfo;
        }

        public BattleforgedArmor() {
            ArmorTags.Add("CanArmorPierce");
            ArmorRegenCooldownSeconds = 5;
        }
    }
}
