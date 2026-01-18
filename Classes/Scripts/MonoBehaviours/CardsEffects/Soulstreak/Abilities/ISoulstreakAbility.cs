using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AALUND13Cards.Classes.MonoBehaviours.CardsEffects.Soulstreak.Abilities {
    public interface ISoulstreakAbility {
        void OnBlock();
        void OnReset();
        void OnUpdate();
        void OnSoulsAdded(uint addedSouls);
        void OnSoulsReset(uint removedSouls);
    }
}
