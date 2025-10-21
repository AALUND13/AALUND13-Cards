using System;
using System.Collections.Generic;
using UnityEngine;

namespace AALUND13Cards.Core.Handlers {
    public class DeathActionHandler : MonoBehaviour {
        public static DeathActionHandler Instance;

        internal Dictionary<Player, Action> registeredReviveActions = new Dictionary<Player, Action>();
        internal Dictionary<Player, Action> registeredTrueDeathActions = new Dictionary<Player, Action>();

        public DeathActionHandler() {
            Instance = this;
        }

        public void RegisterReviveAction(Player player, Action onRevive) {
            if(registeredReviveActions.TryGetValue(player, out Action action)) {
                action += onRevive;
            } else {
                registeredReviveActions.Add(player, onRevive);
            }
        }

        public void DeregisterReviveAction(Player player, Action onRevive) {
            if(registeredReviveActions.TryGetValue(player, out Action action)) {
                action -= onRevive;
            }
        }

        public void RegisterTrueDeathAction(Player player, Action onTrueDeath) {
            if(registeredTrueDeathActions.TryGetValue(player, out Action action)) {
                action += onTrueDeath;
            } else {
                registeredTrueDeathActions.Add(player, onTrueDeath);
            }
        }

        public void DeregisterTrueDeathAction(Player player, Action onTrueDeath) {
            if(registeredTrueDeathActions.TryGetValue(player, out Action action)) {
                action -= onTrueDeath;
            }
        }
    }
}
