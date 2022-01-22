using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace TAW_VR.Runtime.Core.Drawing
{
  [DisallowMultipleComponent, RequireComponent(typeof(MeshCollider)), RequireComponent(typeof(Rigidbody))]
  public class Drawable : MonoBehaviour
  {
    [Title("Settings")]
    [Range(5, 11)]
    public int texturePower = 10;

    [Title("Details")] 
    public RenderTexture splatMap;
    
    [Title("Privates")] 
    private int _textureSize;
    private MeshRenderer _meshRenderer;
    private Material _drawMaterial;
    private Material _originalMaterial;
    private Material _baseMaterial;
    private bool _isDrawing;
    private Shader _drawingShader;
    private Vector2 _collisionCoordinates;
    private RaycastHit _hit;

    private void Start()
    {
      _textureSize = 2 << texturePower - 1;
      _meshRenderer = GetComponent<MeshRenderer>();
      _originalMaterial = _meshRenderer.material;
      _drawingShader = _drawingShader = Shader.Find("Drawing/DrawingUnlit");
      _baseMaterial = new Material(_drawingShader)
        {
          mainTexture = _originalMaterial.mainTexture,
          color = _originalMaterial.color
        };
        _meshRenderer.material = _baseMaterial;

        splatMap = new RenderTexture(_textureSize, _textureSize, 0, RenderTextureFormat.ARGBFloat);
        _drawMaterial = new Material(_drawingShader);
        
        _drawMaterial.SetVector("_DrawColor", Color.white);
        _drawMaterial.mainTexture = _baseMaterial.mainTexture;
        _baseMaterial.SetTexture("_Splat", splatMap);
    }

    private void Update()
    {
      if (!_isDrawing) return;
      _drawMaterial.SetInt("_IsDrawing", 1);
      _drawMaterial.SetVector("_Coordinate", new Vector4(_collisionCoordinates.x, _collisionCoordinates.y, 0, 0));
      
      RenderTexture tmp = RenderTexture.GetTemporary(splatMap.width, splatMap.height, 0, RenderTextureFormat.ARGBFloat);
      Graphics.Blit(splatMap, tmp);
      Graphics.Blit(tmp, splatMap, _drawMaterial);
      RenderTexture.ReleaseTemporary(tmp);
      
      _drawMaterial.SetInt("_IsDrawing", 0);
    }

    private void OnCollisionEnter(Collision collision)
    {
      Debug.LogError("IN");
      DrawingBrush brush = collision.transform.GetComponent<DrawingBrush>();
      if (brush == null) return;
      
      _drawMaterial.SetVector("_DrawColor", brush.brushColor);
      _drawMaterial.SetFloat("_BrushSize", 1 - 0.001f * brush.brushSize);
    }

    private void OnCollisionStay(Collision collision)
    {
      Debug.LogError("COLL");
      DrawingBrush brush = collision.transform.GetComponent<DrawingBrush>();
      if (brush == null) return;
      
      Vector3 brushPosition = brush.brushHead.position;

      if (Physics.Raycast(brushPosition, brush.transform.forward, out _hit))
      {
        _collisionCoordinates = new Vector2(_hit.textureCoord2.x, _hit.textureCoord2.y);
        _isDrawing = true;
      }
      else _isDrawing = false;
    }

    private void OnTriggerEnter(Collider other)
    {
      DrawingBrush brush = other.GetComponent<DrawingBrush>();
      if (brush == null) return;
      
      _drawMaterial.SetVector("_DrawColor", brush.brushColor);
      _drawMaterial.SetFloat("_BrushSize", 1 - 0.001f * brush.brushSize);
    }

    private void OnTriggerStay(Collider other)
    {
      DrawingBrush brush = other.GetComponent<DrawingBrush>();
      if (brush == null) return;
      
      Vector3 brushPosition = brush.brushHead.position;
      
      if (Physics.Raycast(brushPosition,  brush.transform.forward, out _hit))
      {
        _collisionCoordinates = new Vector2(_hit.textureCoord2.x, _hit.textureCoord2.y);
        _isDrawing = true;
      }
      else _isDrawing = false;
    }
  }
}