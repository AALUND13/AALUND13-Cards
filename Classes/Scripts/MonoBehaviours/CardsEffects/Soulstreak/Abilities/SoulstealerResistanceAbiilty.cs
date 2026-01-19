using AALUND13Cards.Classes.Cards;
using AALUND13Cards.Classes.MonoBehaviours.CardsEffects.Soulstreak.Abilities;
using AALUND13Cards.Core.Extensions;
using JARL.Armor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AALUND13Cards.Classes.MonoBehaviours.CardsEffects.Soulstreak.Abilities {
    public class SoulstealerResistanceAbiilty : SoulstreakAbility<SoulstealerResistanceAbiilty> {
        private Player player;
        private float addedDamageResistance = 0f;

        public SoulstealerResistanceAbiilty(Player player) {
            this.player = player;
        }

        public override void OnSoulsAdded(uint addedSouls) {
            float currentResistance = player.data.GetCustomStatsRegistry().GetOrCreate<ClassesStats>().DamageResistance;

            float resistanceToAdd = SoulstreakStats.DamageResistancePerKill * addedSouls;

            float newResistance = Mathf.Min(currentResistance + resistanceToAdd, 0.75f);
            float actualAdded = newResistance - currentResistance;

            addedDamageResistance += actualAdded;
            player.data.GetCustomStatsRegistry().GetOrCreate<ClassesStats>().DamageResistance = newResistance;
        }

        public override void OnSoulsReset(uint removedSouls) {
            player.data.GetCustomStatsRegistry().GetOrCreate<ClassesStats>().DamageResistance -= addedDamageResistance;
            addedDamageResistance = 0;
        }
    }
}
