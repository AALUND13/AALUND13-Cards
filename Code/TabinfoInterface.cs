using System;
using TabInfo.Utils;
using UnboundLib;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using HarmonyLib;
using Landfall.Network;
using Photon.Pun;
using System.Reflection;
using UnboundLib.Utils;
using Photon.Realtime;

internal class TabinfoInterface
{
    public static string positiveOrNegative(float value)
    {
        if (value > 0) return "+";
        return "";
    }
    public static void Setup()
    {
        StatCategory catSS = TabInfoManager.RegisterCategory("Soulstreak Shield", 0);
        ExtensionMethods.SetFieldValue(catSS, "priority", -6);
        TabInfoManager.RegisterStat(catSS, "Soul Shield", (p) => p.GetComponentInChildren<AAStatsModifiers>() != null && p.GetComponentInChildren<SoulstreakObject>().soulShieldMaxHealth > 0, (p) => $"{(p.GetComponentInChildren<SoulstreakObject>().soulShieldDepleted ? "Depleted" : p.GetComponentInChildren<SoulstreakObject>().soulShieldHealth.ToString()+"/"+p.GetComponentInChildren<SoulstreakObject>().soulShieldMaxHealth)}");
        var cat = TabInfoManager.RegisterCategory("Soulstreak Stats", 7);
        TabInfoManager.RegisterStat(cat, "Damage Multiply Per Kill", (p) => p.GetComponentInChildren<AAStatsModifiers>() != null, (p) => $"{positiveOrNegative(p.GetComponentInChildren<AAStatsModifiers>().DamageMultiplyPerKill - 1)}{p.GetComponentInChildren<AAStatsModifiers>().DamageMultiplyPerKill * 100 - 100}%");
        TabInfoManager.RegisterStat(cat, "Movement Speed Multiply Per Kill", (p) => p.GetComponentInChildren<AAStatsModifiers>() != null, (p) => $"{positiveOrNegative(p.GetComponentInChildren<AAStatsModifiers>().MovementSpeedMultiplyPerKill - 1)}{p.GetComponentInChildren<AAStatsModifiers>().MovementSpeedMultiplyPerKill * 100 - 100}%");
        TabInfoManager.RegisterStat(cat, "ATK Speed Multiply Per Kill", (p) => p.GetComponentInChildren<AAStatsModifiers>() != null, (p) => $"{positiveOrNegative(p.GetComponentInChildren<AAStatsModifiers>().ATKSpeedMultiplyPerKill - 1)}{p.GetComponentInChildren<AAStatsModifiers>().ATKSpeedMultiplyPerKill * 100 - 100}%");
        TabInfoManager.RegisterStat(cat, "Block Cooldown Multiply Per Kill", (p) => p.GetComponentInChildren<AAStatsModifiers>() != null, (p) => $"{positiveOrNegative(p.GetComponentInChildren<AAStatsModifiers>().BlockCooldownMultiplyPerKill - 1)}{p.GetComponentInChildren<AAStatsModifiers>().BlockCooldownMultiplyPerKill * 100 - 100}%");
        TabInfoManager.RegisterStat(cat, "Health Multiply Per Kill", (p) => p.GetComponentInChildren<AAStatsModifiers>() != null, (p) => $"{positiveOrNegative(p.GetComponentInChildren<AAStatsModifiers>().HealthMultiplyPerKill - 1)}{p.GetComponentInChildren<AAStatsModifiers>().HealthMultiplyPerKill * 100 - 100}%");
        TabInfoManager.RegisterStat(cat, "Max Multiply Per Kill", (p) => p.GetComponentInChildren<AAStatsModifiers>() != null, (p) => $"{positiveOrNegative(p.GetComponentInChildren<AAStatsModifiers>().MaxMultiplyPerKill - 1)}{p.GetComponentInChildren<AAStatsModifiers>().MaxMultiplyPerKill * 100 - 100}%");
        TabInfoManager.RegisterStat(cat, "Heal Percentage Per Kill", (p) => p.GetComponentInChildren<AAStatsModifiers>() != null, (p) => $"{p.GetComponentInChildren<AAStatsModifiers>().HealPercentagePerKill * 100}%");
        //Soul Shield
        TabInfoManager.RegisterStat(catSS, "Soul Shield Percentage", (p) => p.GetComponentInChildren<AAStatsModifiers>() != null && p.GetComponentInChildren<SoulstreakObject>().soulShieldMaxHealth > 0, (p) => $"{p.GetComponentInChildren<AAStatsModifiers>().soulShieldPercentage * 100}%");
        TabInfoManager.RegisterStat(catSS, "Soul Shield Percentage Regen", (p) => p.GetComponentInChildren<AAStatsModifiers>() != null && p.GetComponentInChildren<SoulstreakObject>().soulShieldMaxHealth > 0, (p) => $"{p.GetComponentInChildren<AAStatsModifiers>().soulShieldPercentageRegen * 100}%");
    }
}