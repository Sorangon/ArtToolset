using UnityEngine;
#if UNITY_EDITOR
using UnityEditor; 
#endif

public class ProceduralTexture : Texture {
#if UNITY_EDITOR
    [MenuItem("Test/Create Texture")]
    private static void CreateTexture() {
        ProceduralTexture pt = new ProceduralTexture();
        AssetDatabase.CreateAsset(pt, "Assets/ProceduralTexture.prst");   
    }
#endif
}
