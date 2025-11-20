using AALUND13Cards.Core;
using AALUND13Cards.Core.Cards.Effects;
using UnboundLib;
using UnityEngine;

namespace AALUND13Cards.Devil.Cards.Effects {
    public class CardsDrawsMultiplierAddedEffect : OnAddedEffect {
        [Range(0f, 2f)]
        public float CardsDrawsMultiplier = 1;

        public override void OnAdded(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats) {
            AAC_Core.Instance.ExecuteAfterSeconds(0.5f, () => {
                int drawToSet = Mathf.RoundToInt(DrawNCards.DrawNCards.GetPickerDraws(player.playerID) * CardsDrawsMultiplier);
                DrawNCards.DrawNCards.SetPickerDraws(player.playerID, drawToSet);
            });
        }
    }
}
