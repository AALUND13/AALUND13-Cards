using AALUND13Cards.Core.Cards;
using AALUND13Cards.Core.Extensions;
using AALUND13Cards.Core.Handlers;
using AALUND13Cards.Devil.Handlers.ExtraPickHandlers;
using JARL.Bases;
using RarityLib.Utils;
using UnityEngine;

namespace AALUND13Cards.Devil.Cards.StatModifers {
    public class DevilStatsModifers : CustomStatModifers {
        [Header("Blocks")]
        public bool DisbaleBlockTime = false;
        public float FixedBlockCooldown = 0;

        [Header("Cards Stats")]
        public int DevilPicks = 0;
        public bool SetGuaranteedRarity = false;
        public CardRarity GuaranteesRarity;

        public override void Apply(Player player) {
            CharacterData data = player.data;
            var additionalData = data.GetCustomStatsRegistry().GetOrCreate<DevilCardsStats>();

            if(DisbaleBlockTime) {
                additionalData.DisbaleBlockTime = true;
            }

            if(additionalData.FixedBlockCooldown < FixedBlockCooldown) {
                additionalData.FixedBlockCooldown = FixedBlockCooldown;
            }

            if(SetGuaranteedRarity) {
                var rarity = RarityUtils.GetRarity(GuaranteesRarity.ToString());
                additionalData.GuaranteedRarity = RarityUtils.GetRarityData(rarity);
            }

            if(DevilPicks > 0) {
                ExtraCardPickHandler.AddExtraPick<DevilCardsPickHandler>(player, DevilPicks, ExtraPickPhaseTrigger.TriggerInPlayerPickEnd);
            }
        }
    }
}
