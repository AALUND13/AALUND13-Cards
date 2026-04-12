using AALUND13Cards.Core.Extensions;
using AALUND13Cards.Standard.Cards;
using UnityEngine;

namespace AALUND13Cards.Standard.MonoBehaviours.ProjectilesEffects {
    public class RayHitApplyFrozenTime : RayHitEffect {
        [Range(0, 10)]
        public float FrozenTime = 0.3f;

        public override HasToReturn DoHitEffect(HitInfo hit) {
            if(!hit.transform) return HasToReturn.canContinue;

            var data = hit.transform.GetComponent<CharacterData>();
            if(data == null || data.dead) return HasToReturn.canContinue;

            data.GetCustomStatsRegistry().GetOrCreate<StandardStats>().FrozenTime = FrozenTime;

            return HasToReturn.canContinue;
        }
    }
}
