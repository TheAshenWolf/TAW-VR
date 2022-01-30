#if MIRROR
using Sirenix.OdinInspector;
using UnityEngine;

namespace TAW_VR.Runtime.Multiplayer.Core.Scripts
{
  public class IpBoardButton : MonoBehaviour
  {
    public int button;
    public IpBoard board;
    
    [Button]
    private void OnTriggerEnter(Collider other)
    {
      board.NumberClick(button);
    }
  }
}
#endif
