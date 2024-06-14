using UnityEditor;
using UnityEngine;

public class DuplicateComponentsEditor : EditorWindow
{
    GameObject sourceObject;
    GameObject targetObject;

    [MenuItem("Tools/Duplicate Components")]
    public static void ShowWindow()
    {
        GetWindow<DuplicateComponentsEditor>("Duplicate Components");
    }

    void OnGUI()
    {
        GUILayout.Label("Duplicate Components", EditorStyles.boldLabel);

        sourceObject = EditorGUILayout.ObjectField("Source Object", sourceObject, typeof(GameObject), true) as GameObject;
        targetObject = EditorGUILayout.ObjectField("Target Object", targetObject, typeof(GameObject), true) as GameObject;

        if (GUILayout.Button("Duplicate"))
        {
            Duplicate();
        }
    }

    void Duplicate()
    {
        if (sourceObject == null || targetObject == null)
        {
            Debug.LogError("Source and Target objects must be set.");
            return;
        }

        Component[] components = sourceObject.GetComponents<Component>();
        foreach (Component comp in components)
        {
            if (comp is Transform)
                continue;

            UnityEditorInternal.ComponentUtility.CopyComponent(comp);
            UnityEditorInternal.ComponentUtility.PasteComponentAsNew(targetObject);
        }
    }
}
