using System;
using UnityEngine;

namespace TAW_VR.Runtime.Core.Drawing
{
  public class DrawingColorSource : MonoBehaviour
  {
    public Renderer colorRenderer;
    public Color sourceColor;

    private void Start()
    {
      colorRenderer.material.color = sourceColor;
    }

    private void OnDrawGizmos()
    {
      Gizmos.color = sourceColor;
      Gizmos.DrawSphere(transform.position + new Vector3(0, .15f, 0), .05f);
    }
  }
}