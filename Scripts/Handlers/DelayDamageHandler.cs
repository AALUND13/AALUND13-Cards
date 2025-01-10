using AALUND13Card.Extensions;
using ModdingUtils.GameModes;
using UnboundLib;
using UnityEngine;

namespace AALUND13Card.Handlers {
    public class DelayDamageHandler : MonoBehaviour, IBattleStartHookHandler {
        public Player player;

        public void Awake() {
            player = GetComponent<Player>();
            InterfaceGameModeHooksManager.instance.RegisterHooks(this);
        }

        public void DelayDamage(Vector2 damage, Vector2 position, Color blinkColor, GameObject damagingWeapon, Player damagingPlayer, bool healthRemoval, bool lethal, bool ignoreBlock) {
            this.ExecuteAfterSeconds(player.data.GetAdditionalData().secondToDealDamage, () => {
                player.data.GetAdditionalData().dealDamage = true;
                player.data.healthHandler.DoDamage(damage, position, blinkColor, damagingWeapon, damagingPlayer, healthRemoval, lethal, ignoreBlock);
            });
        }

        public void OnDestroy() {
            InterfaceGameModeHooksManager.instance.RemoveHooks(this);
        }

        public void OnBattleStart() {
            StopAllCoroutines();
        }
    }
}
