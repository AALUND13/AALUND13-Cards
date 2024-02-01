using AALUND13Card.MonoBehaviours;
using TabInfo.Utils;

namespace AALUND13Card
{
    public class TabinfoInterface
    {
        public static void Setup()
        {
            var cat = TabInfoManager.RegisterCategory("Soulstreak Stats", 7);

            // Health
            TabInfoManager.RegisterStat(cat, "Health Multiply Per Kill", (p) => p.GetComponentInChildren<SoulstreakMono>() != null && p.GetComponentInChildren<SoulstreakMono>().soulstreakStats.HealthMultiplyPerKill != 1,
                (p) => $"{p.GetComponentInChildren<SoulstreakMono>().soulstreakStats.HealthMultiplyPerKill * 100:0}%");
            TabInfoManager.RegisterStat(cat, "Heal Percentage Per Kill", (p) => p.GetComponentInChildren<SoulstreakMono>() != null && p.GetComponentInChildren<SoulstreakMono>().soulstreakStats.HealPercentagePerKill != 0,
                (p) => $"{p.GetComponentInChildren<SoulstreakMono>().soulstreakStats.HealPercentagePerKill * 100:0}%");

            // Soul Armor
            TabInfoManager.RegisterStat(cat, "Soul Armor Percentage", (p) => p.GetComponentInChildren<SoulstreakMono>() != null && p.GetComponentInChildren<SoulstreakMono>().soulstreakStats.soulArmorPercentage != 0,
                (p) => $"{p.GetComponentInChildren<SoulstreakMono>().soulstreakStats.soulArmorPercentage * 100:0}%");
            TabInfoManager.RegisterStat(cat, "Soul Armor Percentage Regen Rate", (p) => p.GetComponentInChildren<SoulstreakMono>() != null && p.GetComponentInChildren<SoulstreakMono>().soulstreakStats.soulArmorPercentageRegenRate != 0,
                (p) => $"{p.GetComponentInChildren<SoulstreakMono>().soulstreakStats.soulArmorPercentageRegenRate * 100:0}%");

            // Gun
            TabInfoManager.RegisterStat(cat, "Damage Multiply Per Kill", (p) => p.GetComponentInChildren<SoulstreakMono>() != null && p.GetComponentInChildren<SoulstreakMono>().soulstreakStats.DamageMultiplyPerKill != 1,
                (p) => $"{p.GetComponentInChildren<SoulstreakMono>().soulstreakStats.DamageMultiplyPerKill * 100:0}%");
            TabInfoManager.RegisterStat(cat, "ATK Speed Multiply Per Kill", (p) => p.GetComponentInChildren<SoulstreakMono>() != null && p.GetComponentInChildren<SoulstreakMono>().soulstreakStats.ATKSpeedMultiplyPerKill != 1,
                (p) => $"{p.GetComponentInChildren<SoulstreakMono>().soulstreakStats.ATKSpeedMultiplyPerKill * 100:0}%");

            // Other
            TabInfoManager.RegisterStat(cat, "Movement Speed Multiply Per Kill", (p) => p.GetComponentInChildren<SoulstreakMono>() != null && p.GetComponentInChildren<SoulstreakMono>().soulstreakStats.MovementSpeedMultiplyPerKill != 1,
                (p) => $"{p.GetComponentInChildren<SoulstreakMono>().soulstreakStats.MovementSpeedMultiplyPerKill * 100:0}%");
            TabInfoManager.RegisterStat(cat, "Block Cooldown Multiply Per Kill", (p) => p.GetComponentInChildren<SoulstreakMono>() != null && p.GetComponentInChildren<SoulstreakMono>().soulstreakStats.BlockCooldownMultiplyPerKill != 1,
                (p) => $"{p.GetComponentInChildren<SoulstreakMono>().soulstreakStats.BlockCooldownMultiplyPerKill * 100:0}%");
            
            // Souls
            TabInfoManager.RegisterStat(cat, "Souls", (p) => p.GetComponentInChildren<SoulstreakMono>() != null && p.GetComponentInChildren<SoulstreakMono>().killsStreak != 0,
                (p) => $"{p.GetComponentInChildren<SoulstreakMono>().killsStreak}");
        }
    }
}
