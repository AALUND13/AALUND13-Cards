using AALUND13Cards.Armors;
using AALUND13Cards.Extensions;
using AALUND13Cards.Handlers;
using AALUND13Cards.Handlers.ExtraPickHandlers;
using AALUND13Cards.MonoBehaviours;
using JARL.Armor;
using UnityEngine;

namespace AALUND13Cards.Cards {
    public enum ExtraPicksType {
        None,
        Normal,
        Steel
    }

    public class AAStatModifers : MonoBehaviour {
        #region Soulstreak Stats
        [Header("Soulstreak Stats")]
        
        public float MaxHealth = 0;
        public float PlayerSize = 0;
        public float MovementSpeed = 0;

        public float AttackSpeed = 0;
        public float Damage = 0;
        public float BulletSpeed = 0;

        public float SoulArmorPercentage = 0;
        public float SoulArmorPercentageRegenRate = 0;

        public float SoulDrainDamageMultiply = 0;
        public float SoulDrainLifestealMultiply = 0;

        public AbilityType AbilityType;
        #endregion

        #region Railgun Stats
        [Header("Railgun Stats Add")]
        public float MaximumCharge = 0f;
        public float ChargeRate = 0f;
        public float FullChargeThreshold = 0f;

        [Header("Railgun Stats Multiplier")]
        public float MaximumChargeMultiplier = 1f;
        public float ChargeRateMultiplier = 1f;

        public float RailgunDamageMultiplier = 1f;
        public float RailgunBulletSpeedMultiplier = 1f;
        #endregion

        [Header("Uncategorized Stats")]
        public float SecondToDealDamage = 0;
        public float CurrentHPRegenPercentage = 0;

        [Header("Blocks Stats")]
        public int BlocksWhenRecharge = 0;
        public float BlockPircePercent = 0f;

        [Header("Armors Stats")]
        public float BattleforgedArmor = 0;
        public float ArmorPiercePercent = 0f;
        public float DamageAgainstArmorPercentage = 1f;

        [Header("Extra Picks")]
        public int ExtraPicks = 0;
        public ExtraPicksType ExtraPicksType;

        [Header("Extra Cards")]
        public int RandomCardsAtStart = 0;
        public int ExtraCardPicks = 0;

        public void Apply(Player player) {
            CharacterData data = player.data;
            var additionalData = data.GetAdditionalData();
            var jarlAdditionalData = JARL.Extensions.CharacterDataExtensions.GetAdditionalData(data);

            #region Soulstreak Stats
            additionalData.SoulStreakStats.MaxHealth += MaxHealth;
            additionalData.SoulStreakStats.PlayerSize += PlayerSize;
            additionalData.SoulStreakStats.MovementSpeed += MovementSpeed;

            additionalData.SoulStreakStats.AttackSpeed += AttackSpeed;
            additionalData.SoulStreakStats.Damage += Damage;
            additionalData.SoulStreakStats.BulletSpeed += BulletSpeed;

            additionalData.SoulStreakStats.SoulArmorPercentage += SoulArmorPercentage;
            additionalData.SoulStreakStats.SoulArmorPercentageRegenRate += SoulArmorPercentageRegenRate;

            additionalData.SoulStreakStats.SoulDrainDPSFactor += SoulDrainDamageMultiply;
            additionalData.SoulStreakStats.SoulDrainLifestealMultiply += SoulDrainLifestealMultiply;

            additionalData.SoulStreakStats.AbilityType |= AbilityType;
            #endregion

            #region Railgun Stats
            // Apply Railgun Add Stats
            additionalData.RailgunStats.MaximumCharge = Mathf.Max(additionalData.RailgunStats.MaximumCharge + MaximumCharge, 0f);
            additionalData.RailgunStats.ChargeRate = Mathf.Max(additionalData.RailgunStats.ChargeRate + ChargeRate, 0f);
            additionalData.RailgunStats.FullChargeThreshold = Mathf.Max(additionalData.RailgunStats.FullChargeThreshold + FullChargeThreshold, 0f);

            // Apply Railgun Multiplier Stats
            additionalData.RailgunStats.MaximumCharge *= MaximumChargeMultiplier;
            additionalData.RailgunStats.ChargeRate *= ChargeRateMultiplier;
            additionalData.RailgunStats.RailgunDamageMultiplier += RailgunDamageMultiplier - 1f;
            additionalData.RailgunStats.RailgunBulletSpeedMultiplier += RailgunBulletSpeedMultiplier - 1f;  
            #endregion

            // Apply Uncategorized Stats
            if(SecondToDealDamage > 0) {
                additionalData.dealDamage = false;
            }
            additionalData.secondToDealDamage += SecondToDealDamage;
            additionalData.CurrentHPRegenPercentage += CurrentHPRegenPercentage;

            // Apply Blocks Stats
            additionalData.BlocksWhenRecharge += BlocksWhenRecharge;
            additionalData.BlockPircePercent = Mathf.Clamp(additionalData.BlockPircePercent + BlockPircePercent, 0f, 1f);

            // Apply Extra Cards Stats
            additionalData.RandomCardsAtStart += RandomCardsAtStart;
            additionalData.ExtraCardPicks += ExtraCardPicks;

            // Apply Armor Stats
            if(BattleforgedArmor > 0) {
                ArmorFramework.ArmorHandlers[player].AddArmor<BattleforgedArmor>(BattleforgedArmor, 0, 0, ArmorReactivateType.Percent, 0.5f);
            }
            jarlAdditionalData.ArmorPiercePercent = Mathf.Clamp(jarlAdditionalData.ArmorPiercePercent + ArmorPiercePercent, 0f, 1f);
            additionalData.DamageAgainstArmorPercentage += DamageAgainstArmorPercentage - 1f;

            ExtraPickHandler extraPickHandler = GetExtraPickHandler(ExtraPicksType);
            if(extraPickHandler != null && ExtraPicks > 0 && player.data.view.IsMine) {
                ExtraCardPickHandler.AddExtraPick(extraPickHandler, player, ExtraPicks);
            }
        }

        public void OnReassign(Player player) {
            CharacterData data = player.data;
            var additionalData = data.GetAdditionalData();

            additionalData.ExtraCardPicks += ExtraCardPicks;
        }

        public ExtraPickHandler GetExtraPickHandler(ExtraPicksType type) {
            switch(type) {
                case ExtraPicksType.Normal:
                    return new ExtraPickHandler();
                case ExtraPicksType.Steel:
                    return new SteelPickHandler();
                default:
                    return null;
            }
        }
    }
}
