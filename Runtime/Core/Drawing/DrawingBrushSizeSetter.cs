using System;
using UnityEngine;

namespace TAW_VR.Runtime.Core.Drawing
{
  public class DrawingBrushSizeSetter : MonoBehaviour
  {
    public Renderer[] renderers = new Renderer[10];
    public DrawingBrush brush;

    private void Start()
    {
      SetBrushSize(brush.brushSize);
    }

    public void SetBrushSize(int size)
    {
      int iterator = 0;
      foreach (Renderer rend in renderers)
      {
        iterator++;
        rend.material.color = iterator <= size ? Color.green : Color.white;
      }
      brush.brushSize = size;
    }
  }
}