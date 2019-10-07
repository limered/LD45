using UnityEngine;

namespace Shaders.ImageProcessing
{
    public class GrainEffect : MonoBehaviour
    {
        public Material material;

        private void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            Graphics.Blit(source, destination, material);
        }
    }
}
