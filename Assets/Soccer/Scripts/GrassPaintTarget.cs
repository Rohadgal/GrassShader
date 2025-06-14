using UnityEngine;

[ExecuteAlways]
public class GrassPaintTarget : MonoBehaviour
{
    public RenderTexture renderTexture;
    public Texture2D brushTex;
    public Material brushMat;
    public Material grassMaterial;
    [Range(0.01f, 0.2f)] public float brushSize = 0.05f;
    public Color brushColor = Color.white;

    private void OnEnable()
    {
        if (grassMaterial && renderTexture)
            grassMaterial.SetTexture("_GrassMask", renderTexture);
        

    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                PaintAtUV(hit.textureCoord);
            }
        }
    }
    public void ClearMask()
    {
        if (renderTexture == null) return;

        RenderTexture.active = renderTexture;
        GL.Clear(true, true, Color.clear);  // Clear color and depth buffers to transparent
        RenderTexture.active = null;
    }


    public void PaintAtUV(Vector2 uv)
    {
        if (!brushMat)
            brushMat = new Material(Shader.Find("Hidden/GrassBrush"));

        brushMat.SetTexture("_BrushTex", brushTex);
        brushMat.SetColor("_Color", brushColor);
        brushMat.SetFloat("_BrushSize", brushSize);
        brushMat.SetVector("_BrushUV", new Vector4(uv.x, uv.y, 0, 0));

        RenderTexture.active = renderTexture;
        Graphics.Blit(null, renderTexture, brushMat);
        RenderTexture.active = null;

        if (grassMaterial)
            grassMaterial.SetTexture("_GrassMask", renderTexture);
    }

    public void SaveRenderTexture()
    {
        RenderTexture.active = renderTexture;
        Texture2D tex = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGBA32, false);
        tex.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        tex.Apply();
        RenderTexture.active = null;

        byte[] bytes = tex.EncodeToPNG();
        System.IO.File.WriteAllBytes(Application.dataPath + "/SavedGrassMask.png", bytes);
#if UNITY_EDITOR
        UnityEditor.AssetDatabase.Refresh();
#endif
    }
}