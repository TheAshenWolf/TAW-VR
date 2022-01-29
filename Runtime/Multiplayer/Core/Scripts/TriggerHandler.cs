#if MIRROR
using Mirror;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace TAW_VR.Runtime.Multiplayer.Core.Scripts
{
  public class TriggerHandler : MonoBehaviour
  {
    public UnityEvent triggerEnterCallback;

    public void OnTriggerEnter(Collider other)
    {
      triggerEnterCallback?.Invoke();
    }
  }
}
#endif