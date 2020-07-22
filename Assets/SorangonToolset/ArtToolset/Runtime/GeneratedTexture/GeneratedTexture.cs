//Created by Julien Delaunay (Sorangon)
//Repository link : https://github.com/Sorangon/ArtToolset

namespace SorangonToolset.ArtToolset {
    using UnityEngine;

    public abstract class GeneratedTexture : ScriptableObject {
        #region Data
        [SerializeField] protected Texture2D m_texture = null;
        #endregion

        private void Awake() {
            Debug.Log("Object initialized");
        }

        #region Texture
        /// <summary>
        /// Returns the generated texture
        /// </summary>
        /// <param name="recompute">Should the texture should be recomputed, in case a parameter had been changed</param>
        /// <returns></returns>
        public Texture2D GetTexture(bool recompute = false) {
            bool generateTexture = ReferenceEquals(m_texture, null);
            if(generateTexture) {
                m_texture = CreateTexture();
            }

            if(recompute || generateTexture) {
                ComputeTexture();
            }

            return m_texture;
        }

        /// <summary>
        /// Compute each pixel of the texture and return the result
        /// </summary>
        /// <returns></returns>
        protected abstract void ComputeTexture();

        /// <summary>
        /// Generate a the texture with target dimensions 
        /// </summary>
        /// <returns></returns>
        protected abstract Texture2D CreateTexture();
        #endregion
    }
}

