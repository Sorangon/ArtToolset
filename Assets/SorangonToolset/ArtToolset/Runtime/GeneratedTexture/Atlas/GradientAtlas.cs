//Created by Julien Delaunay (Sorangon)
//Repository link : https://github.com/Sorangon/ArtToolset

namespace SorangonToolset.ArtToolset {
    using UnityEngine;

    [CreateAssetMenu(fileName = "newAtlasTexture", menuName = "Art Toolset/Generated Texture/Curve Atlas")]
    public class GradientAtlas : GeneratedTexture {
        #region Datas
        [SerializeField, GradientUsage(true)] private Gradient[] m_Gradients = { };
        [SerializeField] private int m_Resolution = 128;
        #endregion

        protected override void ComputeTexture() {
            int rows = Mathf.Min(m_Gradients.Length, m_Resolution);
#if UNITY_EDITOR
            if (!Application.isPlaying) {
                m_Texture.SetPixels(new Color[m_Resolution * m_Resolution]);
            }
#endif
            for (int y = 0; y < rows; y++) {
                for (int x = 0; x < m_Resolution; x++) {
                    m_Texture.SetPixel(x, m_Resolution - y - 1, m_Gradients[y].Evaluate((float)x / (float)m_Resolution));
                }
            }

            m_Texture.Apply();
        }

        protected override Texture2D CreateTexture() {
            Texture2D tex = new Texture2D(m_Resolution, m_Resolution);
            tex.wrapMode = TextureWrapMode.Clamp;
            tex.filterMode = FilterMode.Bilinear;
            return tex;
        }
    }
}