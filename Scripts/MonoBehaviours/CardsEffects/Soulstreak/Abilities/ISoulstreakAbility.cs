using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AALUND13Cards.MonoBehaviours.CardsEffects.Soulstreak.Abilities {
    public interface ISoulstreakAbility {
        void OnBlock(SoulstreakMono soulstreak);
        void OnReset(SoulstreakMono soulstreak);
        void OnUpdate(SoulstreakMono soulstreak);
    }
}
