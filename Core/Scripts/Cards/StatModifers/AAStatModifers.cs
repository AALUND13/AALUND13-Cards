using AALUND13Cards.Core.Extensions;
using JARL.Armor;
using JARL.Bases;
using RarityLib.Utils;
using UnityEngine;

namespace AALUND13Cards.Core.Cards.StatModifers {
    public class AAStatModifers : CustomStatModifers {
        [Header("Blocks Stats")]
        public float BlockPircePercent = 0f;

        public override void Apply(Player player) {
            CharacterData data = player.data;
            var additionalData = data.GetAdditionalData();

            additionalData.BlockPircePercent = Mathf.Clamp(additionalData.BlockPircePercent + BlockPircePercent, 0f, 1f);
        }
    }
}
