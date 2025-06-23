using System.Collections.Generic;
using UnityEngine;

namespace AALUND13Cards.Handlers {
    public interface IOnDoDamageEvent {
        void OnDamage(Vector2 damage);
    }
    public interface IOnTakeDamageEvent {
        void OnTakeDamage(Vector2 damage);
    }
    public interface IOnTakeDamageOvertimeEvent {
        void OnTakeDamageOvertime(Vector2 damage);
    }



    public class DamageEventHandler {
        public enum DamageEventType {
            OnDoDamage,
            OnTakeDamage,
            OnTakeDamageOvertime
        }

        internal static Dictionary<Player, List<IOnDoDamageEvent>> OnDoDamageEvents = new Dictionary<Player, List<IOnDoDamageEvent>>();
        internal static Dictionary<Player, List<IOnTakeDamageEvent>> OnTakeDamageEvents = new Dictionary<Player, List<IOnTakeDamageEvent>>();
        internal static Dictionary<Player, List<IOnTakeDamageOvertimeEvent>> OnTakeDamageOvertimeEvents = new Dictionary<Player, List<IOnTakeDamageOvertimeEvent>>();

        public static void RegisterDamageEvent(object obj, Player player) {
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
        public static void UnregisterDamageEvent(object obj, Player player) {
            if(obj is IOnDoDamageEvent onDoDamageEvent && OnDoDamageEvents.ContainsKey(player)) {
                OnDoDamageEvents[player].Remove(onDoDamageEvent);
            }
            if(obj is IOnTakeDamageEvent onTakeDamageEvent && OnTakeDamageEvents.ContainsKey(player)) {
                OnTakeDamageEvents[player].Remove(onTakeDamageEvent);
            }
            if(obj is IOnTakeDamageOvertimeEvent onTakeDamageOvertimeEvent && OnTakeDamageOvertimeEvents.ContainsKey(player)) {
                OnTakeDamageOvertimeEvents[player].Remove(onTakeDamageOvertimeEvent);
            }
        }

        internal static void TriggerDamageEvent(DamageEventType eventType, Player player, Vector2 damage) {
            switch(eventType) {
                case DamageEventType.OnDoDamage:
                    if(OnDoDamageEvents.ContainsKey(player)) {
                        foreach(var onDoDamageEvent in OnDoDamageEvents[player]) {
                            onDoDamageEvent.OnDamage(damage);
                        }
                    }
                    break;
                case DamageEventType.OnTakeDamage:
                    if(OnTakeDamageEvents.ContainsKey(player)) {
                        foreach(var onTakeDamageEvent in OnTakeDamageEvents[player]) {
                            onTakeDamageEvent.OnTakeDamage(damage);
                        }
                    }
                    break;
                case DamageEventType.OnTakeDamageOvertime:
                    if(OnTakeDamageOvertimeEvents.ContainsKey(player)) {
                        foreach(var onTakeDamageOvertimeEvent in OnTakeDamageOvertimeEvents[player]) {
                            onTakeDamageOvertimeEvent.OnTakeDamageOvertime(damage);
                        }
                    }
                    break;
            }
        }
    }
}
