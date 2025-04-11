using AALUND13Card.MonoBehaviours;
using JARL.Armor;
using JARL.Armor.Bases;
using UnityEngine;

namespace AALUND13Card.Armors {
    public class SoulArmor : ArmorBase {
        public override BarColor GetBarColor() {
            return new BarColor(Color.magenta * 0.6f, Color.magenta * 0.45f);
        }

        public override void OnRespawn() {
            SoulstreakMono soulstreak = ArmorHandler.GetComponentInChildren<SoulstreakMono>();
            if(soulstreak != null) {
                MaxArmorValue = 0;
                CurrentArmorValue = 0;

                soulstreak.AbilityCooldown = 0;
                soulstreak.AbilityActive = false;
            }
        }

        public override void OnRegister() {
            ArmorRegenCooldownSeconds = 5;
        }
    }
}
