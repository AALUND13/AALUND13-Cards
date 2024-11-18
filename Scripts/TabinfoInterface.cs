using AALUND13Card.Extensions;
using AALUND13Card.MonoBehaviours;
using TabInfo.Utils;

namespace AALUND13Card {
    public class TabinfoInterface {
        public static void Setup() {
            var cat = TabInfoManager.RegisterCategory("Soulstreak Stats", 7);

            // Health
            TabInfoManager.RegisterStat(cat, "Health Multiply Per Kill", (p) => p.GetComponentInChildren<SoulstreakMono>() != null && p.data.GetAdditionalData().SoulStreakStats.HealthMultiplyPerKill != 1,
                (p) => $"{p.data.GetAdditionalData().SoulStreakStats.HealthMultiplyPerKill * 100:0}%");
            TabInfoManager.RegisterStat(cat, "Heal Percentage Per Kill", (p) => p.GetComponentInChildren<SoulstreakMono>() != null && p.data.GetAdditionalData().SoulStreakStats.HealPercentagePerKill != 0,
                (p) => $"{p.data.GetAdditionalData().SoulStreakStats.HealPercentagePerKill * 100:0}%");

            // Soul Armor
            TabInfoManager.RegisterStat(cat, "Soul Armor Percentage", (p) => p.GetComponentInChildren<SoulstreakMono>() != null && p.data.GetAdditionalData().SoulStreakStats.SoulArmorPercentage != 0,
                (p) => $"{p.data.GetAdditionalData().SoulStreakStats.SoulArmorPercentage * 100:0}%");
            TabInfoManager.RegisterStat(cat, "Soul Armor Percentage Regen Rate", (p) => p.GetComponentInChildren<SoulstreakMono>() != null && p.data.GetAdditionalData().SoulStreakStats.SoulArmorPercentageRegenRate != 0,
                (p) => $"{p.data.GetAdditionalData().SoulStreakStats.SoulArmorPercentageRegenRate * 100:0}%");

            // Gun
            TabInfoManager.RegisterStat(cat, "Damage Multiply Per Kill", (p) => p.GetComponentInChildren<SoulstreakMono>() != null && p.data.GetAdditionalData().SoulStreakStats.DamageMultiplyPerKill != 1,
                (p) => $"{p.data.GetAdditionalData().SoulStreakStats.DamageMultiplyPerKill * 100:0}%");
            TabInfoManager.RegisterStat(cat, "ATK Speed Multiply Per Kill", (p) => p.GetComponentInChildren<SoulstreakMono>() != null && p.data.GetAdditionalData().SoulStreakStats.ATKSpeedMultiplyPerKill != 1,
                (p) => $"{p.data.GetAdditionalData().SoulStreakStats.ATKSpeedMultiplyPerKill * 100:0}%");

            // Other
            TabInfoManager.RegisterStat(cat, "Movement Speed Multiply Per Kill", (p) => p.GetComponentInChildren<SoulstreakMono>() != null && p.data.GetAdditionalData().SoulStreakStats.MovementSpeedMultiplyPerKill != 1,
                (p) => $"{p.data.GetAdditionalData().SoulStreakStats.MovementSpeedMultiplyPerKill * 100:0}%");
            TabInfoManager.RegisterStat(cat, "Block Cooldown Multiply Per Kill", (p) => p.GetComponentInChildren<SoulstreakMono>() != null && p.data.GetAdditionalData().SoulStreakStats.BlockCooldownMultiplyPerKill != 1,
                (p) => $"{p.data.GetAdditionalData().SoulStreakStats.BlockCooldownMultiplyPerKill * 100:0}%");

            // Souls
            TabInfoManager.RegisterStat(cat, "Souls", (p) => p.GetComponentInChildren<SoulstreakMono>() != null && p.data.GetAdditionalData().Killstreak != 0,
                (p) => $"{p.data.GetAdditionalData().Killstreak}");
        }
    }
}
