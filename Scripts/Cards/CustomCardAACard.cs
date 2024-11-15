using JARL.Bases;
using ModdingUtils.Extensions;

namespace AALUND13Card.CustomCards {
    public class CustomCardAACard : CustomCardUnity {
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers, Block block) {
            cardInfo.GetAdditionalData().canBeReassigned = false;
        }

        public override string GetModName() {
            return AALUND13_Cards.modInitials;
        }
    }
}