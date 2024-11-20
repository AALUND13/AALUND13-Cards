using UnityEngine;

namespace AALUND13Card {
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
}
