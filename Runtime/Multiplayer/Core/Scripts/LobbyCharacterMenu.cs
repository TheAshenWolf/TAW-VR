#if MIRROR
using Mirror;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace TAW_VR.Runtime.Multiplayer.Core.Scripts
{
  public class LobbyCharacterMenu : MonoBehaviour
  {
    public UnityEvent triggerEnterCallback;
    public NetworkManager networkManager;

    public void OnTriggerEnter(Collider other)
    {
      triggerEnterCallback?.Invoke();
    }

    [Button]
    public void HostServer()
    {
      networkManager.StartHost();
    }

    [Button]
    public void ConnectOnLocalNetwork()
    {
      networkManager.networkAddress = "localhost";
      networkManager.StartClient();
    }
  }
}
#endif