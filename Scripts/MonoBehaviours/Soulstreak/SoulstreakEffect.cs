using AALUND13Card.Extensions;
using ModdingUtils.GameModes;
using ModdingUtils.MonoBehaviours;

namespace AALUND13Card.MonoBehaviours.Soulstreak {
    public class SoulstreakEffect : ReversibleEffect, IPickStartHookHandler, IGameStartHookHandler {
        public override void OnStart() {
            InterfaceGameModeHooksManager.instance.RegisterHooks(this);
            SetLivesToEffect(int.MaxValue);

            applyImmediately = false;
            ApplyStats();
        }

        public void ApplyStats() {
            SoulStreakStats soulStreakStats = data.GetAdditionalData().SoulStreakStats;
            uint souls = data.GetAdditionalData().Souls;

            ClearModifiers();
            characterDataModifier.maxHealth_mult = 1 + (soulStreakStats.MaxHealth - 1) * souls;
            characterStatModifiersModifier.sizeMultiplier_mult = 1 + (soulStreakStats.PlayerSize - 1) * souls;
            characterStatModifiersModifier.movementSpeed_mult = 1 + (soulStreakStats.MovementSpeed - 1) * souls;

            gunStatModifier.attackSpeed_mult = 1 + (soulStreakStats.AttackSpeed - 1) * souls;
            gunStatModifier.damage_mult = 1 + (soulStreakStats.Damage - 1) * souls;
            gunStatModifier.projectileSpeed_mult = 1 + (soulStreakStats.BulletSpeed - 1) * souls;
            ApplyModifiers();
        }

        public void OnGameStart() {
            Destroy(this);
        }

        public void OnPickStart() {
            Destroy(this);
        }

        public override void OnOnDestroy() {
            InterfaceGameModeHooksManager.instance.RemoveHooks(this);
        }
    }
}
