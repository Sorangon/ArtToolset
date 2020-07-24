//Created by Julien Delaunay (Sorangon)
//Repository link : https://github.com/Sorangon/ArtToolset

namespace SorangonToolset.ArtToolset {
    using UnityEngine;

    /// <summary>
    /// A generated texture that only computed from a value on one dimension
    /// </summary>
    public abstract class GeneratedTexture1D : GeneratedTexture{
        #region Enums
        public enum Mapping {
            Horizontal,
            Vertical
        }
        #endregion

        #region Data
        [SerializeField] protected Mapping m_mapping = Mapping.Horizontal;
        [SerializeField] protected bool m_invert = false;
        #endregion

        #region Texture
        protected override void ComputeTexture() {
            int pixelCount = m_mapping == Mapping.Horizontal ? m_texture.width : m_texture.height;
            for(int i = 0; i < pixelCount; i++) {
                float ratio = (float)i / (float)pixelCount;

                if(m_invert) {
                    ratio = 1 - ratio;
                }

                if(m_mapping == Mapping.Horizontal) {
                    m_texture.SetPixel(i, 0, SampleTexture1D(ratio));
                } else {
                    m_texture.SetPixel(0, i, SampleTexture1D(ratio));
                }
            }

            m_texture.Apply();
        }

        protected override Texture2D CreateTexture() {
            Debug.Log("Created new texture");

            int xDim = m_mapping == Mapping.Horizontal ? 512 : 1;
            int yDim = m_mapping == Mapping.Horizontal ? 1 : 512;

            return new Texture2D(xDim, yDim, GetTextureFormat(), false) {
                wrapMode = TextureWrapMode.Clamp
            };
        }

        protected abstract TextureFormat GetTextureFormat();
        protected abstract Color SampleTexture1D(float ratio);
        #endregion
    }
}