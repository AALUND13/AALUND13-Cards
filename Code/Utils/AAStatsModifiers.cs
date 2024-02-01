using System;
using UnityEngine;

namespace AALUND13Card
{
    [Serializable]
    public class AAStatsModifiers : MonoBehaviour
    {
        [Header("Soulstreak Values")]
        public float DamageMultiplyPerKill;
        public float MovementSpeedMultiplyPerKill;
        public float ATKSpeedMultiplyPerKill;
        public float BlockCooldownMultiplyPerKill;
        public float HealthMultiplyPerKill;
        public float MaxMultiplyPerKill;
        public float HealPercentagePerKill;
        //Soul Shield
        [Header("Soulstreak Soul Shield")]
        public float soulShieldPercentage;
        public float soulShieldPercentageRegen;
    }
}