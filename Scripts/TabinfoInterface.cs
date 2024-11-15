using AALUND13Card.MonoBehaviours;
using TabInfo.Utils;

namespace AALUND13Card {
    public class TabinfoInterface {
        public static void Setup() {
            var cat = TabInfoManager.RegisterCategory("Soulstreak Stats", 7);

            // Health
            TabInfoManager.RegisterStat(cat, "Health Multiply Per Kill", (p) => p.GetComponentInChildren<SoulstreakMono>() != null && p.GetComponentInChildren<SoulstreakMono>().SoulstreakStats.HealthMultiplyPerKill != 1,
                (p) => $"{p.GetComponentInChildren<SoulstreakMono>().SoulstreakStats.HealthMultiplyPerKill * 100:0}%");
            TabInfoManager.RegisterStat(cat, "Heal Percentage Per Kill", (p) => p.GetComponentInChildren<SoulstreakMono>() != null && p.GetComponentInChildren<SoulstreakMono>().SoulstreakStats.HealPercentagePerKill != 0,
                (p) => $"{p.GetComponentInChildren<SoulstreakMono>().SoulstreakStats.HealPercentagePerKill * 100:0}%");

            // Soul Armor
            TabInfoManager.RegisterStat(cat, "Soul Armor Percentage", (p) => p.GetComponentInChildren<SoulstreakMono>() != null && p.GetComponentInChildren<SoulstreakMono>().SoulstreakStats.SoulArmorPercentage != 0,
                (p) => $"{p.GetComponentInChildren<SoulstreakMono>().SoulstreakStats.SoulArmorPercentage * 100:0}%");
            TabInfoManager.RegisterStat(cat, "Soul Armor Percentage Regen Rate", (p) => p.GetComponentInChildren<SoulstreakMono>() != null && p.GetComponentInChildren<SoulstreakMono>().SoulstreakStats.SoulArmorPercentageRegenRate != 0,
                (p) => $"{p.GetComponentInChildren<SoulstreakMono>().SoulstreakStats.SoulArmorPercentageRegenRate * 100:0}%");

            // Gun
            TabInfoManager.RegisterStat(cat, "Damage Multiply Per Kill", (p) => p.GetComponentInChildren<SoulstreakMono>() != null && p.GetComponentInChildren<SoulstreakMono>().SoulstreakStats.DamageMultiplyPerKill != 1,
                (p) => $"{p.GetComponentInChildren<SoulstreakMono>().SoulstreakStats.DamageMultiplyPerKill * 100:0}%");
            TabInfoManager.RegisterStat(cat, "ATK Speed Multiply Per Kill", (p) => p.GetComponentInChildren<SoulstreakMono>() != null && p.GetComponentInChildren<SoulstreakMono>().SoulstreakStats.ATKSpeedMultiplyPerKill != 1,
                (p) => $"{p.GetComponentInChildren<SoulstreakMono>().SoulstreakStats.ATKSpeedMultiplyPerKill * 100:0}%");

            // Other
            TabInfoManager.RegisterStat(cat, "Movement Speed Multiply Per Kill", (p) => p.GetComponentInChildren<SoulstreakMono>() != null && p.GetComponentInChildren<SoulstreakMono>().SoulstreakStats.MovementSpeedMultiplyPerKill != 1,
                (p) => $"{p.GetComponentInChildren<SoulstreakMono>().SoulstreakStats.MovementSpeedMultiplyPerKill * 100:0}%");
            TabInfoManager.RegisterStat(cat, "Block Cooldown Multiply Per Kill", (p) => p.GetComponentInChildren<SoulstreakMono>() != null && p.GetComponentInChildren<SoulstreakMono>().SoulstreakStats.BlockCooldownMultiplyPerKill != 1,
                (p) => $"{p.GetComponentInChildren<SoulstreakMono>().SoulstreakStats.BlockCooldownMultiplyPerKill * 100:0}%");

            // Souls
            TabInfoManager.RegisterStat(cat, "Souls", (p) => p.GetComponentInChildren<SoulstreakMono>() != null && p.GetComponentInChildren<SoulstreakMono>().KillsStreak != 0,
                (p) => $"{p.GetComponentInChildren<SoulstreakMono>().KillsStreak}");
        }
    }
}
