using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AALUND13Cards.Standard.MonoBehaviours {
    public class MaximumScale : MonoBehaviour {
        public float MaxScale = 5;
        
        private void Start() {
            transform.localScale = new Vector3(Mathf.Min(transform.localScale.x, MaxScale), Mathf.Min(transform.localScale.y, MaxScale), Mathf.Min(transform.localScale.z, MaxScale));
        }
    }
}
