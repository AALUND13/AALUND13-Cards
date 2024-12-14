using AALUND13Card.Extensions;
using AALUND13Card.MonoBehaviours;
using UnboundLib;
using UnityEngine;

namespace AALUND13Card.Cards {
    public class SoulstreakStatsCard : CustomCardAACard {

        [Header("Stats")]
        public float
            MaxHealth = 0,
            PlayerSize = 0,
            MovementSpeed = 0,

            AttackSpeed = 0,
            Damage = 0,
            BulletSpeed = 0;

        [Header("Soul Armor")]
        public float SoulArmorPercentage;
        public float SoulArmorPercentageRegenRate;

        [Header("Ability")]
        public AbilityType AbilityType;
        public float SoulDrainMultiply;

        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats) {
            AALUND13_Cards.Instance.ExecuteAfterFrames(2, () => {
                SoulStreakStats soulStreakStats = new SoulStreakStats();

                soulStreakStats.AttackSpeed = AttackSpeed;
                soulStreakStats.MovementSpeed = MovementSpeed;
                soulStreakStats.Damage = Damage;
                soulStreakStats.PlayerSize = PlayerSize;
                soulStreakStats.MaxHealth = MaxHealth;
                soulStreakStats.BulletSpeed = BulletSpeed;

                soulStreakStats.SoulArmorPercentage = SoulArmorPercentage;
                soulStreakStats.SoulArmorPercentageRegenRate = SoulArmorPercentageRegenRate;

                soulStreakStats.SoulDrainMultiply = SoulDrainMultiply;

           
                soulStreakStats.AbilityType |= AbilityType;

                data.GetAdditionalData().SoulStreakStats.AddStats(soulStreakStats);
            });
        }
    }
}