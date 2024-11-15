using UnityEngine;

namespace AALUND13Card {
    public class Classes {
        public class DamageDealSecond {
            public float TimeToDealDamage;
            public Vector2 Damage;
            public Vector2 Position;
            public Color BlinkColor;
            public GameObject DamagingWeapon;
            public Player DamagingPlayer;
            public bool HealthRemoval;
            public bool Lethal;
            public bool IgnoreBlock;

            public DamageDealSecond(float timeToDealDamage, Vector2 damage, Vector2 position, Color blinkColor, GameObject damagingWeapon, Player damagingPlayer, bool healthRemoval, bool lethal, bool ignoreBlock) {
                TimeToDealDamage = timeToDealDamage;
                Damage = damage;
                Position = position;
                BlinkColor = blinkColor;
                DamagingWeapon = damagingWeapon;
                DamagingPlayer = damagingPlayer;
                HealthRemoval = healthRemoval;
                Lethal = lethal;
                IgnoreBlock = ignoreBlock;
            }
        }

        public class PlayerStats {
            public float Damage;
            public float AttackSpeed;
            public float MovementSpeed;
            public float Cooldown;
            public float MaxHealth;
            public float Health;

            public PlayerStats(CharacterData data) {
                Copy(data);
            }

            public void Copy(CharacterData data) {
                Damage = data.weaponHandler.gun.damage;
                AttackSpeed = data.weaponHandler.gun.attackSpeed;
                MovementSpeed = data.stats.movementSpeed;
                Cooldown = data.block.cooldown;
                MaxHealth = data.maxHealth;
                Health = data.health;
            }
        }
    }
}
