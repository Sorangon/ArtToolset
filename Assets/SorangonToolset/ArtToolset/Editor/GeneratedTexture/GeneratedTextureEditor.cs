//Julien Delaunay(Sorangon)
//Repository link : https://github.com/Sorangon/ArtToolset

namespace SorangonToolset.ArtToolset.Editors {
    using UnityEngine;
    using UnityEditor;
    using System.Reflection;

    /// <summary>
    /// The base class for Generated textures custom editors 
    /// </summary>
    public abstract class GeneratedTextureEditor : Editor {
        #region Currents
        private MethodInfo m_computeTextureMethod = null;
        #endregion

        #region Callbacks
        private void OnEnable() {
            FindProperties();
            m_computeTextureMethod = typeof(GeneratedTexture).GetMethod("ComputeTexture", BindingFlags.NonPublic | BindingFlags.Instance);
        }

        public override void OnInspectorGUI() {
            //Do some stuff here with base class
        }
        #endregion

        #region Initialization
        protected virtual void FindProperties() { }
        #endregion

        #region Texture
        /// <summary>
        /// Create or just rename a target generated texture
        /// </summary>
        /// <param name="gt"></param>
        internal static void SetupTextureAsset(GeneratedTexture gt) {
            string path = AssetDatabase.GetAssetPath(gt);

            //Checks if the texture subobject already exists
            Texture2D textureObject = null;
            var assets = AssetDatabase.LoadAllAssetsAtPath(path);
            for(int i = 0; i < assets.Length; i++) {
                if(assets[i] is Texture2D) {
                    textureObject = assets[i] as Texture2D;
                    break;
                }
            }

            if(textureObject == null) {
                //Create a new one
                textureObject = gt.GetTexture(true);
                textureObject.name = gt.name + "_Texture";
                AssetDatabase.AddObjectToAsset(textureObject, gt);
            }

            textureObject.name = gt.name + "_Texture";
            AssetDatabase.Refresh();
            AssetDatabase.SaveAssets();
        }
        protected void ComputeTexture() {
            m_computeTextureMethod.Invoke(target,null);
        }
        #endregion

    }
}