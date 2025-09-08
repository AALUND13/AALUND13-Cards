using Photon.Pun;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnboundLib;
using UnityEngine;

namespace AALUND13Cards.MonoBehaviours.CardsEffects {
    public class LifeLinkedAdder : MonoBehaviour {
        public GameObject LifeLinkPrefab;

        public void Start() {
            Player player = gameObject.GetComponentInParent<Player>();
            if(player.data.view.IsMine) {
                Player playerToLinked = ModdingUtils.Utils.PlayerStatus.GetOtherPlayers(player).GetRandom<Player>();

                PhotonNetwork.Instantiate(LifeLinkPrefab.name, Vector3.zero, Quaternion.identity, 0, new object[] { player.playerID, playerToLinked.playerID });
            }
        }
    }
}
