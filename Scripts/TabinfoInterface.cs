using AALUND13Cards.Extensions;
using AALUND13Cards.MonoBehaviours.CardsEffects.Soulstreak;
using TabInfo.Utils;

namespace AALUND13Cards {
    public class TabinfoInterface {
        public static void Setup() {
            var aaStatsCategory = TabInfoManager.RegisterCategory("AA Stats", 6);

            // Delayed Damage
            TabInfoManager.RegisterStat(aaStatsCategory, "Delay Damage", (p) => p.data.GetAdditionalData().secondToDealDamage != 0,
                (p) => $"{p.data.GetAdditionalData().secondToDealDamage} seconds");

            // Extra Cards
            TabInfoManager.RegisterStat(aaStatsCategory, "Random Cards At Start", (p) => p.data.GetAdditionalData().RandomCardsAtStart != 0,
                (p) => $"{p.data.GetAdditionalData().RandomCardsAtStart}");
            TabInfoManager.RegisterStat(aaStatsCategory, "Extra Card Picks", (p) => p.data.GetAdditionalData().ExtraCardPicks != 0,
                (p) => $"{p.data.GetAdditionalData().ExtraCardPicks}");

            // Armor Stats
            TabInfoManager.RegisterStat(aaStatsCategory, "Damage Against Armor Percentage", (p) => p.data.GetAdditionalData().DamageAgainstArmorPercentage != 1f,
                (p) => $"{p.data.GetAdditionalData().DamageAgainstArmorPercentage * 100:0}%");

            // Blocks Stats
            TabInfoManager.RegisterStat(aaStatsCategory, "Blocks When Recharge", (p) => p.data.GetAdditionalData().BlocksWhenRecharge != 0,
                (p) => $"{p.data.GetAdditionalData().BlocksWhenRecharge}");
            TabInfoManager.RegisterStat(aaStatsCategory, "Block Pierce Percent", (p) => p.data.GetAdditionalData().BlockPircePercent != 0,
                (p) => $"{p.data.GetAdditionalData().BlockPircePercent * 100:0}%");

            // Uncategorized Stats
            TabInfoManager.RegisterStat(aaStatsCategory, "Current HP Regen Percentage", (p) => p.data.GetAdditionalData().CurrentHPRegenPercentage != 0,
                (p) => $"{p.data.GetAdditionalData().CurrentHPRegenPercentage * 100:0}%");
            TabInfoManager.RegisterStat(aaStatsCategory, "Damage Reduction", (p) => p.data.GetAdditionalData().DamageReduction != 0,
                (p) => $"{p.data.GetAdditionalData().DamageReduction * 100:0}%");
            TabInfoManager.RegisterStat(aaStatsCategory, "DPS", (_) => true,
                (p) => $"{p.GetDPS()}");
            TabInfoManager.RegisterStat(aaStatsCategory, "Is Bind", (p) => p.data.GetAdditionalData().isBind,
                (p) => p.data.GetAdditionalData().isBind ? "Yes" : "No");

            #region Soulstreak Stats
            var category = TabInfoManager.RegisterCategory("Soulstreak Stats", 7);

            // Character Stats
            TabInfoManager.RegisterStat(category, "Max Health Per Kill", (p) => p.GetComponentInChildren<SoulstreakMono>() != null && p.data.GetAdditionalData().SoulStreakStats.MaxHealth != 0,
                (p) => $"{p.data.GetAdditionalData().SoulStreakStats.MaxHealth * 100:0}%");
            TabInfoManager.RegisterStat(category, "player Size Per Kill", (p) => p.GetComponentInChildren<SoulstreakMono>() != null && p.data.GetAdditionalData().SoulStreakStats.PlayerSize != 0,
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
            #endregion

            #region Railgun Stats
            var railgunCategory = TabInfoManager.RegisterCategory("Railgun Stats", 8);

            // Charge Stats
            TabInfoManager.RegisterStat(railgunCategory, "Charge", (p) => p.data.GetAdditionalData().RailgunStats.IsEnabled && p.data.GetAdditionalData().RailgunStats.MaximumCharge != 0,
                (p) => $"{p.data.GetAdditionalData().RailgunStats.CurrentCharge:0.00}/{p.data.GetAdditionalData().RailgunStats.MaximumCharge:0.00}");
            TabInfoManager.RegisterStat(railgunCategory, "Charge Rate", (p) => p.data.GetAdditionalData().RailgunStats.IsEnabled && p.data.GetAdditionalData().RailgunStats.ChargeRate != 0,
                (p) => $"{p.data.GetAdditionalData().RailgunStats.ChargeRate:0.00}/s");

            // Gun Stats
            TabInfoManager.RegisterStat(railgunCategory, "Railgun Damage Multiplier", (p) => p.data.GetAdditionalData().RailgunStats.IsEnabled && p.data.GetAdditionalData().RailgunStats.RailgunDamageMultiplier != 1f,
                (p) => $"{p.data.GetAdditionalData().RailgunStats.RailgunDamageMultiplier * 100:0}%");
            TabInfoManager.RegisterStat(railgunCategory, "Railgun Bullet Speed Multiplier", (p) => p.data.GetAdditionalData().RailgunStats.IsEnabled && p.data.GetAdditionalData().RailgunStats.RailgunBulletSpeedMultiplier != 1f,
                (p) => $"{p.data.GetAdditionalData().RailgunStats.RailgunBulletSpeedMultiplier * 100:0}%");
            TabInfoManager.RegisterStat(railgunCategory, "Railgun Full Charge Threshold", (p) => p.data.GetAdditionalData().RailgunStats.IsEnabled && p.data.GetAdditionalData().RailgunStats.FullChargeThreshold != 0,
                (p) => $"{p.data.GetAdditionalData().RailgunStats.FullChargeThreshold:0.00}");
            #endregion
        }
    }
}
