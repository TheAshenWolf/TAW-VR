#if MIRROR
using System.Collections.Generic;
using Mirror;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TAW_VR.Runtime.Multiplayer.Core.Scripts
{
  public class NetworkVrRig : NetworkBehaviour
  {
    [Title("Renderers")] public List<Renderer> serverRenderers;
    public List<Renderer> clientRenderers;

    [Title("GameObjects")] public GameObject hostCrown;

    private Color _color;

    public void Start()
    {
      if (!isServer && isLocalPlayer || isLocalPlayer)
      {
        hostCrown.SetActive(false);
      }

      if (isLocalPlayer)
      {
        foreach (Renderer serverRenderer in serverRenderers)
        {
          serverRenderer.enabled = false;
        }
      }
      else
      {
        foreach (Renderer clientRenderer in clientRenderers)
        {
          clientRenderer.enabled = false;
        }
      }

      SetColor();
    }

    private void SetColor()
    {
      _color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1);

      foreach (Renderer serverRenderer in serverRenderers)
      {
        if (serverRenderer.name == "Eye") continue;
        serverRenderer.material.color = _color;
      }
    }
  }
}
#endif