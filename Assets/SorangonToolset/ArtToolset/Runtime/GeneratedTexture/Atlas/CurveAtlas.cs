//Created by Julien Delaunay (Sorangon)
//Repository link : https://github.com/Sorangon/ArtToolset

namespace SorangonToolset.ArtToolset {
    using UnityEngine;

    [CreateAssetMenu(fileName = "newAtlasTexture", menuName = "Art Toolset/Generated Texture/Curve Atlas")]
    public class CurveAtlas : GeneratedTexture {
        #region Datas
        [SerializeField] private CurveTexture[] m_Curves = { };
        [SerializeField] private int m_Resolution = 128;
        #endregion

        protected override void ComputeTexture() {
            int rows = Mathf.Min(m_Curves.Length, m_Resolution);
            for (int y = 0; y < rows; y++) {
                if (m_Curves == null) continue;

                for (int x = 0; x < m_Resolution; x++) {
                    m_Texture.SetPixel(x, y + 1, m_Curves[y].SampleTexture1D((float)x / (float)m_Resolution));
                }
            }

            m_Texture.Apply();
        }

        protected override Texture2D CreateTexture() {
            return new Texture2D(m_Resolution, m_Resolution);
        }
    }
}