using AALUND13Cards.Core.Handlers;
using ModdingUtils.MonoBehaviours;
using System.Collections.Generic;
using UnityEngine;

namespace AALUND13Cards.Devil.MonoBehaviours.CardsEffects {
    public class DevilSoulEffect : MonoBehaviour {
        public float HealthOnDeathMultiplier = 0.65f;
        public float DamageOnDeathMultiplier = 0.65f;

        private List<DevilSoulDebuffEffect> statChanges = new List<DevilSoulDebuffEffect>();
        private Player player;

        private void Start() {
            player = GetComponentInParent<Player>();

            DeathActionHandler.Instance.RegisterReviveAction(player, OnDeath);
            DeathActionHandler.Instance.RegisterTrueDeathAction(player, OnTrueDeath);
        }

        private void OnDestroy() {
            DeathActionHandler.Instance.DeregisterReviveAction(player, OnDeath);
            DeathActionHandler.Instance.DeregisterTrueDeathAction(player, OnTrueDeath);
        }

        private void OnDeath() {
            var statAdded = player.gameObject.AddComponent<DevilSoulDebuffEffect>();
            statAdded.Initialize(this);
            statChanges.Add(statAdded);
        }

        private void OnTrueDeath() {
            foreach(var buff in statChanges) {
                Destroy(buff);
            }
            statChanges.Clear();
        }
    }

    internal class DevilSoulDebuffEffect : ReversibleEffect {
        public DevilSoulEffect devilSoulEffect;

        public void Initialize(DevilSoulEffect stats) {
            devilSoulEffect = stats;
        }

        public override void OnAwake() {
            SetLivesToEffect(int.MaxValue);
        }

        public override void OnStart() {
            characterDataModifier.maxHealth_mult = devilSoulEffect.HealthOnDeathMultiplier;
            gunStatModifier.damage_mult = devilSoulEffect.DamageOnDeathMultiplier;
        }
    }
}
