using AALUND13Cards.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AALUND13Cards.MonoBehaviours.CardsEffects {
    public class ReverseLifeSteelEffect : MonoBehaviour, IOnDoDamageEvent {
        public float ReverseLifeSteelPercentage = 0.2f;

        private Player player;

        public void OnDamage(DamageInfo damage) {
            damage.DamagingPlayer.data.healthHandler.Heal(damage.Damage.magnitude * ReverseLifeSteelPercentage);
        }

        private void Start() {
            player = GetComponentInParent<Player>();
        }
    }
}
