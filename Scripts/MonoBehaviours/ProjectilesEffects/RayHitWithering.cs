using AALUND13Cards.Extensions;
using AALUND13Cards.Handlers;
using AALUND13Cards.Utils;
using UnityEngine;

namespace AALUND13Cards.MonoBehaviours.ProjectilesEffects {
    public class RayHitWithering : RayHitEffect {
        public float PercentageDamagePerSecond = 0.005f;

        public override HasToReturn DoHitEffect(HitInfo hit) {
            if(!hit.transform) return HasToReturn.canContinue;

            var data = hit.transform.GetComponent<CharacterData>();
            if(data == null || data.dead) return HasToReturn.canContinue;

            var effectivePercent = MathUtils.GetEffectivePercentCap(
                GetComponentInParent<ProjectileHit>().ownPlayer.GetSPS(),
                PercentageDamagePerSecond
            );

            ConstantDamageHandler.Instance.AddConstantPrecentageDamage(
                data.player,
                GetComponentInParent<ProjectileHit>().ownPlayer,
                Color.black,
                effectivePercent
            );

            return HasToReturn.canContinue;
        }
    }
}
