//Created by Julien Delaunay (Sorangon)
//Repository link : https://github.com/Sorangon/ArtToolset

using UnityEngine;

namespace SorangonToolset.ArtToolset {

    /// <summary>
    /// An asset that generate a ramp texture from a gradient
    /// </summary>
    [CreateAssetMenu(menuName = "Art Toolset/Ramp Texture", fileName = "NewRampTexture", order = 800)]
    public class RampTexture : ScriptableObject {
        #region Settings
        [SerializeField] public Gradient _ramp = new Gradient();
        [SerializeField] private Texture2D _generatedTexture = null;
        #endregion



        
        private void ComputeTexture() {
            Debug.Log("Compute Texture");
        }
    }
}
