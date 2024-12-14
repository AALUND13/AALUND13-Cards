using AALUND13Card.Extensions;

namespace AALUND13Card.Cards {
    public class DelayDamageCard : CustomCardAACard {
        public float SecondToDealDamage = 4;

        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats) {
            player.data.GetAdditionalData().dealDamage = false;
            player.data.GetAdditionalData().secondToDealDamage += SecondToDealDamage;
        }
    }
}