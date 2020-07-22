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
        private FieldInfo m_textureProperty = null;
        private MethodInfo m_computeTextureMethod = null;
        #endregion

        #region Callbacks
        private void OnEnable() {
            FindProperties();
            m_textureProperty = typeof(GeneratedTexture).GetField("m_texture", BindingFlags.NonPublic | BindingFlags.Instance);
            m_computeTextureMethod = typeof(GeneratedTexture).GetMethod("ComputeTexture", BindingFlags.NonPublic | BindingFlags.Instance);

            if(m_textureProperty.GetValue(target) == null) {
                Debug.Log("Texture is null");
                CreateTextureAsset();
            }
        }

        public override void OnInspectorGUI() {
            //Do some stuff here with base class
        }
        #endregion

        #region Initialization
        protected virtual void FindProperties() { }
        protected virtual void Initialize() { }
        #endregion

        #region Texture
        protected void CreateTextureAsset() {
            GeneratedTexture gt = target as GeneratedTexture;
            Texture2D tex = gt.GetTexture(true);
            tex.name = gt.name + "_Texture";

            string path = AssetDatabase.GetAssetPath(gt);

            AssetDatabase.CreateAsset(tex, "");
            AssetDatabase.AddObjectToAsset(tex, gt);
            AssetDatabase.ImportAsset(path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            OnCreateTextureAsset();
        }

        protected virtual void OnCreateTextureAsset() { }
        #endregion

        protected void ComputeTexture() {
            m_computeTextureMethod.Invoke(target,null);
        }
    }
}