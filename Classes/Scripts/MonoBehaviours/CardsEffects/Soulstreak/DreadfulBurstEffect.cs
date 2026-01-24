using AALUND13Cards.Classes.Cards;
using AALUND13Cards.Classes.MonoBehaviours.CardsEffects.Soulstreak.Abilities;
using AALUND13Cards.Core.Extensions;
using AALUND13Cards.Core.Handlers;
using AALUND13Cards.Core.MonoBehaviours;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AALUND13Cards.Classes.MonoBehaviours.CardsEffects.Soulstreak {
    public class DreadfulBurstEffect : MonoBehaviour, ISoulstreakAbility {
        private float storedDamage = 0;
        private int ticks = 0;

        private Player player;
        private DamageSpawnObjects damageSpawnObjects;

        private const int MINIMUM_DRAIN_TICKS = 20;

        private void Start() {
            player = GetComponentInParent<Player>();
            damageSpawnObjects = GetComponent<DamageSpawnObjects>();

            DamageEventHandler.Instance.RegisterDamageEvent(this, player);
            player.data.GetCustomStatsRegistry().GetOrCreate<SoulStreakStats>().AddAbilityRaw(this);
            player.data.GetCustomStatsRegistry().GetOrCreate<SoulStreakStats>().GetAbility<SoulDrainAbility>()
                .soulstreakDrain.OnPlayerDamage += OnSoulDrainTick;
        }

        public void OnSoulDrainTick(Player target, float damage) {
            if(player.data.GetCustomStatsRegistry().GetOrCreate<SoulStreakStats>().DamageStorage <= ticks)
                return;

            storedDamage += damage;
            ticks++;
        }

        public void OnBlock() {
            if(MINIMUM_DRAIN_TICKS <= ticks) {
                damageSpawnObjects.SpawnDamage(Vector2.up * storedDamage);
                storedDamage = 0f;
                ticks = 0;
            }
        }

        public void OnRevive() {
            storedDamage = 0f;
            storedDamage = 0f;
        }
        
        public AbilityBarInfo GetBarInfo() {
            return new AbilityBarInfo(
                true, 
                MINIMUM_DRAIN_TICKS <= ticks, 
                ticks, 
                player.data.GetCustomStatsRegistry().GetOrCreate<SoulStreakStats>().DamageStorage
            );
        }


        public void OnUpdate() { }

        public void OnSoulsAdded(uint addedSouls) { }

        public void OnSoulsReset(uint remainingSouls) { }

        public void OnRemove() { }
    }
}
