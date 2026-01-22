using AALUND13Cards.Classes.Armors;
using JARL.Armor;
using JARL.Armor.Bases;
using UnityEngine;

namespace AALUND13Cards.Classes.MonoBehaviours.CardsEffects.Soulstreak.Abilities {
    public class ArmorAbility : SoulstreakAbility<ArmorAbility> {
        public float AbilityCooldownTime;

        public float AbilityCooldown { get; private set; }
        private bool abilityActive;

        private ArmorHandler armorHandler;
        private Player player;

        public ArmorAbility(Player player, float abilityCooldownTime) {
            AbilityCooldownTime = abilityCooldownTime;
            AbilityCooldown = 0f;
            abilityActive = false;

            armorHandler = ArmorFramework.ArmorHandlers[player];
            this.player = player;
        }

        public override void OnBlock() {
            if(!abilityActive && AbilityCooldown == 0) {
                ArmorBase soulArmor = armorHandler.GetArmorByType<SoulArmor>();
                soulArmor.MaxArmorValue = player.data.maxHealth * SoulstreakStats.SoulArmorPercentage * (SoulstreakStats.Souls + 1);
                soulArmor.ArmorRegenerationRate = soulArmor.MaxArmorValue * SoulstreakStats.SoulArmorPercentageRegenRate;
                soulArmor.CurrentArmorValue = soulArmor.MaxArmorValue;
                abilityActive = true;
            }
        }

        public override void OnRevive() {
            abilityActive = false;
            AbilityCooldown = 0;

            ArmorBase soulArmor = armorHandler.GetArmorByType<SoulArmor>();
            soulArmor.MaxArmorValue = 0;
            soulArmor.CurrentArmorValue = 0;
        }

        public override void OnUpdate() {
            AbilityCooldown = Mathf.Max(AbilityCooldown - TimeHandler.deltaTime, 0);

            if(armorHandler.GetArmorByType<SoulArmor>().CurrentArmorValue <= 0 && armorHandler.GetArmorByType<SoulArmor>().MaxArmorValue > 0) {
                armorHandler.GetArmorByType<SoulArmor>().MaxArmorValue = 0;
                AbilityCooldown = 10;
                abilityActive = false;
            }
        }
    }
}
