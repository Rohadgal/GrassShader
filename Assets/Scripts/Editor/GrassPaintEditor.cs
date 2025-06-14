using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GrassPaintTarget))]
public class GrassPainterEditor : Editor
{
    private void OnSceneGUI()
    {
        GrassPaintTarget targetScript = (GrassPaintTarget)target;
        Event e = Event.current;

        if ((e.type == EventType.MouseDown || e.type == EventType.MouseDrag) && e.button == 0)
        {
            Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Vector2 uv = hit.textureCoord;
                targetScript.PaintAtUV(uv);
                e.Use();
                Debug.Log("PAINT");
            }
        }
    }
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GrassPaintTarget paintTarget = (GrassPaintTarget)target;
        if (GUILayout.Button("Clear Grass Mask"))
        {
            paintTarget.ClearMask();
        }
    }
}