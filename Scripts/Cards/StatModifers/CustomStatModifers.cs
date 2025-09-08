using UnityEngine;

namespace AALUND13Cards.Cards {
    public abstract class CustomStatModifers : MonoBehaviour {
        public abstract void Apply(Player player);
        public virtual void OnReassign(Player player) { }
    }
}
