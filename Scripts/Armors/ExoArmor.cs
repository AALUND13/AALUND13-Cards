using JARL.Armor;
using JARL.Armor.Bases;
using System;
using UnityEngine;

namespace AALUND13Cards.Armors {
    public class ExoArmor : ArmorBase {
        public Action<float> OnArmorDamaged;
        public float ArmorDamageReduction;

        private float queuedDamage;

        public override BarColor GetBarColor() {
            return new BarColor(Color.cyan * 0.6f, Color.cyan * 0.45f);
        }

        public override DamageArmorInfo OnDamage(float damage, Player DamagingPlayer, ArmorDamagePatchType? armorDamagePatchType) {
            queuedDamage += damage;
            
            float reducedDamage = damage * (1 - ArmorDamageReduction);
            return base.OnDamage(reducedDamage, DamagingPlayer, armorDamagePatchType);
        }

        public override void OnUpdate() {
            if(queuedDamage > 0) {
                OnArmorDamaged?.Invoke(queuedDamage);
                queuedDamage = 0;
            }
        }

        public ExoArmor() {
            ArmorTags.Add("CanArmorPierce");
        }
    }
}
