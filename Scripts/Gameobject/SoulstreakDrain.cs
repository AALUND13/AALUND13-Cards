using AALUND13Card.Extensions;
using SoundImplementation;
using UnityEngine;

namespace AALUND13Card.MonoBehaviours {
    public class SoulstreakDrain : MonoBehaviour {
        private SoulStreakStats soulstreakStats;
        private DealDamageToPlayer dealDamageToPlayer;

        private Player player;

        public void Start() {
            player = GetComponentInParent<Player>();
            soulstreakStats = player.data.GetAdditionalData().SoulStreakStats;

            dealDamageToPlayer = GetComponent<DealDamageToPlayer>();
            dealDamageToPlayer.soundDamage.variables.audioMixerGroup = SoundVolumeManager.Instance.audioMixer.FindMatchingGroups("SFX")[0];
        }

        public void Update() {
            if(soulstreakStats != null) {
                dealDamageToPlayer.damage = player.data.weaponHandler.gun.damage * 55 * soulstreakStats.SoulDrainMultiply;
            }
        }

        public void Heal() {
            player.data.healthHandler.Heal(player.data.weaponHandler.gun.damage * 55 * soulstreakStats.SoulDrainMultiply * 0.25f);
        }
    }
}
