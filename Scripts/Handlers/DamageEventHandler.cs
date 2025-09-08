using System.Collections.Generic;
using UnityEngine;

namespace AALUND13Cards.Handlers {
    public struct DamageInfo {
        public Vector2 Damage;
        public bool IsLethal;

        public Player DamagingPlayer;
        public Player HurtPlayer;

        public DamageInfo(Vector2 damage, bool isLethal, Player damagingPlayer, Player hurtPlayer) {
            Damage = damage;
            IsLethal = isLethal;
            DamagingPlayer = damagingPlayer;
            HurtPlayer = hurtPlayer;
        }
    }

    public interface IOnDoDamageEvent {
        void OnDamage(DamageInfo damage);
    }
    public interface IOnTakeDamageEvent {
        void OnTakeDamage(DamageInfo damage);
    }
    public interface IOnTakeDamageOvertimeEvent {
        void OnTakeDamageOvertime(DamageInfo damage);
    }



    public class DamageEventHandler : MonoBehaviour {
        public enum DamageEventType {
            OnDoDamage,
            OnTakeDamage,
            OnTakeDamageOvertime
        }

        internal Dictionary<Player, List<IOnDoDamageEvent>> OnDoDamageEvents = new Dictionary<Player, List<IOnDoDamageEvent>>();
        internal Dictionary<Player, List<IOnTakeDamageEvent>> OnTakeDamageEvents = new Dictionary<Player, List<IOnTakeDamageEvent>>();
        internal Dictionary<Player, List<IOnTakeDamageOvertimeEvent>> OnTakeDamageOvertimeEvents = new Dictionary<Player, List<IOnTakeDamageOvertimeEvent>>();

        internal Dictionary<Player, List<IOnDoDamageEvent>> OnDoDamageEventsOtherPlayer = new Dictionary<Player, List<IOnDoDamageEvent>>();
        internal Dictionary<Player, List<IOnTakeDamageEvent>> OnTakeDamageEventsOtherPlayer = new Dictionary<Player, List<IOnTakeDamageEvent>>();
        internal Dictionary<Player, List<IOnTakeDamageOvertimeEvent>> OnTakeDamageOvertimeEventsOtherPlayer = new Dictionary<Player, List<IOnTakeDamageOvertimeEvent>>();

        public static DamageEventHandler Instance;

        public void RegisterDamageEvent(object obj, Player player) {
            if(obj is IOnDoDamageEvent onDoDamageEvent) {
                if(!OnDoDamageEvents.ContainsKey(player)) {
                    OnDoDamageEvents[player] = new List<IOnDoDamageEvent>();
                }
                OnDoDamageEvents[player].Add(onDoDamageEvent);
            }
            if(obj is IOnTakeDamageEvent onTakeDamageEvent) {
                if(!OnTakeDamageEvents.ContainsKey(player)) {
                    OnTakeDamageEvents[player] = new List<IOnTakeDamageEvent>();
                }
                OnTakeDamageEvents[player].Add(onTakeDamageEvent);
            }
            if(obj is IOnTakeDamageOvertimeEvent onTakeDamageOvertimeEvent) {
                if(!OnTakeDamageOvertimeEvents.ContainsKey(player)) {
                    OnTakeDamageOvertimeEvents[player] = new List<IOnTakeDamageOvertimeEvent>();
                }
                OnTakeDamageOvertimeEvents[player].Add(onTakeDamageOvertimeEvent);
            }
        }

        public void RegisterDamageEventForOtherPlayers(object obj, Player player) {
            if(obj is IOnDoDamageEvent onDoDamageEvent) {
                if(!OnDoDamageEventsOtherPlayer.ContainsKey(player)) {
                    OnDoDamageEventsOtherPlayer[player] = new List<IOnDoDamageEvent>();
                }
                OnDoDamageEventsOtherPlayer[player].Add(onDoDamageEvent);
            }
            if(obj is IOnTakeDamageEvent onTakeDamageEvent) {
                if(!OnTakeDamageEventsOtherPlayer.ContainsKey(player)) {
                    OnTakeDamageEventsOtherPlayer[player] = new List<IOnTakeDamageEvent>();
                }
                OnTakeDamageEventsOtherPlayer[player].Add(onTakeDamageEvent);
            }
            if(obj is IOnTakeDamageOvertimeEvent onTakeDamageOvertimeEvent) {
                if(!OnTakeDamageOvertimeEventsOtherPlayer.ContainsKey(player)) {
                    OnTakeDamageOvertimeEventsOtherPlayer[player] = new List<IOnTakeDamageOvertimeEvent>();
                }
                OnTakeDamageOvertimeEventsOtherPlayer[player].Add(onTakeDamageOvertimeEvent);
            }
        }

        public void UnregisterDamageEvent(object obj, Player player) {
            if(obj is IOnDoDamageEvent onDoDamageEvent && OnDoDamageEvents.ContainsKey(player)) {
                OnDoDamageEvents[player].Remove(onDoDamageEvent);
            }
            if(obj is IOnTakeDamageEvent onTakeDamageEvent && OnTakeDamageEvents.ContainsKey(player)) {
                OnTakeDamageEvents[player].Remove(onTakeDamageEvent);
            }
            if(obj is IOnTakeDamageOvertimeEvent onTakeDamageOvertimeEvent && OnTakeDamageOvertimeEvents.ContainsKey(player)) {
                OnTakeDamageOvertimeEvents[player].Remove(onTakeDamageOvertimeEvent);
            }

            if(obj is IOnDoDamageEvent onDoDamageEventOther && OnDoDamageEventsOtherPlayer.ContainsKey(player)) {
                OnDoDamageEventsOtherPlayer[player].Remove(onDoDamageEventOther);
            }
            if(obj is IOnTakeDamageEvent onTakeDamageEventOther && OnTakeDamageEventsOtherPlayer.ContainsKey(player)) {
                OnTakeDamageEventsOtherPlayer[player].Remove(onTakeDamageEventOther);
            }
            if(obj is IOnTakeDamageOvertimeEvent onTakeDamageOvertimeEventOther && OnTakeDamageOvertimeEventsOtherPlayer.ContainsKey(player)) {
                OnTakeDamageOvertimeEventsOtherPlayer[player].Remove(onTakeDamageOvertimeEventOther);
            }
        }

        internal static void TriggerDamageEvent(DamageEventType eventType, Player hurtPlayer, Player damagingPlayer, Vector2 damage, bool isLethal) {
            DamageInfo damageInfo = new DamageInfo(damage, isLethal, damagingPlayer, hurtPlayer);

            switch(eventType) {
                case DamageEventType.OnDoDamage:
                    if(Instance.OnDoDamageEvents.ContainsKey(hurtPlayer)) {
                        foreach(var onDoDamageEvent in Instance.OnDoDamageEvents[hurtPlayer]) {
                            onDoDamageEvent.OnDamage(damageInfo);
                        }
                    }
                    break;
                case DamageEventType.OnTakeDamage:
                    if(Instance.OnTakeDamageEvents.ContainsKey(hurtPlayer)) {
                        foreach(var onTakeDamageEvent in Instance.OnTakeDamageEvents[hurtPlayer]) {
                            onTakeDamageEvent.OnTakeDamage(damageInfo);
                        }
                    }
                    break;
                case DamageEventType.OnTakeDamageOvertime:
                    if(Instance.OnTakeDamageOvertimeEvents.ContainsKey(hurtPlayer)) {
                        foreach(var onTakeDamageOvertimeEvent in Instance.OnTakeDamageOvertimeEvents[hurtPlayer]) {
                            onTakeDamageOvertimeEvent.OnTakeDamageOvertime(damageInfo);
                        }
                    }
                    break;
            }

            foreach(var kvp in Instance.OnDoDamageEventsOtherPlayer) {
                if(kvp.Key != hurtPlayer) {
                    foreach(var onDoDamageEvent in kvp.Value) {
                        if(eventType == DamageEventType.OnDoDamage) {
                            onDoDamageEvent.OnDamage(damageInfo);
                        }
                    }
                }
            }
            foreach(var kvp in Instance.OnTakeDamageEventsOtherPlayer) {
                if(kvp.Key != hurtPlayer) {
                    foreach(var onTakeDamageEvent in kvp.Value) {
                        if(eventType == DamageEventType.OnTakeDamage) {
                            onTakeDamageEvent.OnTakeDamage(damageInfo);
                        }
                    }
                }
            }
            foreach(var kvp in Instance.OnTakeDamageOvertimeEventsOtherPlayer) {
                if(kvp.Key != hurtPlayer) {
                    foreach(var onTakeDamageOvertimeEvent in kvp.Value) {
                        if(eventType == DamageEventType.OnTakeDamageOvertime) {
                            onTakeDamageOvertimeEvent.OnTakeDamageOvertime(damageInfo);
                        }
                    }
                }
            }
        }

        private void Awake() {
            if(Instance != null) {
                Destroy(this);
                return;
            }
            Instance = this;
        }
    }
}
