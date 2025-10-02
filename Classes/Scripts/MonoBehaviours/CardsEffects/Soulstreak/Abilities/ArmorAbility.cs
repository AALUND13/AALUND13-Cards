using AALUND13Cards.Classes.Armors;
using JARL.Armor;
using JARL.Armor.Bases;
using UnityEngine;

namespace AALUND13Cards.Classes.MonoBehaviours.CardsEffects.Soulstreak.Abilities {
    public class ArmorAbility : ISoulstreakAbility {
        public float AbilityCooldownTime;

        private float abilityCooldown;
        private bool abilityActive;

        private ArmorHandler armorHandler;

        public ArmorAbility(Player player, float abilityCooldownTime) {
            AbilityCooldownTime = abilityCooldownTime;
            abilityCooldown = 0f;
            abilityActive = false;

            armorHandler = ArmorFramework.ArmorHandlers[player];
        }

        public void OnBlock(SoulstreakMono soulstreak) {
            if(!abilityActive && abilityCooldown == 0) {
                ArmorBase soulArmor = armorHandler.GetArmorByType<SoulArmor>();
                soulArmor.MaxArmorValue = soulstreak.Data.maxHealth * soulstreak.SoulstreakStats.SoulArmorPercentage * (soulstreak.SoulstreakStats.Souls + 1);
                soulArmor.ArmorRegenerationRate = soulArmor.MaxArmorValue * soulstreak.SoulstreakStats.SoulArmorPercentageRegenRate;
                soulArmor.CurrentArmorValue = soulArmor.MaxArmorValue;
                abilityActive = true;
            }
        }

        public void OnReset(SoulstreakMono soulstreak) {
            abilityActive = false;
            abilityCooldown = 0;

            ArmorBase soulArmor = armorHandler.GetArmorByType<SoulArmor>();
            soulArmor.MaxArmorValue = 0;
            soulArmor.CurrentArmorValue = 0;

        }

        public void OnUpdate(SoulstreakMono soulstreak) {
            abilityCooldown = Mathf.Max(abilityCooldown - TimeHandler.deltaTime, 0);

            if(armorHandler.GetArmorByType<SoulArmor>().CurrentArmorValue <= 0 && armorHandler.GetArmorByType<SoulArmor>().MaxArmorValue > 0) {
                armorHandler.GetArmorByType<SoulArmor>().MaxArmorValue = 0;
                abilityCooldown = 10;
                abilityActive = false;
            }
        }
    }
}
