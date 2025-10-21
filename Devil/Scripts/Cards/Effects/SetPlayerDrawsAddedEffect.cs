using AALUND13Cards.Core;
using AALUND13Cards.Core.Cards.Effects;
using UnboundLib;
using UnityEngine;

namespace AALUND13Cards.Devil.Cards.Effects {
    public class SetPlayerDrawsAddedEffect : OnAddedEffect {
        [Range(0, 30)]
        public int NumberOfDrawToSet = 5;

        public override void OnAdded(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats) {
            AAC_Core.Instance.ExecuteAfterSeconds(0.5f, () => {
                DrawNCards.DrawNCards.SetPickerDraws(player.playerID, NumberOfDrawToSet);
            });
        }
    }
}
