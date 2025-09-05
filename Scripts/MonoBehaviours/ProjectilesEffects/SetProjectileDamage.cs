using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AALUND13Cards.MonoBehaviours.ProjectilesEffects {
    public class SetProjectileDamage : MonoBehaviour {
        public float damage;

        private void Start() {
            GetComponentInParent<ProjectileHit>().damage = damage;
        }
    }
}
