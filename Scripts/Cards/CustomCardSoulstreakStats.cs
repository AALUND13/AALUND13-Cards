using AALUND13Card.Extensions;
using AALUND13Card.MonoBehaviours;
using UnboundLib;
using UnityEngine;

namespace AALUND13Card.CustomCards {
    public class CustomCardSoulstreakStats : CustomCardAACard {

        [Header("Health")]
        public float HealthMultiplyPerKill;
        public float HealPercentagePerKill;

        [Header("Soul Armor")]
        public float SoulArmorPercentage;
        public float SoulArmorPercentageRegenRate;

        [Header("Gun")]
        public float DamageMultiplyPerKill;
        public float ATKSpeedMultiplyPerKill;

        [Header("Other")]
        public float MovementSpeedMultiplyPerKill;
        public float BlockCooldownMultiplyPerKill;
        public AbilityType AbilityType;

        public float SoulDrainMultiply;

        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats) {
            AALUND13_Cards.Instance.ExecuteAfterFrames(2, () => {
                SoulStreakStats soulStreakStats = new SoulStreakStats();

                soulStreakStats.HealthMultiplyPerKill = HealthMultiplyPerKill;
                soulStreakStats.HealPercentagePerKill = HealPercentagePerKill;

                soulStreakStats.SoulArmorPercentage = SoulArmorPercentage;
                soulStreakStats.SoulArmorPercentageRegenRate = SoulArmorPercentageRegenRate;

                soulStreakStats.DamageMultiplyPerKill = DamageMultiplyPerKill;
                soulStreakStats.ATKSpeedMultiplyPerKill = ATKSpeedMultiplyPerKill;

                soulStreakStats.MovementSpeedMultiplyPerKill = MovementSpeedMultiplyPerKill;
                soulStreakStats.BlockCooldownMultiplyPerKill = BlockCooldownMultiplyPerKill;
                soulStreakStats.AbilityType = AbilityType;

                soulStreakStats.SoulDrainMultiply = SoulDrainMultiply;

                data.GetAdditionalData().SoulStreakStats.AddStats(soulStreakStats);
            });
        }
    }
}