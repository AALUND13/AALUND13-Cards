using AALUND13Cards.Classes.Cards;
using AALUND13Cards.Classes.MonoBehaviours.CardsEffects.Soulstreak;
using AALUND13Cards.Classes.Utils;
using AALUND13Cards.Core.Extensions;
using TabInfo.Utils;
using UnityEngine;

namespace AALUND13Cards.ExtraCards {
    internal class TabinfoInterface {
        public static void Setup() {
            var aaStatsCategory = AALUND13Cards.Core.TabinfoInterface.GetOrCreateCategory("AA Stats", 6);

            #region Reaper Stats
            // Percentage Damage Stats
            TabInfoManager.RegisterStat(aaStatsCategory, "Scaling Percentage Damage", (p) => GetReaperStatsFromPlayer(p).ScalingPercentageDamage != 0,
                (p) => $"{(Mathf.Min(GetReaperStatsFromPlayer(p).ScalingPercentageDamage, Mathf.Min(GetReaperStatsFromPlayer(p).ScalingPercentageDamageCap, MathUtils.PERCENT_CAP)) + GetReaperStatsFromPlayer(p).ScalingPercentageDamageUnCap) * 100:0}%");
            TabInfoManager.RegisterStat(aaStatsCategory, "Effective Percentage Damage", (p) => GetReaperStatsFromPlayer(p).ScalingPercentageDamage != 0,
                (p) => $"{(MathUtils.GetEffectivePercentCap(p.GetSPS(), GetReaperStatsFromPlayer(p).ScalingPercentageDamage, GetReaperStatsFromPlayer(p).ScalingPercentageDamageCap) + MathUtils.GetEffectivePercent(p.GetSPS(), GetReaperStatsFromPlayer(p).ScalingPercentageDamageUnCap)) * 100:0}%");
            #endregion

            #region Soulstreak Stats
            var category = TabInfoManager.RegisterCategory("Soulstreak Stats", 7);

            // Character Stats
            TabInfoManager.RegisterStat(category, "Max Health Per Kill", (p) => p.GetComponentInChildren<SoulstreakMono>() != null && GetSoulstreakStatsFromPlayer(p).MaxHealth != 0,
                (p) => $"{GetSoulstreakStatsFromPlayer(p).MaxHealth * 100:0}%");
            TabInfoManager.RegisterStat(category, "player Size Per Kill", (p) => p.GetComponentInChildren<SoulstreakMono>() != null && GetSoulstreakStatsFromPlayer(p).PlayerSize != 0,
                (p) => $"{GetSoulstreakStatsFromPlayer(p).PlayerSize * 100:0}%");
            TabInfoManager.RegisterStat(category, "Movement Speed Per Kill", (p) => p.GetComponentInChildren<SoulstreakMono>() != null && GetSoulstreakStatsFromPlayer(p).MovementSpeed != 0,
                (p) => $"{GetSoulstreakStatsFromPlayer(p).MovementSpeed * 100:0}%");

            // Gun Stats
            TabInfoManager.RegisterStat(category, "Attack Speed Pre Kill", (p) => p.GetComponentInChildren<SoulstreakMono>() != null && GetSoulstreakStatsFromPlayer(p).AttackSpeed != 0,
                (p) => $"{GetSoulstreakStatsFromPlayer(p).AttackSpeed * 100:0}%");
            TabInfoManager.RegisterStat(category, "Damage Per Kill", (p) => p.GetComponentInChildren<SoulstreakMono>() != null && GetSoulstreakStatsFromPlayer(p).Damage != 0,
                (p) => $"{GetSoulstreakStatsFromPlayer(p).Damage * 100:0}%");
            TabInfoManager.RegisterStat(category, "Bullet Speed Per Kill", (p) => p.GetComponentInChildren<SoulstreakMono>() != null && GetSoulstreakStatsFromPlayer(p).BulletSpeed != 0,
                (p) => $"{GetSoulstreakStatsFromPlayer(p).BulletSpeed * 100:0}%");

            // Soul Armor
            TabInfoManager.RegisterStat(category, "Soul Armor Percentage", (p) => p.GetComponentInChildren<SoulstreakMono>() != null && GetSoulstreakStatsFromPlayer(p).SoulArmorPercentage != 0,
                (p) => $"{GetSoulstreakStatsFromPlayer(p).SoulArmorPercentage * 100:0}%");
            TabInfoManager.RegisterStat(category, "Soul Armor Percentage Regen Rate", (p) => p.GetComponentInChildren<SoulstreakMono>() != null && GetSoulstreakStatsFromPlayer(p).SoulArmorPercentageRegenRate != 0,
                (p) => $"{GetSoulstreakStatsFromPlayer(p).SoulArmorPercentageRegenRate * 100:0}%");

            // Soul Drain
            TabInfoManager.RegisterStat(category, "Soul Drain DPS Factor", (p) => p.GetComponentInChildren<SoulstreakMono>() != null && GetSoulstreakStatsFromPlayer(p).SoulDrainDPSFactor != 0,
                               (p) => $"{GetSoulstreakStatsFromPlayer(p).SoulDrainDPSFactor * 100:0}%");

            // Souls
            TabInfoManager.RegisterStat(category, "Souls", (p) => p.GetComponentInChildren<SoulstreakMono>() != null && GetSoulstreakStatsFromPlayer(p).Souls != 0,
                (p) => $"{GetSoulstreakStatsFromPlayer(p).Souls}");
            #endregion

            #region Railgun Stats
            var railgunCategory = TabInfoManager.RegisterCategory("Railgun Stats", 8);

            // Charge Stats
            TabInfoManager.RegisterStat(railgunCategory, "Charge", (p) => GetRailgunStatsFromPlayer(p).IsEnabled && GetRailgunStatsFromPlayer(p).MaximumCharge != 0,
                (p) => $"{GetRailgunStatsFromPlayer(p).CurrentCharge:0.00}/{GetRailgunStatsFromPlayer(p).MaximumCharge:0.00}");
            TabInfoManager.RegisterStat(railgunCategory, "Charge Rate", (p) => GetRailgunStatsFromPlayer(p).IsEnabled && GetRailgunStatsFromPlayer(p).ChargeRate != 0,
                (p) => $"{GetRailgunStatsFromPlayer(p).ChargeRate:0.00}/s");

            // Gun Stats
            TabInfoManager.RegisterStat(railgunCategory, "Railgun Damage Multiplier", (p) => GetRailgunStatsFromPlayer(p).IsEnabled && GetRailgunStatsFromPlayer(p).RailgunDamageMultiplier != 1f,
                (p) => $"{GetRailgunStatsFromPlayer(p).RailgunDamageMultiplier * 100:0}%");
            TabInfoManager.RegisterStat(railgunCategory, "Railgun Bullet Speed Multiplier", (p) => GetRailgunStatsFromPlayer(p).IsEnabled && GetRailgunStatsFromPlayer(p).RailgunBulletSpeedMultiplier != 1f,
                (p) => $"{GetRailgunStatsFromPlayer(p).RailgunBulletSpeedMultiplier * 100:0}%");
            TabInfoManager.RegisterStat(railgunCategory, "Railgun Full Charge Threshold", (p) => GetRailgunStatsFromPlayer(p).IsEnabled && GetRailgunStatsFromPlayer(p).FullChargeThreshold != 0,
                (p) => $"{GetRailgunStatsFromPlayer(p).FullChargeThreshold:0.00}");
            #endregion
        }


        private static RailgunStats GetRailgunStatsFromPlayer(Player player) {
            return player.data.GetAdditionalData().CustomStatsRegistry.GetOrCreate<RailgunStats>();
        }

        private static SoulStreakStats GetSoulstreakStatsFromPlayer(Player player) {
            return player.data.GetAdditionalData().CustomStatsRegistry.GetOrCreate<SoulStreakStats>();
        }

        private static ReaperStats GetReaperStatsFromPlayer(Player player) {
            return player.data.GetAdditionalData().CustomStatsRegistry.GetOrCreate<ReaperStats>();
        }
    }
}
