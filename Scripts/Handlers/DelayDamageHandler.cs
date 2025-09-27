using AALUND13Cards.Extensions;
using ModdingUtils.GameModes;
using UnboundLib;
using UnityEngine;

namespace AALUND13Cards.Handlers {
    public class DelayDamageHandler : MonoBehaviour {
        public static DelayDamageHandler Instance { get; private set; }
        public Player player;

        public void Awake() {
            player = GetComponent<Player>();
            Instance = this;
        }

        public void DelayDamage(Vector2 damage, Vector2 position, Color blinkColor, GameObject damagingWeapon, Player damagingPlayer, bool healthRemoval, bool lethal, bool ignoreBlock) {
            this.ExecuteAfterSeconds(player.data.GetAdditionalData().secondToDealDamage, () => {
                player.data.GetAdditionalData().dealDamage = true;
                player.data.healthHandler.DoDamage(damage, position, blinkColor, damagingWeapon, damagingPlayer, healthRemoval, lethal, ignoreBlock);
            });
        }
    }
}
