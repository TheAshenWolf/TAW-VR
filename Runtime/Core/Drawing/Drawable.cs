using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using TAW_VR.Runtime.Core.EnumsAndStructs;
using UnityEngine;

namespace TAW_VR.Runtime.Core.Drawing
{
  [DisallowMultipleComponent, RequireComponent(typeof(MeshCollider)), RequireComponent(typeof(Rigidbody))]
  public class Drawable : MonoBehaviour
  {
    [Title("Components")]
    
    [Title("Settings")]
    [Range(5, 13)]
    public int texturePower = 10;
    [ColorUsage(false)]
    public Color drawingColor;
    public DrawingShaderMode drawingShaderMode = DrawingShaderMode.UnlitOpaque;

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

    private void Start()
    {
      _meshRenderer = GetComponent<MeshRenderer>();
      _originalMaterial = _meshRenderer.material;

      /*switch (drawingShaderMode)
      {
        case DrawingShaderMode.Surface:
          break;
        case DrawingShaderMode.UnlitOpaque:
          break;
        case DrawingShaderMode.Transparent:
          break;
        default:
          throw new ArgumentOutOfRangeException(nameof(drawingShaderMode),"This shader mode does not exist");
      }*/
        _drawingShader = _drawingShader = Shader.Find("Unlit/DrawingShader");
        _baseMaterial = new Material(_drawingShader)
        {
          mainTexture = _originalMaterial.mainTexture,
          color = _originalMaterial.color
        };
        _meshRenderer.material = _baseMaterial;

        splatMap = new RenderTexture(_textureSize, _textureSize, 0, RenderTextureFormat.ARGBFloat);
        _drawMaterial = new Material(_drawingShader);
        
        _drawMaterial.SetVector("_DrawColor", drawingColor);
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

    private void OnCollisionStay(Collision collision)
    {
      List<Vector3> contactPoints = collision.contacts.Select(x => x.point).ToList();
      Vector3 averageImpact = new Vector3(0, 0, 0);
      contactPoints.ForEach(point => averageImpact += point);
      averageImpact /= collision.contactCount;
      
      
    }

    private void OnTriggerStay(Collider other)
    {
      //other.ClosestPointOnBounds()
      throw new NotImplementedException();
    }
  }
}