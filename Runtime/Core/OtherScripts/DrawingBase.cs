using UnityEngine;

namespace TAW_VR.Runtime.Core.OtherScripts
{
  public class DrawingBase : MonoBehaviour
  {
    public int textureSize = 1024;
    public Color color;
    public bool showPreview;

    [SerializeField] private Shader shader;

    private Camera _camera;
    private Material _drawMaterial, _baseMaterial, _originalMaterial;
    private RenderTexture _splatMap;
    private Renderer _meshRenderer;
    private bool _isDrawing;

    private void Start()
    {
      _meshRenderer = GetComponent<MeshRenderer>();
      _originalMaterial = _meshRenderer.material;
      
      _baseMaterial = new Material(shader)
      {
        mainTexture = _originalMaterial.mainTexture,
        color = _originalMaterial.color
      };
      _meshRenderer.material = _baseMaterial;
      
      _splatMap = new RenderTexture(textureSize, textureSize, 0, RenderTextureFormat.ARGBFloat);
      _drawMaterial = new Material(shader);

      _drawMaterial.SetVector("_DrawColor", color);
      _drawMaterial.mainTexture = _baseMaterial.mainTexture;
      _baseMaterial.SetTexture("_Splat", _splatMap);
    }

    public void Draw(float x, float y)
    {
      _drawMaterial.SetInt("_IsDrawing", 1);
      _drawMaterial.SetVector("_Coordinate", new Vector4(x, y, 0, 0));

      RenderTexture tmp = RenderTexture.GetTemporary(_splatMap.width, _splatMap.height, 0,
        RenderTextureFormat.ARGBFloat);
      Graphics.Blit(_splatMap, tmp);
      Graphics.Blit(tmp, _splatMap, _drawMaterial);
      RenderTexture.ReleaseTemporary(tmp);

      _drawMaterial.SetInt("_IsDrawing", 0);
    }

    private void OnValidate()
    {
      _drawMaterial.SetVector("_DrawColor", color);
    }

    private void OnGUI()
    {
      if (!showPreview) return;
      GUI.DrawTexture(new Rect(0, 0, 256, 256), _splatMap, ScaleMode.ScaleToFit, false, 1);
    }
  }
}