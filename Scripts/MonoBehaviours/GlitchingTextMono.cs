using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

namespace AALUND13Card.MonoBehaviours {
    public class GlitchingTextMono : MonoBehaviour {
        public static Regex regex = new Regex(@"<glitch>(.*?)<\/glitch>", RegexOptions.Compiled);

        public bool UseGlitchTag = false;
        public bool EffectAllText = true;

        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789abcdefghijklmnopqrstuvwxyz!@#$%^&*()_+-=<>?:\"{}|,./;'[]\\'~";
        private List<(TextMeshProUGUI, string)> textMeshes = new List<(TextMeshProUGUI, string)>();
        private List<char> characters = new List<char>();

        private void Start() {
            characters = chars.ToList();
            if(!EffectAllText) return;

            TextMeshProUGUI[] allChildren = gameObject.GetComponentsInChildren<TextMeshProUGUI>();
            textMeshes.AddRange(allChildren.Select(obj => (obj, obj.text)));
        }

        public void AddTextMesh(TextMeshProUGUI textMesh) {
            textMeshes.Add((textMesh, textMesh.text));
        }

        private void Update() {
            if(textMeshes.Count == 0) return;

            foreach((TextMeshProUGUI textMesh, string originalText) in textMeshes) {
                if(UseGlitchTag) {
                    // Handle glitch effect with the <glitch> tag
                    string processedText = ProcessTextWithGlitchTag(originalText);
                    textMesh.text = processedText;
                } else {
                    // Apply glitch effect to the entire text
                    GlitchCharacters(textMesh, originalText);
                }
            }
        }

        private void GlitchCharacters(TextMeshProUGUI textMesh, string originalText) {
            textMesh.text = "<mspace=0.5em>";
            for(int i = 0; i < originalText.Length; i++) {
                textMesh.text += characters[UnityEngine.Random.Range(0, characters.Count)];
            }
        }

        private string ProcessTextWithGlitchTag(string originalText) {
            // Find all <glitch>...</glitch> matches and replace their content with random characters
            return regex.Replace(originalText, match => {
                string glitchText = match.Groups[1].Value;
                string randomGlitchedText = GenerateGlitchedText(glitchText.Length);
                return $"<mspace=0.5em>{randomGlitchedText}</mspace>";
            });
        }

        private string GenerateGlitchedText(int length) {
            char[] glitchedChars = new char[length];
            for(int i = 0; i < length; i++) {
                glitchedChars[i] = characters[UnityEngine.Random.Range(0, characters.Count)];
            }
            return new string(glitchedChars);
        }
    }
}
