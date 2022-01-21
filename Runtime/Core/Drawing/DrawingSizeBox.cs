using System;
using UnityEngine;

namespace TAW_VR.Runtime.Core.Drawing
{
  public class DrawingSizeBox : MonoBehaviour
  {
    [SerializeField] private DrawingBrushSizeSetter drawingBrushSizeSetter;
    [SerializeField] private int size;
    private void Start()
    {
      this.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
      drawingBrushSizeSetter.SetBrushSize(size);
    }
  }
}