using AALUND13Card.MonoBehaviours;
using Photon.Realtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AALUND13Card
{
    public class Classes
    {
        public class DamageDealSecond
        {
            public float timeToDealDamage;
            public Vector2 damage;
            public Vector2 position;
            public Color blinkColor;
            public GameObject damagingWeapon;
            public Player damagingPlayer;
            public bool healthRemoval;
            public bool lethal;
            public bool ignoreBlock;

            public DamageDealSecond(float timeToDealDamage, Vector2 damage, Vector2 position, Color blinkColor, GameObject damagingWeapon, Player damagingPlayer, bool healthRemoval, bool lethal, bool ignoreBlock)
            {
                this.timeToDealDamage = timeToDealDamage;
                this.damage = damage;
                this.position = position;
                this.blinkColor = blinkColor;
                this.damagingWeapon = damagingWeapon;
                this.damagingPlayer = damagingPlayer;
                this.healthRemoval = healthRemoval;
                this.lethal = lethal;
                this.ignoreBlock = ignoreBlock;
            }
        }

        public class PlayerStats
        {
            public float damage;
            public float attackSpeed;
            public float movementSpeed;
            public float cooldown;
            public float maxHealth;
            public float health;

            public PlayerStats(CharacterData data)
            {
                Copy(data);
            }

            public void Copy(CharacterData data)
            {
                damage = data.weaponHandler.gun.damage;
                attackSpeed = data.weaponHandler.gun.attackSpeed;
                movementSpeed = data.stats.movementSpeed;
                cooldown = data.block.cooldown;
                maxHealth = data.maxHealth;
                health = data.health;
            }
        }
    }
}
