using ModdingUtils.GameModes;
using System.Collections.Generic;
using UnityEngine;

namespace AALUND13Cards {
    public class PickCardTracker : MonoBehaviour, IPickStartHookHandler {
        public static PickCardTracker instance;

        private readonly List<CardInfo> cardPickedInPickPhase = new List<CardInfo>();
        public IReadOnlyList<CardInfo> CardPickedInPickPhase => cardPickedInPickPhase.AsReadOnly();

        public bool AlreadyPickedInPickPhase(CardInfo card) {
            if(card == null) return false;
            return cardPickedInPickPhase.Contains(card);
        }

        public void OnPickStart() {
            cardPickedInPickPhase.Clear();
        }

        internal void AddCardPickedInPickPhase(CardInfo card) {
            if(card == null) return;

            if(card.sourceCard != null) card = card.sourceCard;
            if(!cardPickedInPickPhase.Contains(card)) {
                cardPickedInPickPhase.Add(card);
            }
        }

        private void Awake() {
            if(instance != null) {
                UnityEngine.Debug.LogWarning("PickCardTracker instance already exists! Destroying the new one.");
                Destroy(gameObject);
                return;
            }
            instance = this;

            InterfaceGameModeHooksManager.instance.RegisterHooks(this);
        }
    }
}
