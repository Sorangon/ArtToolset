//Created by Julien Delaunay (Sorangon)
//Repository link : https://github.com/Sorangon/ArtToolset


namespace SorangonToolset.ArtToolset.Editors {
    using UnityEngine;
    using UnityEditor;

    /// <summary>
    /// The custom editor class of the Ramp Texture
    /// </summary>
    [CustomEditor(typeof(RampTexture))]
    public class RampTextureEditor : GeneratedTextureEditor {
        #region Styles
        private GUIContent m_gradientLabelContent = new GUIContent("Ramp", "The gradient that will be computed into a ramp texture (RGBA)");
        #endregion

        #region Serialized Properties
        private SerializedProperty m_rampProperty = null;

        protected override void FindProperties() {
            m_rampProperty = serializedObject.FindProperty("ramp");
        }
        #endregion

        #region Callbacks
        public override void OnInspectorGUI() {
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(m_rampProperty, m_gradientLabelContent);
            if(EditorGUI.EndChangeCheck()) {
                ComputeTexture();
            }

            serializedObject.ApplyModifiedProperties();
        }
        #endregion
    }
}