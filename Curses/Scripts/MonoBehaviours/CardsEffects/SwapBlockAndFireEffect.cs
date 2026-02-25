using InControl;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AALUND13Cards.Curses.MonoBehaviours.CardsEffects {
    public class SwapBlockAndFireEffect : MonoBehaviour {
        private CharacterData characterData;
        private PlayerActions playerActions;

        private List<BindingSource> originalBlockBindings;
        private List<BindingSource> originalFireBindings;

        private void Start() {
            characterData = GetComponentInParent<CharacterData>();
            playerActions = characterData.playerActions;

            originalBlockBindings = playerActions.Block.Bindings.ToList();
            originalFireBindings = playerActions.Fire.Bindings.ToList();

            playerActions.Block.ClearBindings();
            playerActions.Fire.ClearBindings();

            foreach(var binding in originalFireBindings) {
                playerActions.Block.AddBinding(binding);
            }

            foreach(var binding in originalBlockBindings) {
                playerActions.Fire.AddBinding(binding);
            }
        }

        private void OnDestroy() {
            playerActions.Block.ClearBindings();
            playerActions.Fire.ClearBindings();

            foreach(var binding in originalBlockBindings) {
                playerActions.Block.AddBinding(binding);
            }

            foreach(var binding in originalFireBindings) {
                playerActions.Fire.AddBinding(binding);
            }
        }
    }
}
