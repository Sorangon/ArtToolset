//Created by Julien Delaunay (Sorangon)
//Repository link : https://github.com/Sorangon/ArtToolset

namespace SorangonToolset.ArtToolset {
    using UnityEngine;

    [CreateAssetMenu(fileName = "newAtlasTexture", menuName = "Art Toolset/Generated Texture/Curve Atlas")]
    public class CurveAtlas : GeneratedTexture {
        #region Datas
        [SerializeField] private CurveTexture[] m_Curves;
        [SerializeField] private int _resolution = 128;
        #endregion

        protected override void ComputeTexture() {
        }

        protected override Texture2D CreateTexture() {
            return null;
        }
    }
}