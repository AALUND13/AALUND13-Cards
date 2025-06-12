using Photon.Pun;
using UnityEngine;

namespace AALUND13Cards.MonoBehaviours {
    public class RayHitExecution : RayHitEffect {
        [Range(0, 1)]
        public float executionPercentage = 0.3f;

        public override HasToReturn DoHitEffect(HitInfo hit) {
            if(!hit.transform) return HasToReturn.canContinue;

            var data = hit.transform.GetComponent<CharacterData>();
            if(data == null || data.dead) return HasToReturn.canContinue;

            float healthPercentage = data.health / data.maxHealth;
            if(healthPercentage > executionPercentage) return HasToReturn.canContinue;

            if(data.stats.remainingRespawns > 0) {
                data.view.RPC("RPCA_Die_Phoenix", RpcTarget.All, Vector2.one * 100000f);
            } else {
                data.view.RPC("RPCA_Die", RpcTarget.All, Vector2.one * 100000f);
            }
            data.health = 0f;

            return HasToReturn.canContinue;
        }
    }
}
