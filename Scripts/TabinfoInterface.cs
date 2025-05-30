using AALUND13Card.Extensions;
using AALUND13Card.MonoBehaviours;
using TabInfo.Utils;

namespace AALUND13Card {
    public class TabinfoInterface {
        public static void Setup() {
            var aaStatsCategory = TabInfoManager.RegisterCategory("AA Stats", 6);
            TabInfoManager.RegisterStat(aaStatsCategory, "Random Cards At Start", (p) => p.data.GetAdditionalData().RandomCardsAtStart != 0,
                (p) => $"{p.data.GetAdditionalData().RandomCardsAtStart}");
            TabInfoManager.RegisterStat(aaStatsCategory, "Extra Card Picks", (p) => p.data.GetAdditionalData().ExtraCardPicks != 0,
                (p) => $"{p.data.GetAdditionalData().ExtraCardPicks}");
            TabInfoManager.RegisterStat(aaStatsCategory, "Delay Damage", (p) => p.data.GetAdditionalData().secondToDealDamage != 0,
                (p) => $"{p.data.GetAdditionalData().secondToDealDamage} seconds");
            TabInfoManager.RegisterStat(aaStatsCategory, "DPS", (_) => true,
                (p) => $"{p.GetDPS()}");

            var category = TabInfoManager.RegisterCategory("Soulstreak Stats", 7);

            // Character Stats
            TabInfoManager.RegisterStat(category, "Max Health Per Kill", (p) => p.GetComponentInChildren<SoulstreakMono>() != null && p.data.GetAdditionalData().SoulStreakStats.MaxHealth != 0,
                (p) => $"{p.data.GetAdditionalData().SoulStreakStats.MaxHealth * 100:0}%");
            TabInfoManager.RegisterStat(category, "Player Size Per Kill", (p) => p.GetComponentInChildren<SoulstreakMono>() != null && p.data.GetAdditionalData().SoulStreakStats.PlayerSize != 0,
                (p) => $"{p.data.GetAdditionalData().SoulStreakStats.PlayerSize * 100:0}%");
            TabInfoManager.RegisterStat(category, "Movement Speed Per Kill", (p) => p.GetComponentInChildren<SoulstreakMono>() != null && p.data.GetAdditionalData().SoulStreakStats.MovementSpeed != 0,
                (p) => $"{p.data.GetAdditionalData().SoulStreakStats.MovementSpeed * 100:0}%");

            // Gun Stats
            TabInfoManager.RegisterStat(category, "Attack Speed Pre Kill", (p) => p.GetComponentInChildren<SoulstreakMono>() != null && p.data.GetAdditionalData().SoulStreakStats.AttackSpeed != 0,
                (p) => $"{p.data.GetAdditionalData().SoulStreakStats.AttackSpeed * 100:0}%");
            TabInfoManager.RegisterStat(category, "Damage Per Kill", (p) => p.GetComponentInChildren<SoulstreakMono>() != null && p.data.GetAdditionalData().SoulStreakStats.Damage != 0,
                (p) => $"{p.data.GetAdditionalData().SoulStreakStats.Damage * 100:0}%");
            TabInfoManager.RegisterStat(category, "Bullet Speed Per Kill", (p) => p.GetComponentInChildren<SoulstreakMono>() != null && p.data.GetAdditionalData().SoulStreakStats.BulletSpeed != 0,
                (p) => $"{p.data.GetAdditionalData().SoulStreakStats.BulletSpeed * 100:0}%");

            // Soul Armor
            TabInfoManager.RegisterStat(category, "Soul Armor Percentage", (p) => p.GetComponentInChildren<SoulstreakMono>() != null && p.data.GetAdditionalData().SoulStreakStats.SoulArmorPercentage != 0,
                (p) => $"{p.data.GetAdditionalData().SoulStreakStats.SoulArmorPercentage * 100:0}%");
            TabInfoManager.RegisterStat(category, "Soul Armor Percentage Regen Rate", (p) => p.GetComponentInChildren<SoulstreakMono>() != null && p.data.GetAdditionalData().SoulStreakStats.SoulArmorPercentageRegenRate != 0,
                (p) => $"{p.data.GetAdditionalData().SoulStreakStats.SoulArmorPercentageRegenRate * 100:0}%");

            // Soul Drain
            TabInfoManager.RegisterStat(category, "Soul Drain DPS Factor", (p) => p.GetComponentInChildren<SoulstreakMono>() != null && p.data.GetAdditionalData().SoulStreakStats.SoulDrainDPSFactor != 0,
                               (p) => $"{p.data.GetAdditionalData().SoulStreakStats.SoulDrainDPSFactor * 100:0}%");

            // Souls
            TabInfoManager.RegisterStat(category, "Souls", (p) => p.GetComponentInChildren<SoulstreakMono>() != null && p.data.GetAdditionalData().Souls != 0,
                (p) => $"{p.data.GetAdditionalData().Souls}");
        }
    }
}
