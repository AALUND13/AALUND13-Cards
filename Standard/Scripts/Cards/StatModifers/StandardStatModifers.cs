using AALUND13Cards.Core.Cards;
using AALUND13Cards.Core.Extensions;
using JARL.Bases;
using RarityLib.Utils;
using UnityEngine;

namespace AALUND13Cards.Standard.Cards.StatModifers {
    public class StandardStatModifers : CustomStatModifers {
        [Header("Curses Stats")]
        public bool SetMaxRarityForCurse = false;
        public CardRarity MaxRarityForCurse;

        [Header("Blocks Stats")]
        public int BlocksWhenRecharge = 0;
        public float StunBlockTime = 0f;

        [Header("Uncategorized Stats")]
        public float SecondToDealDamage = 0;

        public override void Apply(Player player) {
            CharacterData data = player.data;
            var additionalData = data.GetCustomStatsRegistry().GetOrCreate<StandardStats>();

            // Apply Uncategorized Stats
            if(SecondToDealDamage > 0) additionalData.dealDamage = false;
            additionalData.secondToDealDamage += SecondToDealDamage;

            // Apply Curses Stats
            if(SetMaxRarityForCurse) {
                var rarity = RarityUtils.GetRarity(MaxRarityForCurse.ToString());
                additionalData.MaxRarityForCurse = RarityUtils.GetRarityData(rarity);
            }

            // Apply Blocks Stats
            additionalData.BlocksWhenRecharge += BlocksWhenRecharge;
            additionalData.StunBlockTime += StunBlockTime;
        }
    }
}
