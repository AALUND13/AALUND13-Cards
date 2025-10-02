using Photon.Pun;
using UnboundLib;
using UnityEngine;

namespace AALUND13Cards.Curses.MonoBehaviours.CardsEffects {
    public class LifeLinkedAdder : MonoBehaviour {
        public GameObject LifeLinkPrefab;

        private GameObject instantiatedLifeLink;

        private void Start() {
            Player player = gameObject.GetComponentInParent<Player>();
            if(player.data.view.IsMine) {
                Player playerToLinked = ModdingUtils.Utils.PlayerStatus.GetOtherPlayers(player).GetRandom<Player>();

                instantiatedLifeLink = PhotonNetwork.Instantiate(LifeLinkPrefab.name, Vector3.zero, Quaternion.identity, 0, new object[] { player.playerID, playerToLinked.playerID });
            }
        }

        private void OnDestroy() {
            if(instantiatedLifeLink != null) {
                PhotonNetwork.Destroy(instantiatedLifeLink);
            }
        }
    }
}
