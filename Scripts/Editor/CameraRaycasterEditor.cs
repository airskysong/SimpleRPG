using UnityEditor;

namespace MyRPG.CameraUI
{
    [CustomEditor(typeof(CameraRaycaster))]
    public class CameraRaycasterEditor : Editor
    {
        bool isLayerPriorityUnfolded = false;

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            isLayerPriorityUnfolded = EditorGUILayout.Foldout(isLayerPriorityUnfolded, "LayerPriorities");
            if (!isLayerPriorityUnfolded)
            {
                EditorGUI.indentLevel++;
                {
                    BindArraySize();
                    BindElements();
                }
                EditorGUI.indentLevel--;
            }
            serializedObject.ApplyModifiedProperties();
        }
        void BindArraySize()
        {
            int currenArraySize = serializedObject.FindProperty("layerPriorities.Array.size").intValue;
            int requiredArraySize = EditorGUILayout.IntField("Size", currenArraySize);
            if (currenArraySize != requiredArraySize)
            {
                serializedObject.FindProperty("layerPriorities.Array.size").intValue = requiredArraySize;
            }
        }
        void BindElements()
        {
            int currentArraySize = serializedObject.FindProperty("layerPriorities.Array.size").intValue;
            for (int i = 0; i < currentArraySize; i++)
            {
                var prop = serializedObject.FindProperty(string.Format("layerPriorities.Array.data[{0}]", i));
                prop.intValue = EditorGUILayout.LayerField(string.Format("Layer{0}", i), prop.intValue);
            }
        }
    }
}
