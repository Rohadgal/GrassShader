using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PlacedUIImageData))]
public class PlacedUIImageDataEditor : Editor
{
    private PlacedUIImageData data;

    private void OnEnable()
    {
        data = (PlacedUIImageData)target;
    }

    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("Clear All"))
        {
            Undo.RecordObject(data, "Clear All Entries");
            data.Clear();
            EditorUtility.SetDirty(data);
        }

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Stored Entries", EditorStyles.boldLabel);

        for (int i = 0; i < data.placedEntries.Count; i++)
        {
            var entry = data.placedEntries[i];
            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.LabelField($"Entry {i + 1}");

            entry.position = EditorGUILayout.Vector2Field("Position", entry.position);
            entry.size = EditorGUILayout.Vector2Field("Size", entry.size);

            if (GUILayout.Button("Delete"))
            {
                Undo.RecordObject(data, "Delete Entry");
                data.placedEntries.RemoveAt(i);
                EditorUtility.SetDirty(data);
                break; // avoid modifying list during loop
            }

            EditorGUILayout.EndVertical();
        }

        if (GUI.changed)
        {
            EditorUtility.SetDirty(data);
        }
    }
}