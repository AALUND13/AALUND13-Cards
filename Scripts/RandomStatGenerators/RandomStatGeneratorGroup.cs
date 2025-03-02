﻿using UnityEngine;

namespace AALUND13Card.RandomStatGenerators {
    public class DamageStatGenerator : RandomStatGenerator {
        public override string StatName => "Damage";
        public DamageStatGenerator(float minValue, float maxValue) : base(minValue, maxValue) { }
        public override string Apply(float value, GameObject cardObj, Gun gun, CharacterStatModifiers characterStats, Block block) {
            gun.damage += value;
            return GetStringValue(value * 100);
        }
        public override bool IsPositive(float value) => value > 0;
    }

    public class ReloadTimeStatGenerator : RandomStatGenerator {
        public override string StatName => "Reload Time";
        public ReloadTimeStatGenerator(float minValue, float maxValue) : base(minValue, maxValue) { }
        public override string Apply(float value, GameObject cardObj, Gun gun, CharacterStatModifiers characterStats, Block block) {
            gun.reloadTime += value;
            return GetStringValue(value * 100);
        }
        public override bool IsPositive(float value) => value < 0;
    }

    public class AttackSpeedStatGenerator : RandomStatGenerator {
        public override string StatName => "Attack Speed";
        public AttackSpeedStatGenerator(float minValue, float maxValue) : base(minValue, maxValue) { }
        public override string Apply(float value, GameObject cardObj, Gun gun, CharacterStatModifiers characterStats, Block block) {
            gun.attackSpeed += value;
            return GetStringValue(value * 100);
        }
        public override bool IsPositive(float value) => value < 0;
    }

    public class MovementSpeedStatGenerator : RandomStatGenerator {
        public override string StatName => "Movement Speed";
        public MovementSpeedStatGenerator(float minValue, float maxValue) : base(minValue, maxValue) { }
        public override string Apply(float value, GameObject cardObj, Gun gun, CharacterStatModifiers characterStats, Block block) {
            characterStats.movementSpeed += value;
            return GetStringValue(value * 100);
        }
        public override bool IsPositive(float value) => value > 0;
    }

    public class HealthStatGenerator : RandomStatGenerator {
        public override string StatName => "Health";
        public HealthStatGenerator(float minValue, float maxValue) : base(minValue, maxValue) { }
        public override string Apply(float value, GameObject cardObj, Gun gun, CharacterStatModifiers characterStats, Block block) {
            characterStats.health += value;
            return GetStringValue(value * 100);
        }
        public override bool IsPositive(float value) => value > 0;
    }

    public class BlockCooldownStatGenerator : RandomStatGenerator {
        public override string StatName => "Block Cooldown";
        public BlockCooldownStatGenerator(float minValue, float maxValue) : base(minValue, maxValue) { }
        public override string Apply(float value, GameObject cardObj, Gun gun, CharacterStatModifiers characterStats, Block block) {
            block.cdMultiplier += value;
            return GetStringValue(value * 100);
        }
        public override bool IsPositive(float value) => value < 0;
    }

    public class BulletSpeedStatGenerator : RandomStatGenerator {
        public override string StatName => "Bullet Speed";
        public BulletSpeedStatGenerator(float minValue, float maxValue) : base(minValue, maxValue) { }
        public override string Apply(float value, GameObject cardObj, Gun gun, CharacterStatModifiers characterStats, Block block) {
            gun.projectileSpeed += value;
            return GetStringValue(value * 100);
        }
        public override bool IsPositive(float value) => value > 0;
    }

    public class RegenStatGenerator : RandomStatGenerator {
        public override string StatName => "Regen";
        public RegenStatGenerator(float minValue, float maxValue) : base(minValue, maxValue) { }
        public override string Apply(float value, GameObject cardObj, Gun gun, CharacterStatModifiers characterStats, Block block) {
            characterStats.regen += value;
            return GetStringValue(value, false);
        }
        public override bool IsPositive(float value) => value > 0;
    }

    public class AmmoStatGenerator : RandomStatGenerator {
        public override string StatName => "Ammo";
        public AmmoStatGenerator(float minValue, float maxValue) : base(minValue, maxValue) { }
        public override string Apply(float value, GameObject cardObj, Gun gun, CharacterStatModifiers characterStats, Block block) {
            gun.ammo += Mathf.RoundToInt(value);
            return GetStringValue(value, false);
        }

        public override bool ShouldAddStat(float value) => Mathf.RoundToInt(value) != 0;

        public override bool IsPositive(float value) => value > 0;
    }

    public class AdditionalBlocksStatGenerator : RandomStatGenerator {
        public override string StatName => "Additiona Block";
        public AdditionalBlocksStatGenerator(float minValue, float maxValue) : base(minValue, maxValue) { }
        public override string Apply(float value, GameObject cardObj, Gun gun, CharacterStatModifiers characterStats, Block block) {
            block.additionalBlocks += Mathf.RoundToInt(value);
            return GetStringValue(value, false);
        }
        public override bool ShouldAddStat(float value) => Mathf.RoundToInt(value) != 0;
        public override bool IsPositive(float value) => value > 0;
    }

    public class ExtraLiveStatGenerator : RandomStatGenerator {
        public override string StatName => "Extra Live";
        public ExtraLiveStatGenerator(float minValue, float maxValue) : base(minValue, maxValue) { }
        public override string Apply(float value, GameObject cardObj, Gun gun, CharacterStatModifiers characterStats, Block block) {
            characterStats.respawns += Mathf.RoundToInt(value);
            return GetStringValue(value, false);
        }
        public override bool ShouldAddStat(float value) => Mathf.RoundToInt(value) != 0;
        public override bool IsPositive(float value) => value > 0;
    }
}
