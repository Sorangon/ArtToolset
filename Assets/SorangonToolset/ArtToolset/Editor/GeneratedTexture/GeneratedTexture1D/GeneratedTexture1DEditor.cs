namespace SorangonToolset.ArtToolset.Editors {
    using UnityEditor;
    using UnityEngine;

    public class GeneratedTexture1DEditor : GeneratedTextureEditor {
        #region Serialized Properties
        private SerializedProperty m_mappingProp = null;
        private SerializedProperty m_invertProp = null;

        protected override void FindProperties() {
            base.FindProperties();
            m_mappingProp = serializedObject.FindProperty("m_mapping");
            m_invertProp = serializedObject.FindProperty("m_invert");
        }
        #endregion

        #region Drawing
        protected override void DrawInspector() {
            base.DrawInspector();
            GUILayout.Space(8f);

            //TODO : Fix reference loss
            //EditorGUI.BeginChangeCheck();
            //EditorGUILayout.PropertyField(m_mappingProp);
            //if(EditorGUI.EndChangeCheck()) {
            //    SetComputeFlagUp();
            //    SetRecreateTextureFlagUp();
            //}

            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(m_invertProp);
            if(EditorGUI.EndChangeCheck()) {
                SetComputeFlagUp();
            }
        }
        #endregion
    }
}