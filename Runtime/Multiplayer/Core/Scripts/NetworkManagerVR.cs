#if MIRROR
using Mirror;
using UnityEngine;

namespace TAW_VR.Runtime.Multiplayer.Core.Scripts
{
    public class NetworkManagerVR : NetworkManager
    {
        /*public override void Start()
        {
            base.Start();

            NetworkClient.OnDisconnectedEvent += () =>
            {
                Debug.LogError("DC");
            };
            
            networkAddress = "localhost";
            StartClient();
        }*/
    }
}

#endif