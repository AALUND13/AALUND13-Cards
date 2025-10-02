using AALUND13Cards.Core.Cards;
using AALUND13Cards.Core.Extensions;
using System;
using UnityEngine;

namespace AALUND13Cards.Curses.Cards.StatModifers {
    public class CursesStatsModifers : CustomStatModifers {
        [Header("Curses Stats")]
        public bool IsBind = false;
        public bool DisableDecayTime = false;

        public override void Apply(Player player) {
            var additionalData = player.data.GetCustomStatsRegistry().GetOrCreate<CursesStats>();

            if(IsBind) additionalData.IsBind = true;
            if(DisableDecayTime) additionalData.DisableDecayTime = true;
        }
    }
}
