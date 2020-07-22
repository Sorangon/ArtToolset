//Created by Julien Delaunay (Sorangon)
//Repository link : https://github.com/Sorangon/ArtToolset

using UnityEngine;

namespace SorangonToolset.ArtToolset {

    /// <summary>
    /// An asset that generate a ramp texture from a gradient
    /// </summary>
    [CreateAssetMenu(menuName = "Art Toolset/Ramp Texture", fileName = "NewRampTexture", order = 800)]
    public class RampTexture : GeneratedTexture {
        #region Settings
        [SerializeField, GradientUsage(true)] public Gradient ramp = new Gradient();
        #endregion

        #region Texture
        protected override void ComputeTexture() {
            for(int i = 0; i < m_texture.width; i++) {
                float ratio = (float)i / (float)m_texture.width;
                m_texture.SetPixel(i, 0, ramp.Evaluate(ratio));
            }

            m_texture.Apply();
        }

        protected override Texture2D CreateTexture() {
            return new Texture2D(512, 1, TextureFormat.RGBAFloat, false) {
                wrapMode = TextureWrapMode.Clamp
            };
        }
        #endregion
    }
}
