using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AALUND13Cards.Classes.MonoBehaviours {
    public interface ICustomDeathEffect {
        void OnDeath(DeathEffect effect, Player player);
    }

    public interface ICustomDeathRespawnEffect {
        void OnRespawn(DeathEffect effect, Player player);
    }
}
