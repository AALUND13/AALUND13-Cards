using UnityEngine;

namespace AALUND13Card.Utils.RandomStatsGenerator {
    public class DamageStatGenerator : RandomStatGenerator {
        public override string StatName => "Damage";
        public DamageStatGenerator(float minValue, float maxValue) : base(minValue, maxValue) { }
        public override string Apply(float value, Gun gun, CharacterStatModifiers characterStats, Block block) {
            gun.damage += value;
            return GetStringValue(value);
        }
        public override bool IsPositive(float value) => value > 0;
    }

    public class ReloadTimeStatGenerator : RandomStatGenerator {
        public override string StatName => "Reload Time";
        public ReloadTimeStatGenerator(float minValue, float maxValue) : base(minValue, maxValue) { }
        public override string Apply(float value, Gun gun, CharacterStatModifiers characterStats, Block block) {
            gun.reloadTime += value;
            return GetStringValue(value);
        }
        public override bool IsPositive(float value) => value < 0;
    }

    public class AttackSpeedStatGenerator : RandomStatGenerator {
        public override string StatName => "Attack Speed";
        public AttackSpeedStatGenerator(float minValue, float maxValue) : base(minValue, maxValue) { }
        public override string Apply(float value, Gun gun, CharacterStatModifiers characterStats, Block block) {
            gun.attackSpeed += value;
            return GetStringValue(value);
        }
        public override bool IsPositive(float value) => value < 0;
    }

    public class MovementSpeedStatGenerator : RandomStatGenerator {
        public override string StatName => "Movement Speed";
        public MovementSpeedStatGenerator(float minValue, float maxValue) : base(minValue, maxValue) { }
        public override string Apply(float value, Gun gun, CharacterStatModifiers characterStats, Block block) {
            characterStats.movementSpeed += value;
            return GetStringValue(value);
        }
        public override bool IsPositive(float value) => value > 0;
    }

    public class HealthStatGenerator : RandomStatGenerator {
        public override string StatName => "Health";
        public HealthStatGenerator(float minValue, float maxValue) : base(minValue, maxValue) { }
        public override string Apply(float value, Gun gun, CharacterStatModifiers characterStats, Block block) {
            characterStats.health += value;
            return GetStringValue(value);
        }
        public override bool IsPositive(float value) => value > 0;
    }

    public class BlockCooldownStatGenerator : RandomStatGenerator {
        public override string StatName => "Block Cooldown";
        public BlockCooldownStatGenerator(float minValue, float maxValue) : base(minValue, maxValue) { }
        public override string Apply(float value, Gun gun, CharacterStatModifiers characterStats, Block block) {
            block.cdMultiplier += value;
            return GetStringValue(value);
        }
        public override bool IsPositive(float value) => value < 0;
    }

    public class BulletSpeedStatGenerator : RandomStatGenerator {
        public override string StatName => "Bullet Speed";
        public BulletSpeedStatGenerator(float minValue, float maxValue) : base(minValue, maxValue) { }
        public override string Apply(float value, Gun gun, CharacterStatModifiers characterStats, Block block) {
            gun.projectileSpeed += value;
            return GetStringValue(value);
        }
        public override bool IsPositive(float value) => value > 0;
    }
}
