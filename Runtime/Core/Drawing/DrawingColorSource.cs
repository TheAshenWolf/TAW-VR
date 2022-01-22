using UnityEngine;

namespace TAW_VR.Runtime.Core.Drawing
{
  public class DrawingColorSource : MonoBehaviour
  {
    public Renderer colorRenderer;
    public Color sourceColor;

    private void Start()
    {
      sourceColor.a = 1;
      colorRenderer.material.color = sourceColor;
    }

    private void OnDrawGizmos()
    {
      sourceColor.a = 1;
      Gizmos.color = sourceColor;
      Gizmos.DrawSphere(transform.position + new Vector3(0, .05f, 0), .025f);
    }
  }
}