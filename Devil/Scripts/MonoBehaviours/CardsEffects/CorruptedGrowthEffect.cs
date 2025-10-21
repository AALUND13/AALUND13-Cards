using ModdingUtils.GameModes;
using ModdingUtils.MonoBehaviours;
using UnityEngine;

namespace AALUND13Cards.Devil.MonoBehaviours.CardsEffects {
    public class CorruptedGrowthEffect : MonoBehaviour, IBattleStartHookHandler, IPointEndHookHandler {
        public float HealthPerCardsMultiplier = 1.05f;
        public float DamagePerCardsMultiplier = 1.05f;

        public float ReloadTimePerCardsMultiplier = 1.05f;
        public float BlockCooldownCardsMultiplier = 0.95f;

        private PerCardEffect perCardEffect;
        private Player player;

        private void Start() {
            player = GetComponentInParent<Player>();

            InterfaceGameModeHooksManager.instance.RegisterHooks(this);
        }

        private void OnDestroy() {
            InterfaceGameModeHooksManager.instance.RemoveHooks(this);
        }

        public void OnBattleStart() {
            perCardEffect = player.gameObject.AddComponent<PerCardEffect>();
            perCardEffect.Initialize(this);
        }

        public void OnPointEnd() {
            if(perCardEffect != null) {
                Destroy(perCardEffect);
            }
        }
    }

    public class PerCardEffect : ReversibleEffect {
        public CorruptedGrowthEffect corruptedGrowthEffect;

        public void Initialize(CorruptedGrowthEffect stats) {
            corruptedGrowthEffect = stats;
        }

        public override void OnAwake() {
            SetLivesToEffect(int.MaxValue);
        }

        public override void OnStart() {
            characterDataModifier.maxHealth_mult = (corruptedGrowthEffect.HealthPerCardsMultiplier - 1) * player.data.currentCards.Count + 1;
            gunStatModifier.damage_mult = (corruptedGrowthEffect.DamagePerCardsMultiplier - 1) * player.data.currentCards.Count + 1;

            gunAmmoStatModifier.reloadTimeMultiplier_mult = (corruptedGrowthEffect.ReloadTimePerCardsMultiplier - 1) * player.data.currentCards.Count + 1;
            blockModifier.cdMultiplier_mult = (corruptedGrowthEffect.BlockCooldownCardsMultiplier - 1) * player.data.currentCards.Count + 1;
        }
    }
}
