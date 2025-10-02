using AALUND13Cards.Core.Utils;
using AALUND13Cards.ExtraCards.Cards.StatModifers;
using AALUND13Cards.ExtraCards.Handlers;
using System;
using UnityEngine;

namespace AALUND13Cards.ExtraCards.Cards {
    public class ExtraCardsStats : ICustomStats {
        public int DuplicatesAsCorrupted = 0;
        public int ExtraCardPicksPerPickPhase = 0;

        public void ResetStats() {
            DuplicatesAsCorrupted = 0;
            ExtraCardPicksPerPickPhase = 0;
        }
    }
}
