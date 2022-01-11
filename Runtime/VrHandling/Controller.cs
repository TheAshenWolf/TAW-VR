using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.XR;

namespace TawVR
{
  public class Controller : MonoBehaviour
  {
    public LineRenderer lineRenderer;
    public InputDevice device;
    public ControllerData data;
    public VrHardware hardwarePart;
    [Required] public VrRig hmd;
    private Transform _transform;
    private bool _teleportReady;
    private bool _canTeleport;
    private bool _attemptTeleport;
    private Vector3 _teleportLocation;
    private Vector3 _position;
    private bool _onTriggerPressureDown;
    private bool _onGripPressureDown;
    private float _clickValue;
    private float _releaseValue;
    public bool initialized;

    private bool _joystickReset;
    private bool _axReset;
    private bool _byReset;

    public void Init(InputDevice inputDevice, VrHardware hwPart)
    {
      device = inputDevice;
      data = new ControllerData();
      hardwarePart = hwPart;

      _clickValue = hmd.clickValue;
      _releaseValue = hmd.releaseValue;
      initialized = true;
    }

    private void Start()
    {
      _transform = transform;
      if (triggerCollider == null) throw new Exception("Trigger collider was not assigned.");
    }

    private void Update()
    {
      if (!initialized) return;
      
      _position = _transform.position;
      _transform.localPosition = data.position;
      _transform.localRotation = data.rotation;
      
      Teleportation();
      HandleCalls();
    }

    public void SendHaptic(float duration = 1f)
    {
      device.SendHapticImpulse(0, .5f, duration);
    }

    #region Grabber

    public Collider triggerCollider;

    public Grabbable grabbedObject;
    private Collider _proximityItem;


    private void OnTriggerEnter(Collider other)
    {
      SendHaptic();
      Grabbable grabbable = other.GetComponent<Grabbable>();
      if (grabbable == null) return;

      _proximityItem = other;
    }

    private void OnCollisionEnter(Collision collision)
    {
      SendHaptic();
      Grabbable grabbable = collision.collider.GetComponent<Grabbable>();
      if (grabbable == null) return;

      _proximityItem = collision.collider;
    }

    private void OnTriggerExit(Collider other)
    {
      if (_proximityItem == other) _proximityItem = null;
    }

    private void OnCollisionExit(Collision collision)
    {
      if (_proximityItem == collision.collider) _proximityItem = null;
    }

    public void GrabObject() // Attempts to grab the item that is being interacted with
    {
      if (_proximityItem == null) return;

      grabbedObject = _proximityItem.GetComponent<Grabbable>();
      grabbedObject.OnGrabbed(this);
    }

    public void ReleaseObject() // Drops the held item
    {
      if (grabbedObject == null) return;

      grabbedObject.OnReleased();
      grabbedObject = null;
    }

    #endregion

    #region Teleportation

    [Button]
    public void StartTeleport() // Should be assigned onHold
    {
      _attemptTeleport = true;
    }

    [Button]
    public void ConfirmTeleport() // Should be assigned onRelease
    {
      _attemptTeleport = false;
      if (_canTeleport)
      {
        hmd.transform.position = _teleportLocation;
        _canTeleport = false;
      }

      lineRenderer.enabled = false;
    }

    private void Teleportation()
    {
      if (hmd.teleportationEnabled && _attemptTeleport)
      {
        if (hmd.useBallisticTeleportation)
        {
          float decayRate = 0.025f;
          float stepDistance = 1f;
          int maxSteps = 128;
          int currentSteps = 0;
          bool rayHit = false;

          Vector3 startingPosition = _position;
          Vector3 pointingDirection = _transform.forward.normalized;
          Vector3 currentPosition = startingPosition;

          List<Vector3> positions = new List<Vector3> { startingPosition };
          lineRenderer.positionCount = 1;


          while (currentSteps <= maxSteps && currentPosition.y >= hmd.floorLevel && !rayHit)
          {
            Vector3 decayedVector = Mathf.Clamp01(decayRate * currentSteps) * Vector3.down +
                                    pointingDirection * Mathf.Clamp01(1 - decayRate * currentSteps);

            decayedVector = decayedVector.normalized * stepDistance;


            Ray raycast = new Ray(currentPosition, decayedVector);
            Physics.Raycast(raycast, out RaycastHit hitObject, stepDistance);

            rayHit = !(hitObject.transform is null);

            if (rayHit)
            {
              _teleportLocation = hitObject.point;
            }

            currentPosition += decayedVector;

            currentSteps++;

            positions.Add(currentPosition);

            lineRenderer.positionCount++;
            lineRenderer.SetPositions(positions.ToArray());
          }

          lineRenderer.enabled = true;

          if (rayHit)
          {
            lineRenderer.startColor = hmd.canTeleportColor;
            lineRenderer.endColor = hmd.canTeleportColor;
            _canTeleport = true;
          }
          else
          {
            lineRenderer.startColor = hmd.cannotTeleportColor;
            lineRenderer.endColor = hmd.cannotTeleportColor;
          }
        }
        else
        {
          Ray raycast = new Ray(_position, _transform.forward.normalized);
          bool rayHit = Physics.Raycast(raycast, out RaycastHit hitObject, float.MaxValue);

          if (rayHit)
          {
            lineRenderer.startColor = hmd.canTeleportColor;
            lineRenderer.endColor = hmd.canTeleportColor;
            lineRenderer.SetPositions(new Vector3[]
            {
              _position,
              hitObject.point
            });
            lineRenderer.enabled = true;
            _teleportLocation = hitObject.point;
            _canTeleport = true;
          }
          else
          {
            lineRenderer.startColor = hmd.cannotTeleportColor;
            lineRenderer.endColor = hmd.cannotTeleportColor;
            lineRenderer.SetPositions(new Vector3[]
            {
              _position, _transform.forward.normalized
            });
            lineRenderer.enabled = true;
          }
        }
      }
    }

    #endregion

    #region Handles

    private void HandleCalls()
    {
      if (hardwarePart == VrHardware.LeftController)
      {
        if (data.gripHold) hmd.leftGripHold?.Invoke();
        if (data.joystickHold) hmd.leftJoystickHold?.Invoke();
        if (data.triggerHold && _clickValue < data.triggerPressure) hmd.leftTriggerHold?.Invoke();
        if (data.axButtonHold) hmd.buttonXHold?.Invoke();
        if (data.byButtonHold) hmd.buttonYHold?.Invoke();

        if (!_onTriggerPressureDown && _clickValue < data.triggerPressure)
        {
          _onTriggerPressureDown = true;
          hmd.leftTriggerClick?.Invoke();
        }
        else if (_onTriggerPressureDown && _releaseValue > data.triggerPressure)
        {
          _onTriggerPressureDown = false;
          hmd.leftTriggerRelease?.Invoke();
        }

        if (!_onGripPressureDown && _clickValue < data.gripPressure)
        {
          _onTriggerPressureDown = true;
          hmd.leftGripClick?.Invoke();
        }
        else if (_onGripPressureDown && _releaseValue > data.gripPressure)
        {
          _onTriggerPressureDown = false;
          hmd.leftGripRelease?.Invoke();
        }

        if (_axReset && data.axButtonHold)
        {
          hmd.buttonXClick?.Invoke();
          _axReset = false;
        }
        else if (!data.axButtonHold)
        {
          if (!_axReset) hmd.buttonXRelease?.Invoke();
          _axReset = true;
        }
        
        if (_byReset && data.byButtonHold)
        {
          hmd.buttonYClick?.Invoke();
          _byReset = false;
        }
        else if (!data.byButtonHold)
        {
          if (!_byReset) hmd.buttonYRelease?.Invoke();
          _byReset = true;
        }
        
        if (_joystickReset && data.joystickHold)
        {
          hmd.leftJoystickClick?.Invoke();
          _byReset = false;
        }
        else if (!data.joystickHold)
        {
          if (!_joystickReset) hmd.leftJoystickRelease?.Invoke();
          _joystickReset = true;
        }

        hmd.leftJoystickAxis?.Invoke(data.joystickAxis);
      }
      else if (hardwarePart == VrHardware.RightController)
      {
        if (data.gripHold) hmd.rightGripHold?.Invoke();
        if (data.joystickHold) hmd.rightJoystickHold?.Invoke();
        if (data.triggerHold && _clickValue < data.triggerPressure) hmd.rightTriggerHold?.Invoke();
        if (data.axButtonHold) hmd.buttonAHold?.Invoke();
        if (data.byButtonHold) hmd.buttonBHold?.Invoke();

        if (!_onTriggerPressureDown && _clickValue < data.triggerPressure)
        {
          _onTriggerPressureDown = true;
          hmd.rightTriggerClick?.Invoke();
        }
        else if (_onTriggerPressureDown && _releaseValue > data.triggerPressure)
        {
          _onTriggerPressureDown = false;
          hmd.rightTriggerRelease?.Invoke();
        }

        if (!_onGripPressureDown && _clickValue < data.gripPressure)
        {
          _onTriggerPressureDown = true;
          hmd.rightGripClick?.Invoke();
        }
        else if (_onGripPressureDown && _releaseValue > data.gripPressure)
        {
          _onTriggerPressureDown = false;
          hmd.rightGripRelease?.Invoke();
        }
        
        if (_axReset && data.axButtonHold)
        {
          hmd.buttonXClick?.Invoke();
          _axReset = false;
        }
        else if (!data.axButtonHold)
        {
          if (!_axReset) hmd.buttonARelease?.Invoke();
          _axReset = true;
        }
        
        if (_byReset && data.byButtonHold)
        {
          hmd.buttonYClick?.Invoke();
          _byReset = false;
        }
        else if (!data.byButtonHold)
        {
          if (!_byReset) hmd.buttonBRelease?.Invoke();
          _byReset = true;
        }
        
        if (_joystickReset && data.joystickHold)
        {
          hmd.rightJoystickClick?.Invoke();
          _byReset = false;
        }
        else if (!data.joystickHold)
        {
          if (!_joystickReset) hmd.rightJoystickRelease?.Invoke();
          _joystickReset = true;
        }

        hmd.rightJoystickAxis?.Invoke(data.joystickAxis);
      }
    }

    #endregion

    #region DataHandling

    public void UpdateControllerData()
    {
      ControllerData controllerData = new ControllerData();

      device.TryGetFeatureValue(CommonUsages.deviceAcceleration, out controllerData.acceleration);
      device.TryGetFeatureValue(CommonUsages.devicePosition, out controllerData.position);
      device.TryGetFeatureValue(CommonUsages.deviceRotation, out controllerData.rotation);
      device.TryGetFeatureValue(CommonUsages.deviceVelocity, out controllerData.velocity);
      device.TryGetFeatureValue(CommonUsages.deviceAcceleration, out controllerData.acceleration);
      device.TryGetFeatureValue(CommonUsages.deviceAngularVelocity, out controllerData.angularVelocity);
      device.TryGetFeatureValue(CommonUsages.deviceAngularAcceleration, out controllerData.angularAcceleration);
      device.TryGetFeatureValue(CommonUsages.grip, out controllerData.gripPressure);
      device.TryGetFeatureValue(CommonUsages.gripButton, out controllerData.gripHold);
      device.TryGetFeatureValue(CommonUsages.trigger, out controllerData.triggerPressure);
      device.TryGetFeatureValue(CommonUsages.triggerButton, out controllerData.triggerHold);
      device.TryGetFeatureValue(CommonUsages.primaryButton, out controllerData.axButtonHold);
      device.TryGetFeatureValue(CommonUsages.secondaryButton, out controllerData.byButtonHold);
      device.TryGetFeatureValue(CommonUsages.primary2DAxis, out controllerData.joystickAxis);
      device.TryGetFeatureValue(CommonUsages.primary2DAxisClick, out controllerData.joystickHold);
      
      data = controllerData;
    }

    #endregion
  }
}