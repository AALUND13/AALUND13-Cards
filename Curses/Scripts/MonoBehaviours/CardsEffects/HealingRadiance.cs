using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AALUND13Cards.Curses.MonoBehaviours.CardsEffects {
    public class HealingRadiance : MonoBehaviour {
        public float healingAmount = 10f;

        private CharacterData data;
        private bool effectActive = false;

        private void Start() {
            data = GetComponentInParent<CharacterData>();
        }

        private void Update() {
            if(data.input.direction == Vector3.zero || data.input.direction == Vector3.down) {
                List<Player> otherPlayers = ModdingUtils.Utils.PlayerStatus.GetEnemyPlayers(data.player);
                foreach(Player player in otherPlayers) {
                    player.data.healthHandler.Heal(healingAmount * TimeHandler.deltaTime);
                }
            }
        }
    }
}
