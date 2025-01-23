using AALUND13Card.Armors;
using AALUND13Card.Extensions;
using AALUND13Card.Handlers;
using AALUND13Card.Handlers.ExtraPickHandlers;
using AALUND13Card.MonoBehaviours;
using JARL.Armor;
using System.Collections.Generic;
using UnityEngine;

namespace AALUND13Card.Cards {
    public enum ExtraPicksType {
        None,
        Normal,
        Steel
    }

    public class AAStatModifers : MonoBehaviour {
        [Header("Soulstreak Stats")]
        public float MaxHealth = 0;
        public float PlayerSize = 0;
        public float MovementSpeed = 0;

        public float AttackSpeed = 0;
        public float Damage = 0;
        public float BulletSpeed = 0;

        public float SoulArmorPercentage = 0;
        public float SoulArmorPercentageRegenRate = 0;

        public float SoulDrainMultiply = 0;

        public AbilityType AbilityType;

        [Header("Uncategorized Stats")]
        public float SecondToDealDamage = 0;

        [Header("Armors Stats")]
        public float BattleforgedArmor = 0;

        [Header("Extra Picks")]
        public int ExtraPicks = 0;
        public ExtraPicksType ExtraPicksType;

        [Header("Extra Cards")]
        public int RandomCardsAtStart = 0;
        public int ExtraCardPicks = 0;

        [Header("Corrupted Cards")]
        public float CorruptedCardSpawnChance = 0f;
        public float CorruptedCardSpawnChancePerPick = 0f;

        public void Apply(Player player) {
            CharacterData data = player.data;
            var additionalData = data.GetAdditionalData();

            // Apply Soulstreak Stats
            additionalData.SoulStreakStats.MaxHealth += MaxHealth;
            additionalData.SoulStreakStats.PlayerSize += PlayerSize;
            additionalData.SoulStreakStats.MovementSpeed += MovementSpeed;

            additionalData.SoulStreakStats.AttackSpeed += AttackSpeed;
            additionalData.SoulStreakStats.Damage += Damage;
            additionalData.SoulStreakStats.BulletSpeed += BulletSpeed;

            additionalData.SoulStreakStats.SoulArmorPercentage += SoulArmorPercentage;
            additionalData.SoulStreakStats.SoulArmorPercentageRegenRate += SoulArmorPercentageRegenRate;

            additionalData.SoulStreakStats.SoulDrainMultiply += SoulDrainMultiply;

            additionalData.SoulStreakStats.AbilityType |= AbilityType;

            // Apply Uncategorized Stats
            additionalData.secondToDealDamage += SecondToDealDamage;

            // Apply Extra Cards Stats
            additionalData.RandomCardsAtStart += RandomCardsAtStart;
            additionalData.ExtraCardPicks += ExtraCardPicks;

            // Corrupted Glitched Cards Stats
            additionalData.CorruptedCardSpawnChance = Mathf.Max(additionalData.CorruptedCardSpawnChance + CorruptedCardSpawnChance, 0);
            additionalData.CorruptedCardSpawnChancePerPick += CorruptedCardSpawnChancePerPick;

            if(BattleforgedArmor > 0) {
                ArmorFramework.ArmorHandlers[player].AddArmor(typeof(BattleforgedArmor), BattleforgedArmor, 0, 0, ArmorReactivateType.Percent, 0.5f);
            }

            ExtraPickHandler extraPickHandler = GetExtraPickHandler(ExtraPicksType);
            if(extraPickHandler != null && ExtraPicks > 0 && player.data.view.IsMine) {
                ExtraCardPickHandler.AddExtraPick(extraPickHandler, player, ExtraPicks);
            }
        }

        public void OnReassign(Player player) {
            CharacterData data = player.data;
            var additionalData = data.GetAdditionalData();

            additionalData.CorruptedCardSpawnChancePerPick += CorruptedCardSpawnChancePerPick;
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
