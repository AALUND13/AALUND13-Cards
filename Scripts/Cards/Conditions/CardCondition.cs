using UnityEngine;

namespace AALUND13Cards.Cards.Conditions {
    public abstract class CardCondition : MonoBehaviour {
        public CardInfo CardInfo { get => GetComponent<CardInfo>(); }
        public abstract bool IsPlayerAllowedCard(Player player);
    }
}
