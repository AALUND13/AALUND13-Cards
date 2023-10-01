using System.Collections;
using UnityEngine;
using UnboundLib;
using TMPro;
using ModdingUtils.Extensions;
using UnboundLib.Extensions;
using ModdingUtils;

public class Soulstreak : AAStatsModifiers
{
    public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats, CardInfo cardInfo)
    {
        UnityEngine.Debug.Log("Card added");
        // Find an existing GameObject with the SoulstreakStatsModifiers component, or create a new one.
        Transform soulstreakStatsTransform = player.transform.Find("Soulstreak");
        GameObject soulstreakStatsObject = null;
        if (soulstreakStatsTransform == null)
        {
            UnityEngine.Debug.Log("Craeting Soulstreak Object");
            soulstreakStatsObject = new GameObject("Soulstreak");
            soulstreakStatsObject.transform.parent = player.transform;
            soulstreakStatsObject.transform.localPosition = new Vector3(0, 3.8f, 0);
        }
        else
        {
            soulstreakStatsObject = player.transform.Find("Soulstreak").gameObject;
        }
        // Get or add the SoulstreakStatsModifiers component to the found/created GameObject.
        UnityEngine.Debug.Log("Getting Or Adding SoulstreakStatsModifiers Component");
        SoulstreakStats soulstreakStats = soulstreakStatsObject.GetOrAddComponent<SoulstreakStats>();
        UnityEngine.Debug.Log("Getting Or Adding SoulstreakObject Component");
        SoulstreakObject soulstreak = soulstreakStatsObject.GetOrAddComponent<SoulstreakObject>();
        UnityEngine.Debug.Log("Getting Or Adding TextMeshPro Component");
        TextMeshPro textMeshPro = soulstreakStatsObject.GetOrAddComponent<TextMeshPro>();
        textMeshPro.alignment = TextAlignmentOptions.Bottom;
        textMeshPro.color = new Color(0.8f,0,0.8f,1);
        textMeshPro.fontSize = 5;
        // Initialize default values if they are not set.
        if (soulstreakStats.ATkSpeedMultiplyPerKill == 0)
        {
            soulstreakStats.ATkSpeedMultiplyPerKill = 1;
            soulstreakStats.DamageMultiplyPerKill = 1;
            soulstreakStats.MovementSpeedMultiplyPerKill = 1;
            soulstreakStats.BlockCooldownMultiplyPerKill = 1;
            soulstreakStats.MaxMultiplyPerKill = 1;
            soulstreakStats.HealthMultiplyPerKill = 1;
        }

        // Get the SoulstreakStatsModifiers component from the card, if it exists.
        SoulstreakStats cardSoulstreakStats = cardInfo.GetComponent<SoulstreakStats>();

        if (cardSoulstreakStats != null)
        {
            // Update the SoulstreakStatsModifiers values based on the card's values.
            soulstreakStats.ATkSpeedMultiplyPerKill = Mathf.Max(soulstreakStats.ATkSpeedMultiplyPerKill + cardSoulstreakStats.ATkSpeedMultiplyPerKill, 0.5f);
            soulstreakStats.BlockCooldownMultiplyPerKill = Mathf.Max(soulstreakStats.BlockCooldownMultiplyPerKill + cardSoulstreakStats.BlockCooldownMultiplyPerKill, 0.5f);
            soulstreakStats.DamageMultiplyPerKill = Mathf.Max(soulstreakStats.DamageMultiplyPerKill + cardSoulstreakStats.DamageMultiplyPerKill, 0.5f);
            soulstreakStats.MovementSpeedMultiplyPerKill = Mathf.Max(soulstreakStats.MovementSpeedMultiplyPerKill + cardSoulstreakStats.MovementSpeedMultiplyPerKill, 0.5f);
            soulstreakStats.MaxMultiplyPerKill = Mathf.Max(soulstreakStats.MaxMultiplyPerKill + cardSoulstreakStats.MaxMultiplyPerKill, 0.5f);
            soulstreakStats.HealthMultiplyPerKill = Mathf.Max(soulstreakStats.HealthMultiplyPerKill + cardSoulstreakStats.HealthMultiplyPerKill, 0.5f);
        }
    }
}
