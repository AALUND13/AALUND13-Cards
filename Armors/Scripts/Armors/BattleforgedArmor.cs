using JARL.Armor;
using JARL.Armor.Bases;
using JARL.Armor.Utlis;
using UnityEngine;

namespace AALUND13Cards.Armors.Armors {
    public class BattleforgedArmor : ArmorBase {
        public override BarColor GetBarColor() {
            return new BarColor(Color.yellow * 0.6f, Color.yellow * 0.45f);
        }

        public override DamageArmorInfo OnDamage(float damage, Player DamagingPlayer, ArmorDamagePatchType? armorDamagePatchType) {
            DamageArmorInfo damageArmorInfo = ArmorUtils.ApplyDamage(CurrentArmorValue, damage);
            float armorLost = CurrentArmorValue - damageArmorInfo.Armor;

            float factor = MaxArmorValue != 0 ? Mathf.Min(1 / (1 + (MaxArmorValue/ArmorHandler.Player.data.maxHealth)) * 2, 1) : 0;
            MaxArmorValue += armorLost * 0.1f * factor;
            return damageArmorInfo;
        }

        public BattleforgedArmor() {
            ArmorTags.Add("CanArmorPierce");
            ArmorRegenCooldownSeconds = 5;
        }
    }
}
