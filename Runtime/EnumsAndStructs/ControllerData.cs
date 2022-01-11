using System;
using UnityEngine;

namespace TawVR
{
  [Serializable]
  public struct ControllerData
  {
    public Vector3 position;
    public Quaternion rotation;
    public Vector3 acceleration;
    public Vector3 velocity;
    public Vector3 angularAcceleration;
    public Vector3 angularVelocity;

    public float gripPressure;
    public bool gripHold; // true when held down

    public float triggerPressure;
    public bool triggerHold; // true when finger is touching the button (what the fuck Oculus?)

    public bool axButtonHold; // true when held down
    public bool byButtonHold; // true when held down
    
    public Vector2 joystickAxis;
    public bool joystickHold; // true when held down
  }
}