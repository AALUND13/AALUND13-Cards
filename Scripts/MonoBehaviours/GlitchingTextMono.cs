using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TMPro;
using UnboundLib;
using UnityEngine;

namespace AALUND13Card.MonoBehaviours {
    public class GlitchingTextMono : MonoBehaviour {
        public List<(TextMeshProUGUI, string)> textMeshes = new List<(TextMeshProUGUI, string)>();
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789abcdefghijklmnopqrstuvwxyz!@#$%^&*()_+-=<>?:\"{ }|,./;'[]\\'~";
        List<char> characters = new List<char>();

        private void Start() {
            this.ExecuteAfterFrames(3, () => {
                characters = chars.ToList();
                TextMeshProUGUI[] allChildren = gameObject.GetComponentsInChildren<TextMeshProUGUI>();
                textMeshes.AddRange(allChildren.Select(obj => (obj, obj.text)));
            });
        }

        private void Update() {
            if(textMeshes.Count == 0) return;
            foreach((TextMeshProUGUI textMesh, string originalText) in textMeshes) {
                GlitchCharacters(textMesh, originalText);
            }
        }

        private void GlitchCharacters(TextMeshProUGUI textMesh, string originalText) {
            textMesh.text = "<mspace=0.5em>";
            for(int i = 0; i < originalText.Length; i++) {
                textMesh.text += characters[UnityEngine.Random.Range(0, characters.Count)];
            }
        }
    }
}
