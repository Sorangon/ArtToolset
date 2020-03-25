//Created by Julien Delaunay (Sorangon)
//Repository link : https://github.com/Sorangon/ArtToolset

using UnityEngine;
using UnityEditor;

namespace SorangonToolset.ArtToolset.Editors {

    /// <summary>
    /// The custom editor class of the Ramp Texture
    /// </summary>
    [CustomEditor(typeof(RampTexture))]
    public class RampTextureEditor : Editor {
        #region Current
        private RampTexture _target = null;
        #endregion

        #region Serialized Properties
        private SerializedProperty _rampProperty = null;
        private SerializedProperty _textureProperty = null;

        private void FindProperties() {
            _rampProperty = serializedObject.FindProperty("_ramp");
            _textureProperty = serializedObject.FindProperty("_generatedTexture");
        }
        #endregion

        #region Callbacks
        private void OnEnable() {
            FindProperties();
            _target = target as RampTexture;
        }

        public override void OnInspectorGUI() {
            EditorGUI.BeginChangeCheck();
            Gradient grad = EditorGUILayout.GradientField("Ramp", _target._ramp);

            if(EditorGUI.EndChangeCheck()) {
                if(_textureProperty.objectReferenceValue == null) {
                    GenerateTextureAsset();
                }

                ComputeTexture(grad);
                _target._ramp = grad;
            }

            EditorGUILayout.LabelField("Pouette");
        }
        #endregion

        #region Texture
        /// <summary>
        /// Compute the texture from the asset gradient and parameters
        /// </summary>
        private void ComputeTexture(Gradient ramp) {
            Texture2D tex = _textureProperty.objectReferenceValue as Texture2D;

            for(int i = 0; i < tex.width; i++) {
                float ratio = (float)i / (float)tex.width;
                tex.SetPixel(i, 0, ramp.Evaluate(ratio));
            }

            tex.Apply();
            serializedObject.ApplyModifiedProperties();
        }

        private void GenerateTextureAsset() {
            Texture generatedtexture = new Texture2D(516, 1);
            generatedtexture.name = target.name + "_Texture";
            Debug.Log("Try generate texture");
            RampTexture targetAsset = target as RampTexture;
            AssetDatabase.AddObjectToAsset(generatedtexture, targetAsset);
            AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(generatedtexture));
            AssetDatabase.Refresh();
            _textureProperty.objectReferenceValue = generatedtexture;
            serializedObject.ApplyModifiedProperties();
        }

        #endregion
    }
}