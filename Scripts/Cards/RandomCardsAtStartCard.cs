using AALUND13Card.Extensions;

namespace AALUND13Card.Cards {
    public class RandomCardsAtStartCard : AACustomCard {
        public int RandomCardsAtStart = 1;

        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats) {
            player.data.GetAdditionalData().RandomCardsAtStart += RandomCardsAtStart;
        }
    }
}