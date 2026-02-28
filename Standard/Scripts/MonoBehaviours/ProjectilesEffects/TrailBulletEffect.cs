using Photon.Pun;
using UnityEngine;

namespace AALUND13Cards.Standard.MonoBehaviours.ProjectilesEffects {
    public class TrailBulletEffect : MonoBehaviour {
        public float SyncInterval = 0.5f;
        public float ActvateRange = 5f;
        public GameObject TrailPrefab;


        private PhotonView view;
        private Player player;

        private PhotonView trailView;

        private GameObject trailObject;

        private float lastSyncTime = 0;
        private bool active = false;

        private void Start() {
            view = GetComponentInParent<PhotonView>();
            player = GetComponentInParent<ProjectileHit>().ownPlayer;
            if(view.IsMine) {
                trailObject = PhotonNetwork.Instantiate(TrailPrefab.name, Vector3.zero, Quaternion.identity, 0, new object[] { transform.localScale, player.playerID });
                trailView = trailObject.GetComponent<PhotonView>();
            }
        }

        private void Update() {
            if(!view.IsMine) return;
            
            Vector3 position = transform.position;
            if(!active && Vector3.Distance(transform.position, player.transform.position) >= ActvateRange) {
                Vector3 dir = (transform.position - player.transform.position).normalized;
                position = player.transform.position + dir * ActvateRange;

                active = true;
            } 
            
            if(active && Time.time > lastSyncTime + SyncInterval) {
                trailView.RPC("RPCA_AddPosition", RpcTarget.All, position);
                lastSyncTime = Time.time;
            }
        }

        private void OnDestroy() {
            if(active) {
                trailView.RPC("RPCA_AddPosition", RpcTarget.All, transform.position);
                trailView.RPC("RPCA_SourceFinished", RpcTarget.All);
            } else if(!active) {
                PhotonNetwork.Destroy(trailObject);
            }
        }
    }
}
