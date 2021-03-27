//Julien Delaunay(Sorangon)
//Repository link : https://github.com/Sorangon/ArtToolset

namespace SorangonToolset.ArtToolset.Editors {
    using UnityEngine;
    using UnityEditor;
    using System.Reflection;
    using System.IO;

    /// <summary>
    /// The base class for Generated textures custom editors 
    /// </summary>
    public abstract class GeneratedTextureEditor : Editor {
        #region Enums
        public enum BakeFormat {
            EXR,
            JPEG,
            PNG
        }
        #endregion

        #region Currents
        private MethodInfo m_ComputeTextureMethod = null;
        private MethodInfo m_UpdateTextureMethod = null;
        private FieldInfo m_GeneratedTextureField = null;
        private SerializedProperty m_RecalculateOnLoad = null;
        private bool m_ComputeTextureFlag = false;
        //private bool m_recreateTextureFlag = false;
        #endregion

        #region Callbacks
        private void OnEnable() {
            FindProperties();
            m_ComputeTextureMethod = typeof(GeneratedTexture).GetMethod("ComputeTexture", BindingFlags.NonPublic | BindingFlags.Instance);
            m_UpdateTextureMethod = typeof(GeneratedTexture).GetMethod("UpdateTexture", BindingFlags.NonPublic | BindingFlags.Instance);
            m_GeneratedTextureField = typeof(GeneratedTexture).GetField("m_Texture", BindingFlags.NonPublic | BindingFlags.Instance);
            m_RecalculateOnLoad = serializedObject.FindProperty("m_RecalculateOnLoad");
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

            if(m_ComputeTextureFlag) {
                serializedObject.ApplyModifiedPropertiesWithoutUndo();
                m_ComputeTextureMethod.Invoke(target, null);
                m_ComputeTextureFlag = false;
            }

            //Separator
            GUILayout.Space(20f);
            GUILayout.Box(new GUIContent(), GUILayout.Height(3f), GUILayout.Width(EditorGUIUtility.currentViewWidth - 25f));
            GUILayout.Space(6f);

            DrawBakeButtons();
            GUILayout.Space(6f);
            EditorGUILayout.PropertyField(m_RecalculateOnLoad);

            serializedObject.ApplyModifiedProperties();
        }

        protected virtual void DrawInspector() { }
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
            m_ComputeTextureFlag = true;
        }

        //protected void SetRecreateTextureFlagUp() {
        //    m_recreateTextureFlag = true;
        //}
        #endregion

        #region Panels
        private void DrawBakeButtons() {
            if(GUILayout.Button("Bake PNG")) {
                BakeTexture(BakeFormat.PNG);
            }

            if(GUILayout.Button("Bake JPEG")) {
                BakeTexture(BakeFormat.JPEG);
            }

            if(GUILayout.Button("Bake EXR")) {
                BakeTexture(BakeFormat.EXR);
            }
        }
        #endregion

        #region Bake
        private void BakeTexture(BakeFormat format) {
            //Get save path
            string extension = format.ToString();
            string projectPath = EditorUtility.SaveFilePanelInProject("Bake texture" + extension, target.name, extension.ToLower(), "Bake the texture in " + extension + " format");
            if(projectPath.Length <= 0) return;
            string path = projectPath.Remove(0, 6); //Remove "Assets" characters
            path = Application.dataPath + path; //Final path


            //Encode texture
            byte[] encodedTexture = { };
            Texture2D srcTex = ((GeneratedTexture)target).GetTexture(true);
            switch(format) {
                case BakeFormat.PNG:
                    encodedTexture = srcTex.EncodeToPNG();
                    break;
                case BakeFormat.JPEG:
                    encodedTexture = srcTex.EncodeToJPG();
                    break;
                case BakeFormat.EXR:
                    encodedTexture = srcTex.EncodeToEXR();
                    break;
            }

            //Save texture
            File.WriteAllBytes(path, encodedTexture);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            //Select new texture
            Texture newTex = AssetDatabase.LoadAssetAtPath(projectPath, typeof(Texture2D)) as Texture2D;
            EditorGUIUtility.PingObject(newTex);
            Selection.activeObject = newTex;
            //bool wontReplace = EditorUtility.DisplayDialog("Replace references", "Would you replace all the references of the source generated texture to the baked one on assets ?", "No", "Replace");
            //if(!wontReplace) {
            //    string targetAssetPath = AssetDatabase.GetAssetPath(m_generatedTextureField.GetValue(target) as Texture2D);
            //    Debug.Log("Replace all references to the texture at path : " + targetAssetPath);
            //    Hash128 dependenciesHash = AssetDatabase.GetAssetDependencyHash(targetAssetPath);
            //}
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