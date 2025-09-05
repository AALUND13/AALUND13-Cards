using AALUND13Cards.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AALUND13Cards.MonoBehaviours.ProjectilesEffects {
    public class RayHitWithering : RayHitEffect {
        public float PercentageDamagePerSecond = 0.005f;
        
        public override HasToReturn DoHitEffect(HitInfo hit) {
            if(!hit.transform) return HasToReturn.canContinue;

            var data = hit.transform.GetComponent<CharacterData>();
            if(data == null || data.dead) return HasToReturn.canContinue;

            ConstantDamageHandler.Instance.AddConstantPrecentageDamage(data.player, PercentageDamagePerSecond);

            return HasToReturn.canContinue;
        }
    }
}
