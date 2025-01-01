using JARL.Bases;

namespace AALUND13Card.Cards {
    public class AACustomCard : CustomCardUnity {
        public virtual void OnRegister(CardInfo cardInfo) { }

        public override string GetModName() {
            return AALUND13_Cards.modInitials;
        }
    }
}