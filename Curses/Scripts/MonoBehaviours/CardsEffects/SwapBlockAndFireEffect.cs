using InControl;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AALUND13Cards.Curses.MonoBehaviours.CardsEffects {
    public class SwapBlockAndFireEffect : MonoBehaviour {
        private CharacterData CharacterData;
        private PlayerActions PlayerActions;

        private List<BindingSource> blockBindingSources;
        private List<BindingSource> fireBindingSources;

        private void Start() {
            CharacterData = GetComponentInParent<CharacterData>();
            PlayerActions = CharacterData.playerActions;

            blockBindingSources = PlayerActions.Block.Bindings.ToList();
            fireBindingSources = PlayerActions.Fire.Bindings.ToList();

            foreach(var blockBinding in blockBindingSources) {
                PlayerActions.Block.ClearBindings();
            }

            foreach(var fireBinding in fireBindingSources) {
                PlayerActions.Fire.ClearBindings();
            }

            foreach(var blockBinding in blockBindingSources) {
                foreach(var fireBinding in fireBindingSources) {
                    PlayerActions.Block.AddBinding(fireBinding);
                }
            }

            foreach(var fireBinding in fireBindingSources) {
                foreach(var blockBinding in blockBindingSources) {
                    PlayerActions.Fire.AddBinding(blockBinding);
                }
            }
        }

        private void OnDestroy() {
            foreach(var blockBinding in blockBindingSources) {
                if(!PlayerActions.Block.HasBinding(blockBinding)) PlayerActions.Block.AddBinding(blockBinding);
            }

            foreach(var fireBinding in fireBindingSources) {
                if(!PlayerActions.Fire.HasBinding(fireBinding)) PlayerActions.Fire.AddBinding(fireBinding);
            }
        }
    }
}
