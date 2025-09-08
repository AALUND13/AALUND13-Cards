using AALUND13Cards.Handlers;
using Photon.Pun;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace AALUND13Cards.MonoBehaviours.CardsEffects {
    [RequireComponent(typeof(PhotonView))]
    public class LifeLinkMono : MonoBehaviour, IPunInstantiateMagicCallback, IOnDoDamageEvent {
        public GameObject LinkTargetOne;
        public GameObject LinkTargetTwo;
        public GameObject LineRenderer;

        private Player player;
        private Player linkedPlayer;

        private float lineTurnBackAmount = 1f;
        private bool linkedDeathStarted = false;

        public void OnPhotonInstantiate(PhotonMessageInfo info) {
            object[] instantiationData = info.photonView.InstantiationData;
            int playerId = (int)instantiationData[0];
            int linkedPlayerId = (int)instantiationData[1];

            player = PlayerManager.instance.players.FirstOrDefault(p => p.playerID == playerId);
            linkedPlayer = PlayerManager.instance.players.FirstOrDefault(p => p.playerID == linkedPlayerId);

            DamageEventHandler.Instance.RegisterDamageEvent(this, linkedPlayer);
            DamageEventHandler.Instance.RegisterDamageEvent(this, player);

            linkedPlayer.data.healthHandler.reviveAction += OnRevive;
            player.data.healthHandler.reviveAction += OnRevive;
        }

        public void OnDamage(DamageInfo damage) {
            if(!linkedPlayer.data.isPlaying || linkedPlayer.data.dead || linkedPlayer.data.healthHandler.isRespawning) {
                return;
            }

            if(damage.HurtPlayer == linkedPlayer) {
                if(damage.IsLethal && linkedPlayer.data.health < damage.Damage.magnitude) {
                    StartCoroutine(LinkedDeath());
                }
            } else if(damage.HurtPlayer == player) {
                if(damage.IsLethal && player.data.health < damage.Damage.magnitude) {
                    linkedDeathStarted = true;
                    lineTurnBackAmount = 0f;
                    LineRenderer.gameObject.SetActive(false);
                }
            }
        }

        private void OnRevive() {
            if(player.data.dead || linkedPlayer.data.dead) return;

            linkedDeathStarted = false;
            lineTurnBackAmount = 1f;
            LineRenderer.gameObject.SetActive(true);
        }

        private IEnumerator LinkedDeath() {
            linkedDeathStarted = true;
            Vector2 oldPosition = LinkTargetTwo.transform.position;

            while(lineTurnBackAmount > 0) {
                lineTurnBackAmount -= Time.deltaTime * 2f;

                Vector2 newPosition = Vector2.Lerp(oldPosition, player.transform.position, 1f - lineTurnBackAmount);
                LinkTargetTwo.transform.position = newPosition;

                yield return null;
            }

            LineRenderer.gameObject.SetActive(false);

            if(player.data.view.IsMine) {
                player.data.view.RPC("RPCA_Die", RpcTarget.All, Vector2.down);
            }
            player.data.health = 0f;
        }

        private void Update() {
            if(player == null || linkedPlayer == null) return;

            LinkTargetOne.transform.position = player.transform.position;
            if(!linkedDeathStarted) {
                LinkTargetTwo.transform.position = linkedPlayer.transform.position;
            }
        }

    }
}
