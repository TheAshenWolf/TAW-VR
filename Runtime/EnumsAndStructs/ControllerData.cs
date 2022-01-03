using UnityEngine;

namespace TawVR
{
  public struct ControllerData
  {
    public Vector3 position;
    public Quaternion rotation;
    public Vector3 acceleration;
    public Vector3 velocity;
    public Vector3 angularAcceleration;
    public Vector3 angularVelocity;

    public float gripPressure;
    public bool gripClick;

    public float triggerPressure;
    public bool triggerClick;

    public bool axButtonClick;
    public bool byButtonClick;

    public Vector2 joystickAxis;
    public bool joystickClick;
  }
}