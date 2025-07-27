using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class CanvasImagePlacerEditor : EditorWindow
{
    public GameObject imagePrefab;
    public Canvas canvas;
    public PlacedUIImageData saveData;

    private List<GameObject> placedImages = new List<GameObject>();
    private bool placingEnabled = false;

    //private float animatorSpeed = 1f;
    //private float animationDuration = 2f;
    private Vector2 imageSize = new Vector2(100, 100);

    private const string rootObjectName = "PlacedUIImagesRoot";
    private GameObject root;

    [MenuItem("Tools/Canvas Image Placer")]
    public static void ShowWindow()
    {
        GetWindow<CanvasImagePlacerEditor>("Canvas Image Placer");
    }

    private void OnGUI()
    {
        GUILayout.Label("Place UI Images on Canvas", EditorStyles.boldLabel);
        imagePrefab = (GameObject)EditorGUILayout.ObjectField("UI Image Prefab", imagePrefab, typeof(GameObject), false);
        canvas = (Canvas)EditorGUILayout.ObjectField("Target Canvas", canvas, typeof(Canvas), true);
        saveData = (PlacedUIImageData)EditorGUILayout.ObjectField("Save Data Asset", saveData, typeof(PlacedUIImageData), false);

        //GUILayout.Space(10);
        //GUILayout.Label("Animation Settings", EditorStyles.boldLabel);
        //animatorSpeed = EditorGUILayout.FloatField("Animator Speed", animatorSpeed);
        //animationDuration = EditorGUILayout.FloatField("Animation Duration (sec)", animationDuration);

        GUILayout.Space(10);
        GUILayout.Label("Image Size", EditorStyles.boldLabel);
        imageSize = EditorGUILayout.Vector2Field("Width x Height", imageSize);

        GUILayout.Space(10);
        if (placingEnabled)
        {
            if (GUILayout.Button("Stop Placing"))
            {
                placingEnabled = false;
                SceneView.duringSceneGui -= OnSceneGUI;
            }
        }
        else
        {
            if (GUILayout.Button("Start Placing"))
            {
                if (imagePrefab != null && canvas != null)
                {
                    EnsureRootExists();
                    placingEnabled = true;
                    SceneView.duringSceneGui += OnSceneGUI;
                }
                else
                {
                    Debug.LogWarning("Assign both a UI Image Prefab and a Canvas.");
                }
            }
        }

        GUILayout.Space(10);

        if (GUILayout.Button("Restore All From Save Data"))
        {
            RestoreFromData();
        }

        if (GUILayout.Button("Clear Placed Images"))
        {
            ClearPlacedImages();

            if (saveData != null)
            {
                saveData.Clear();
                EditorUtility.SetDirty(saveData);
            }
        }

        GUILayout.Label($"Placed: {placedImages.Count}");
    }

    private void OnSceneGUI(SceneView sceneView)
    {
        Event e = Event.current;

        if (e.type == EventType.MouseDown && e.button == 0 && !e.alt)
        {
            Vector2 screenPoint = HandleUtility.GUIPointToScreenPixelCoordinate(e.mousePosition);
            PlaceImage(screenPoint);
            e.Use();
        }
    }

    private void PlaceImage(Vector2 screenPoint)
    {
        EnsureRootExists();

        Vector2 canvasPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            screenPoint,
            SceneView.currentDrawingSceneView.camera,
            out canvasPos
        );

        GameObject newImage = (GameObject)PrefabUtility.InstantiatePrefab(imagePrefab, root.transform);
        RectTransform rt = newImage.GetComponent<RectTransform>();
        rt.anchoredPosition = canvasPos;
        rt.sizeDelta = imageSize;

        // Animator animator = newImage.GetComponent<Animator>();
        // if (animator != null)
        // {
        //     animator.speed = animatorSpeed;
        // }

        placedImages.Add(newImage);
        Undo.RegisterCreatedObjectUndo(newImage, "Place UI Image");

        if (saveData != null)
        {
            saveData.Add(canvasPos, imageSize);
            EditorUtility.SetDirty(saveData);
        }
    }

    private void RestoreFromData()
    {
        if (imagePrefab == null || canvas == null || saveData == null)
        {
            Debug.LogWarning("Missing canvas, prefab, or data to restore.");
            return;
        }

        ClearPlacedImages();
        EnsureRootExists();

        foreach (var entry in saveData.placedEntries)
        {
            GameObject newImage = (GameObject)PrefabUtility.InstantiatePrefab(imagePrefab, root.transform);
            RectTransform rt = newImage.GetComponent<RectTransform>();
            rt.anchoredPosition = entry.position;
            rt.sizeDelta = entry.size;

            // Animator animator = newImage.GetComponent<Animator>();
            // if (animator != null)
            // {
            //     animator.speed = animatorSpeed;
            // }

            placedImages.Add(newImage);
            Undo.RegisterCreatedObjectUndo(newImage, "Restore UI Image");
        }
    }

    private void ClearPlacedImages()
    {
        if (root != null)
        {
            foreach (Transform child in root.transform)
            {
                DestroyImmediate(child.gameObject);
            }
        }

        placedImages.Clear();
    }

    private void EnsureRootExists()
    {
        if (root == null)
        {
            var existing = GameObject.Find(rootObjectName);
            if (existing != null)
            {
                root = existing;
            }
            else
            {
                root = new GameObject(rootObjectName, typeof(RectTransform));
                root.transform.SetParent(canvas.transform, false);

                RectTransform rt = root.GetComponent<RectTransform>();
                rt.anchorMin = Vector2.zero;
                rt.anchorMax = Vector2.one;
                rt.offsetMin = Vector2.zero;
                rt.offsetMax = Vector2.zero;
            }
        }
    }

    private void OnDisable()
    {
        SceneView.duringSceneGui -= OnSceneGUI;
    }
}
