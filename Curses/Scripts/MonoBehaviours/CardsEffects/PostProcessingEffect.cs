using UnityEngine;

namespace AALUND13Cards.Curses.MonoBehaviours {
    [ExecuteInEditMode]
    [RequireComponent(typeof(Camera))]
    public class PostProcessingEffect : MonoBehaviour {
        public Material Material;

        private void OnRenderImage(RenderTexture src, RenderTexture dest) {
            if(Material == null) {
                Graphics.Blit(src, dest);
                return;
            }

            var temp = RenderTexture.GetTemporary(src.width, src.height);

            Graphics.Blit(src, temp, Material, 0);

            Graphics.Blit(temp, dest);

            RenderTexture.ReleaseTemporary(temp);
        }
    }
}
