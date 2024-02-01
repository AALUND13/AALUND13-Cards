using UnityEngine;

namespace AALUND13Card.MonoBehaviours
{
    public class SoulstreakDrain : MonoBehaviour
    {
        private SoulstreakMono soulstreak;
        private SoulStreakStats soulstreakStats;

        private DealDamageToPlayer dealDamageToPlayer;

        private Player player;

        public void Start()
        {
            soulstreak = transform.parent.GetComponentInChildren<SoulstreakMono>();
            if (soulstreak != null)
            {
                soulstreakStats = soulstreak.soulstreakStats;
                player = soulstreak.player;
            }

            dealDamageToPlayer = GetComponent<DealDamageToPlayer>();
        }

        public void Update()
        {
            if (soulstreakStats != null)
            {
                dealDamageToPlayer.damage = player.data.weaponHandler.gun.damage * 55 * soulstreakStats.SoulDrainMultiply;
            }
        }

        public void Heal()
        {
            player.data.healthHandler.Heal(player.data.weaponHandler.gun.damage * 55 * soulstreakStats.SoulDrainMultiply * 0.25f);
        }
    }
}
