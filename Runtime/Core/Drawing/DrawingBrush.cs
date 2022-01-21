using UnityEngine;

namespace TAW_VR.Runtime.Core.Drawing
{
  public class DrawingBrush : MonoBehaviour
  {
    public Transform brushHead;
    [ColorUsage(false)] public Color brushColor;
    public MeshRenderer meshRenderer;
    [Range(1, 10)]
    public int brushSize = 1;

    private void Start()
    {
      brushColor.a = 1;
      meshRenderer.material.color = brushColor;
    }

    private void OnTriggerEnter(Collider other)
    {
      DrawingColorSource colorSource = other.GetComponent<DrawingColorSource>();
      
      if (colorSource == null) return;
      brushColor = colorSource.sourceColor;
      brushColor.a = 1;
      meshRenderer.material.color = brushColor;
    }
  }
}