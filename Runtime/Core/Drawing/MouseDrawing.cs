using UnityEngine;

namespace TAW_VR.Runtime.Core.Drawing
{
  public class MouseDrawing : MonoBehaviour
  {
    public Camera mainCamera;
    
    private Material _drawMaterial, _baseMaterial;
    private RenderTexture _splatMap;
    private RaycastHit _hit;
    
    private DrawingBase _base;
    
    private void Start()
    {
      _base = GetComponent<DrawingBase>();
    }

    private void Update()
    {
      if (Input.GetKey(KeyCode.Mouse0))
      {
        if (Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out _hit))
        {
          // This is actually required. Otherwise we draw on all objects
          if (_hit.transform.gameObject != gameObject) return;
          _base.Draw(_hit.textureCoord2.x, _hit.textureCoord2.y);
        }
      }
    }
  }
}