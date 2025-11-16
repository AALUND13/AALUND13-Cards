using AALUND13Cards.Classes.Utils;
using AALUND13Cards.Core.Extensions;
using Sonigon;
using UnityEngine;

namespace AALUND13Cards.Classes.MonoBehaviours.ProjectilesEffects {
    public class RayHitPercentDamageOverTime : RayHitEffect {
        [Header("Sounds")]
        public SoundEvent soundEventDamageOverTime;

        [Header("Settings")]
        public float PrecntageDamage = 0.15f;
        public Color Color = new Color(0.8867924f, 0.598302f, 0f); // "Amber" color

        [Header("Damage Over Time")]
        public float Time = 5;
        public float Interval = 0.5f;

        public override HasToReturn DoHitEffect(HitInfo hit) {
            if(!hit.transform) return HasToReturn.canContinue;

            var data = hit.transform.GetComponent<CharacterData>();
            if(data == null || data.dead) return HasToReturn.canContinue;


            float effectivePercent = MathUtils.GetEffectivePercentCap(
                GetComponentInParent<ProjectileHit>().ownPlayer.GetSPS(),
                PrecntageDamage
            );
            float damage = data.maxHealth * effectivePercent;

            hit.transform.GetComponent<DamageOverTime>().TakeDamageOverTime(
                damage * transform.forward,
                transform.position,
                Time,
                Interval,
                Color,
                soundEventDamageOverTime,
                GetComponentInParent<ProjectileHit>().ownWeapon,
                GetComponentInParent<ProjectileHit>().ownPlayer
            );

            return HasToReturn.canContinue;
        }
    }
}
