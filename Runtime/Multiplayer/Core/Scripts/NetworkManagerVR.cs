#if MIRROR
using Mirror;

namespace TAW_VR.Runtime.Multiplayer.Core.Scripts
{
    public class NetworkManagerVR : NetworkManager
    {
        public override void Start()
        {
            base.Start();

            if (!isNetworkActive)
            {
                StartHost();
            }
            else
            {
                networkAddress = "localhost";
                StartClient();
            }
        }
    }
}

#endif