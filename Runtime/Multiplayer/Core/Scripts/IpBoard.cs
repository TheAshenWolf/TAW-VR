#if MIRROR
using System;
using System.Text;
using System.Text.RegularExpressions;
using Mirror;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace TAW_VR.Runtime.Multiplayer.Core.Scripts
{
    public class IpBoard : MonoBehaviour
    {
        [Title("Components")] 
        public TextMeshPro text;
        public TelepathyTransport transport;
        public NetworkManager manager;
        
        private string _ip = "____________";
        private int _pointer = 0;

        private string GetPort()
        {
            return transport.port.ToString();
        }

        public string GetIP()
        {
            return $"{_ip.Substring(0,3)}.{_ip.Substring(3,3)}.{_ip.Substring(6,3)}.{_ip.Substring(9,3)}";
        }

        public string GetFormattedIP()
        {
            return $"{_ip.Substring(0,3).TrimStart('0')}.{_ip.Substring(3,3).TrimStart('0')}.{_ip.Substring(6,3).TrimStart('0')}.{_ip.Substring(9,3).TrimStart('0')}";
        }
        
        private void Start()
        {
            enabled = false; // disable ticking
        }

        public void NumberClick(int number)
        {
            if (_pointer < 12)
            {
                StringBuilder builder = new StringBuilder(_ip);
                builder[_pointer] = number.ToString()[0];
                _ip = builder.ToString();
                _pointer++;
            }
            UpdateText();
        }

        public void ConfirmClick()
        {
            if (!ValidateIp()) return;
            else
            {
                manager.networkAddress = GetFormattedIP();
                manager.StartClient();
            }
        }

        public void RemoveClick()
        {
            if (_pointer > 0)
            {
                StringBuilder builder = new StringBuilder(_ip)
                {
                    [_pointer - 1] = '_'
                };
                _ip = builder.ToString();
                _pointer--;
            }
            UpdateText();
        }

        private bool ValidateIp()
        {
            return Regex.IsMatch(GetIP(),
                "^((25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\.){3}(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$");
        }

        public void UpdateText()
        {
            text.text = GetIP() + ":" + GetPort();
        }
        
    }
}
#endif