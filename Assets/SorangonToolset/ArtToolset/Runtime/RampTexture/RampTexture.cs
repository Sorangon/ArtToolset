using UnityEngine;

namespace SorangonToolset.ArtToolset {

    /// <summary>
    /// An asset that generate a ramp texture from a gradient
    /// </summary>
    [CreateAssetMenu(menuName = "Art Toolset/Ramp Texture", fileName = "NewRampTexture", order = 800)]
    public class RampTexture : ScriptableObject {
        #region Settings
        [SerializeField] private Gradient _ramp = new Gradient();
        #endregion


        [ContextMenu("Generate")]
        private void ComputeTexture() {
            Debug.Log("Compute Texture");
        }
    }
}
