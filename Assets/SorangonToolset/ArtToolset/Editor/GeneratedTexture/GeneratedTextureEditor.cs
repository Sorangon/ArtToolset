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
        private MethodInfo m_updateTextureMethod = null;
        private bool m_computeTextureFlag = false;
        private bool m_recreateTextureFlag = false;
        #endregion

        #region Callbacks
        private void OnEnable() {
            FindProperties();
            m_computeTextureMethod = typeof(GeneratedTexture).GetMethod("ComputeTexture", BindingFlags.NonPublic | BindingFlags.Instance);
            m_updateTextureMethod = typeof(GeneratedTexture).GetMethod("UpdateTexture", BindingFlags.NonPublic | BindingFlags.Instance);
            Undo.undoRedoPerformed += OnPerformUndoRedo;
        }

        private void OnDisable() {
            Undo.undoRedoPerformed -= OnPerformUndoRedo;
        }

        public override sealed void OnInspectorGUI() {
            serializedObject.Update();

            DrawInspector();

            //TODO : Fix reference loss
            //if(m_recreateTextureFlag) {
            //    serializedObject.ApplyModifiedPropertiesWithoutUndo();
            //    m_updateTextureMethod.Invoke(target, null);
            //    SetupTextureAsset(target as GeneratedTexture);
            //    m_recreateTextureFlag = false;
            //}

            if(m_computeTextureFlag) {
                serializedObject.ApplyModifiedPropertiesWithoutUndo();
                m_computeTextureMethod.Invoke(target, null);
                m_computeTextureFlag = false;
            }
            serializedObject.ApplyModifiedProperties();
        }

        protected virtual void DrawInspector() {}
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
                AssetDatabase.AddObjectToAsset(textureObject, gt);
            }

            textureObject.name = gt.name + "_Texture";
            AssetDatabase.Refresh();
            AssetDatabase.SaveAssets();
        }

        protected void SetComputeFlagUp() {
            m_computeTextureFlag = true;
        }

        protected void SetRecreateTextureFlagUp() {
            m_recreateTextureFlag = true;
        }
        #endregion

        #region Undo
        private void OnPerformUndoRedo() {
            SetComputeFlagUp();
            //SetRecreateTextureFlagUp();
        }
        #endregion
    }
}