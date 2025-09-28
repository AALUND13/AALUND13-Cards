using AALUND13Cards.Extensions;
using AALUND13Cards.MonoBehaviours.CardsEffects;
using AALUND13Cards.MonoBehaviours.CardsEffects.Soulstreak;
using AALUND13Cards.Utils;
using TabInfo.Utils;
using UnityEngine;

namespace AALUND13Cards {
    public class TabinfoInterface {
        public static void Setup() {
            var aaStatsCategory = TabInfoManager.RegisterCategory("AA Stats", 6);

            // Delayed Damage
            TabInfoManager.RegisterStat(aaStatsCategory, "Delay Damage", (p) => p.data.GetAdditionalData().secondToDealDamage != 0,
                (p) => $"{p.data.GetAdditionalData().secondToDealDamage} seconds");

            // Extra Cards
            TabInfoManager.RegisterStat(aaStatsCategory, "Extra Card Picks", (p) => p.data.GetAdditionalData().ExtraCardPicks != 0,
                (p) => $"{p.data.GetAdditionalData().ExtraCardPicks}");
            TabInfoManager.RegisterStat(aaStatsCategory, "Duplicates As Corrupted", (p) => p.data.GetAdditionalData().DuplicatesAsCorrupted != 0,
                (p) => $"{p.data.GetAdditionalData().DuplicatesAsCorrupted}");

            // Armor Stats
            TabInfoManager.RegisterStat(aaStatsCategory, "Damage Against Armor Percentage", (p) => p.data.GetAdditionalData().DamageAgainstArmorPercentage != 1f,
                (p) => $"{p.data.GetAdditionalData().DamageAgainstArmorPercentage * 100:0}%");
            TabInfoManager.RegisterStat(aaStatsCategory, "Armor Damage Reduction", (p) => p.data.GetAdditionalData().ArmorDamageReduction != 0f,
                (p) => $"{p.data.GetAdditionalData().ArmorDamageReduction * 100:0}%");

            // Blocks Stats
            TabInfoManager.RegisterStat(aaStatsCategory, "Blocks When Recharge", (p) => p.data.GetAdditionalData().BlocksWhenRecharge != 0,
                (p) => $"{p.data.GetAdditionalData().BlocksWhenRecharge}");
            TabInfoManager.RegisterStat(aaStatsCategory, "Block Pierce Percent", (p) => p.data.GetAdditionalData().BlockPircePercent != 0,
                (p) => $"{p.data.GetAdditionalData().BlockPircePercent * 100:0}%");
            TabInfoManager.RegisterStat(aaStatsCategory, "Stun Block Time", (p) => p.data.GetAdditionalData().StunBlockTime != 0,
                (p) => $"{p.data.GetAdditionalData().StunBlockTime:0}s");

            // Curses Stats
            TabInfoManager.RegisterStat(aaStatsCategory, "Max Rarity For Curse", (p) => p.data.GetAdditionalData().MaxRarityForCurse != null,
                (p) => p.data.GetAdditionalData().MaxRarityForCurse.name);
            TabInfoManager.RegisterStat(aaStatsCategory, "Is Bind", (p) => p.data.GetAdditionalData().IsBind,
                (p) => p.data.GetAdditionalData().IsBind ? "Yes" : "No");
            TabInfoManager.RegisterStat(aaStatsCategory, "Is Decay Time Disable", (p) => p.data.GetAdditionalData().DisableDecayTime,
                (p) => p.data.GetAdditionalData().DisableDecayTime ? "Yes" : "No");

            // Uncategorized Stats
            TabInfoManager.RegisterStat(aaStatsCategory, "Damage Reduction", (p) => p.data.GetAdditionalData().DamageReduction != 0,
                (p) => $"{p.data.GetAdditionalData().DamageReduction * 100:0}%");
            TabInfoManager.RegisterStat(aaStatsCategory, "Damage Per Seconds", (_) => true,
                (p) => $"{p.GetDPS()}");
            TabInfoManager.RegisterStat(aaStatsCategory, "Bullets Per Seconds", (_) => true,
                (p) => $"{p.GetSPS()}");

            // Percentage Damage Stats
            TabInfoManager.RegisterStat(aaStatsCategory, "Scaling Percentage Damage", (p) => p.data.GetAdditionalData().ScalingPercentageDamage != 0,
                (p) => $"{(Mathf.Min(p.data.GetAdditionalData().ScalingPercentageDamage, Mathf.Min(p.data.GetAdditionalData().ScalingPercentageDamageCap, MathUtils.PERCENT_CAP)) + p.data.GetAdditionalData().ScalingPercentageDamageUnCap) * 100:0}%");
            TabInfoManager.RegisterStat(aaStatsCategory, "Effective Percentage Damage", (p) => p.data.GetAdditionalData().ScalingPercentageDamage != 0,
                (p) => $"{(MathUtils.GetEffectivePercentCap(p.GetSPS(), p.data.GetAdditionalData().ScalingPercentageDamage, p.data.GetAdditionalData().ScalingPercentageDamageCap) + MathUtils.GetEffectivePercent(p.GetSPS(), p.data.GetAdditionalData().ScalingPercentageDamageUnCap)) * 100:0}%");

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
    }
}
