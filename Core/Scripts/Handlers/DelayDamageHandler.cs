using AALUND13Cards.Core.Extensions;
using ModdingUtils.GameModes;
using System;
using UnboundLib;
using UnityEngine;

namespace AALUND13Cards.Core.Handlers {
    public struct DelayDamageInfo {
        public Vector2 Damage;
        public Vector2 Position;
        public Color BlinkColor;

        public GameObject DamagingWeapon;
        public Player DamagingPlayer;

        public bool HealthRemoval;
        public bool Lethal;
        public bool IngnoreBlock;

        public DelayDamageInfo(Vector2 damage, Vector2 position, Color blinkColor, GameObject damagingWeapon = null, Player damagingPlayer = null, bool healthRemoval = false, bool lethal = true, bool ignoreBlock = false) {
            Damage = damage;
            Position = position;
            BlinkColor = blinkColor;
            DamagingWeapon = damagingWeapon;
            DamagingPlayer = damagingPlayer;
            HealthRemoval = healthRemoval;
            Lethal = lethal;
            IngnoreBlock = ignoreBlock;
        }
    }

    public class DelayDamageHandler : MonoBehaviour {
        public static DelayDamageHandler Instance { get; private set; }
        public Player player;

        public void Awake() {
            player = GetComponent<Player>();
            Instance = this;
        }

        public void DelayDamage(DelayDamageInfo delayDamageInfo, float delay, Action beforeDamageCall = null, Action afterDamageCall = null) {
            this.ExecuteAfterSeconds(delay, () => {
                beforeDamageCall?.Invoke();
                player.data.healthHandler.DoDamage(delayDamageInfo.Damage, delayDamageInfo.Position, delayDamageInfo.BlinkColor, delayDamageInfo.DamagingWeapon, delayDamageInfo.DamagingPlayer, delayDamageInfo.HealthRemoval, delayDamageInfo.Lethal, delayDamageInfo.IngnoreBlock);
                afterDamageCall?.Invoke();
            });
        }
    }
}
