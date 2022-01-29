using System.Net;
using TMPro;
using UnityEngine;

namespace TAW_VR.Runtime.Multiplayer.Core.Scripts
{
  public class IpDisplay : MonoBehaviour
  {
    public TextMeshPro ipField;

    private void Start()
    {
      IPHostEntry host = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName());
      foreach (IPAddress ip in host.AddressList)
      {
        if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
        {
          ipField.text = ip.ToString();
        }
      }
    }
  }
}