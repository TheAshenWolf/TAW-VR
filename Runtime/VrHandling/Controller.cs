using UnityEngine.XR;

namespace TAW_VR.VrHandling
{
  public class Controller
  {
    public InputDevice device;
    public ControllerData data;

    public Controller(InputDevice inputDevice)
    {
      device = inputDevice;
      data = new ControllerData();
    }

    public void SendHaptic(float duration = 1f)
    {
      device.SendHapticImpulse(0, .5f, duration);
    }
  }
}