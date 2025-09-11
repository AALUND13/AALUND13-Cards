using AALUND13Cards.Handlers;
using JARL.Utils;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnboundLib;
using UnityEngine;

namespace AALUND13Cards.MonoBehaviours.CardsEffects {
    [RequireComponent(typeof(PhotonView))]
    public class LifeLinkMono : MonoBehaviour, IPunInstantiateMagicCallback {
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

            DeathHandler.OnPlayerDeath += OnDeath;

            linkedPlayer.data.healthHandler.reviveAction += OnRevive;
            player.data.healthHandler.reviveAction += OnRevive;
        }

        private void OnDeath(Player player, Dictionary<Player, JARL.Utils.DamageInfo> playerDamageInfos) {
            this.ExecuteAfterFrames(1, () => {
                if(player.data.healthHandler.isRespawning) return;

                if(player == linkedPlayer) {
                    if(!linkedDeathStarted) {
                        linkedDeathStarted = true;
                        StartCoroutine(LinkedDeath());
                    }
                } else if(player == this.player) {
                    linkedDeathStarted = true;
                    lineTurnBackAmount = 0f;
                    LineRenderer.gameObject.SetActive(false);
                }
            });
        }

        private void OnRevive() {
            if(player.data.dead || linkedPlayer.data.dead) return;

            linkedDeathStarted = false;
            lineTurnBackAmount = 1f;
            LineRenderer.gameObject.SetActive(true);
        }

        private IEnumerator LinkedDeath() {
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

        private void OnDestroy() {
            if(player != null) {
                DeathHandler.OnPlayerDeath -= OnDeath;
                player.data.healthHandler.reviveAction -= OnRevive;
            }
            if(linkedPlayer != null) {
                linkedPlayer.data.healthHandler.reviveAction -= OnRevive;
            }
        }

    }
}
