using AALUND13Card.MonoBehaviours;
using UnboundLib;
using UnityEngine;

namespace AALUND13Card.CustomCards
{
    public class CustomCardSoulstreakStats : CustomCardAACard
    {

        [Header("Health")]
        public float HealthMultiplyPerKill;
        public float HealPercentagePerKill;

        [Header("Soul Armor")]
        public float soulArmorPercentage;
        public float soulArmorPercentageRegenRate;

        [Header("Gun")]
        public float DamageMultiplyPerKill;
        public float ATKSpeedMultiplyPerKill;

        [Header("Other")]
        public float MovementSpeedMultiplyPerKill;
        public float BlockCooldownMultiplyPerKill;
        public AbilityType abilityType;

        public float SoulDrainMultiply;

        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            AALUND13_Cards.Instance.ExecuteAfterFrames(2, () =>
            {
                SoulstreakMono soulstreakObject = player.gameObject.GetComponentInChildren<SoulstreakMono>();
                SoulStreakStats soulStreakStats = new SoulStreakStats();

                soulStreakStats.HealthMultiplyPerKill = HealthMultiplyPerKill;
                soulStreakStats.HealPercentagePerKill = HealPercentagePerKill;

                soulStreakStats.soulArmorPercentage = soulArmorPercentage;
                soulStreakStats.soulArmorPercentageRegenRate = soulArmorPercentageRegenRate;

                soulStreakStats.DamageMultiplyPerKill = DamageMultiplyPerKill;
                soulStreakStats.ATKSpeedMultiplyPerKill = ATKSpeedMultiplyPerKill;

                soulStreakStats.MovementSpeedMultiplyPerKill = MovementSpeedMultiplyPerKill;
                soulStreakStats.BlockCooldownMultiplyPerKill = BlockCooldownMultiplyPerKill;
                soulStreakStats.abilityType = abilityType;

                soulStreakStats.SoulDrainMultiply = SoulDrainMultiply;

                soulstreakObject.soulstreakStats.AddStats(soulStreakStats);
            });
        }
    }
}