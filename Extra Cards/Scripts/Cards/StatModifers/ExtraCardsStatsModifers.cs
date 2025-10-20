using AALUND13Cards.Core;
using AALUND13Cards.Core.Cards;
using AALUND13Cards.Core.Extensions;
using AALUND13Cards.Core.Handlers;
using AALUND13Cards.ExtraCards.Handlers;
using AALUND13Cards.ExtraCards.Handlers.ExtraPickHandlers;
using System.Collections.Generic;
using UnboundLib;
using UnityEngine;

namespace AALUND13Cards.ExtraCards.Cards.StatModifers {
    public enum ExtraPicksType {
        None,
        Normal,
        Steel
    }

    public class ExtraCardsStatsModifers : CustomStatModifers {
        [Header("Extra Picks")]
        public int ExtraPicks = 0;
        public ExtraPicksType ExtraPicksType;
        public int ExtraPicksForEnemies = 0;
        public ExtraPicksType ExtraPicksTypeForEnemies;
        public ExtraPickPhaseTrigger ExtraPickPhaseTrigger = ExtraPickPhaseTrigger.TriggerInPlayerPickEnd;


        [Header("Extra Cards")]
        public int ExtraCardPicks = 0;
        public int DuplicatesAsCorrupted = 0;

        public override void Apply(Player player) {
            CharacterData data = player.data;
            var additionalData = data.GetCustomStatsRegistry().GetOrCreate<ExtraCardsStats>();


            additionalData.ExtraCardPicksPerPickPhase += ExtraCardPicks;

            ExtraPickHandler extraPickHandler = GetExtraPickHandler(ExtraPicksType);
            if(extraPickHandler != null && ExtraPicks > 0 && player.data.view.IsMine) {
                ExtraCardPickHandler.AddExtraPick(extraPickHandler, player, ExtraPicks, ExtraPickPhaseTrigger);
            }

            ExtraPickHandler enemyExtraPickHandler = GetExtraPickHandler(ExtraPicksTypeForEnemies);
            if(extraPickHandler != null && ExtraPicksForEnemies > 0 && player.data.view.IsMine) {
                List<Player> enemies = ModdingUtils.Utils.PlayerStatus.GetEnemyPlayers(player);

                foreach(Player enemy in enemies) {
                    ExtraCardPickHandler.AddExtraPick(enemyExtraPickHandler, enemy, ExtraPicksForEnemies, ExtraPickPhaseTrigger);
                }
            }


            if(DuplicatesAsCorrupted > 0) {
                AAC_Core.Instance.ExecuteAfterFrames(1, () => {
                    additionalData.DuplicatesAsCorrupted += DuplicatesAsCorrupted;
                });
            }
        }


        public override void OnReassign(Player player) {
            CharacterData data = player.data;
            var additionalData = data.GetAdditionalData().CustomStatsRegistry.GetOrCreate<ExtraCardsStats>();

            additionalData.ExtraCardPicksPerPickPhase += ExtraCardPicks;
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
