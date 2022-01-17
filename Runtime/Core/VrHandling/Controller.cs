using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TAW_VR.Runtime.Core.GameObjectScripts;
using TAW_VR.Runtime.Core.VrHandling;
using TawVR.Runtime.Core.VrHandling;
using UnityEngine;
using UnityEngine.XR;

namespace TawVR.Runtime.VrHandling
{
  public abstract class Controller : MonoBehaviour
  {
    [Title("Components")] [Required] public VrRig hmd;
    public LineRenderer lineRenderer;
    public Collider triggerCollider;
    public Transform pointerHead;

    [Title("Information")] [ReadOnly] public Grabbable grabbedObject;
    [ReadOnly] public Rotateable rotatedObject;
    [ReadOnly] public Scaleable scaledObject;
    [ReadOnly] public ControllerData data;

    [HideInInspector] public VrHardware hardwarePart;
    [HideInInspector] public bool initialized;
    [HideInInspector] public Vector3 pointerPosition => pointerHead.position;

    private InputDevice _device;
    private Transform _transform;
    private Vector3 _teleportLocation;
    private Vector3 _position;
    private bool _canTeleport;
    private bool _attemptTeleport;
    private bool _onTriggerPressureDown;
    private bool _onGripPressureDown;
    private bool _joystickReset;
    private bool _axReset;
    private bool _byReset;

    private float _clickValue;
    private float _releaseValue;

    private Collider _proximityGrabbableItem;
    private Collider _proximityRotateableItem;
    private Collider _proximityScaleableItem;

    private bool _disposeRotateable;
    private bool _disposeScaleable;
    private bool _disposeGrabbable;

    public void Init(InputDevice inputDevice, VrHardware hwPart)
    {
      _device = inputDevice;
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

    public void SendHaptic(float duration = .5f)
    {
      _device.SendHapticImpulse(0, .5f, duration);
    }

    #region Grabber

    private void OnTriggerEnter(Collider other)
    {
      if (scaledObject == null && grabbedObject == null && rotatedObject == null) SendHaptic(.2f);
      if (other.GetComponent<Grabbable>() != null) _proximityGrabbableItem = other;
      if (other.GetComponent<Rotateable>() != null) _proximityRotateableItem = other;
      if (other.GetComponent<Scaleable>() != null) _proximityScaleableItem = other;
    }

    private void OnCollisionEnter(Collision collision)
    {
      if (scaledObject == null && grabbedObject == null && rotatedObject == null) SendHaptic(.2f);
      if (collision.collider.GetComponent<Grabbable>() != null) _proximityGrabbableItem = collision.collider;
      if (collision.collider.GetComponent<Rotateable>() != null) _proximityRotateableItem = collision.collider;
      if (collision.collider.GetComponent<Scaleable>() != null) _proximityScaleableItem = collision.collider;
    }

    private void OnTriggerExit(Collider other)
    {
      if (_proximityGrabbableItem == other)
      {
        if (grabbedObject != null) _disposeGrabbable = true;
        else _proximityGrabbableItem = null;
      }

      if (_proximityRotateableItem == other)
      {
        if (rotatedObject != null) _disposeRotateable = true;
        else _proximityRotateableItem = null;
      }

      if (_proximityScaleableItem == other)
      {
        if (scaledObject != null) _disposeScaleable = true;
        else _proximityScaleableItem = null;
      }
    }

    private void OnCollisionExit(Collision collision)
    {
      if (_proximityGrabbableItem == collision.collider)
      {
        if (grabbedObject != null) _disposeGrabbable = true;
        else _proximityGrabbableItem = null;
      }

      if (_proximityRotateableItem == collision.collider)
      {
        if (rotatedObject != null) _disposeRotateable = true;
        else _proximityRotateableItem = null;
      }

      if (_proximityScaleableItem == collision.collider)
      {
        if (scaledObject != null) _disposeScaleable = true;
        else _proximityScaleableItem = null;
      }
    }

    public void GrabObject() // Attempts to grab the item that is being interacted with
    {
      if (_proximityGrabbableItem == null) return;

      if (grabbedObject == null) grabbedObject = _proximityGrabbableItem.GetComponent<Grabbable>();
      grabbedObject.OnGrabbed(this);
    }

    public void ReleaseObject() // Drops the held item
    {
      if (grabbedObject == null) return;

      grabbedObject.OnReleased();
      grabbedObject = null;
      if (_disposeGrabbable) _proximityGrabbableItem = null;
    }

    public void RotateObject()
    {
      if (_proximityRotateableItem == null) return;
      if (rotatedObject == null) rotatedObject = _proximityRotateableItem.GetComponent<Rotateable>();

      rotatedObject.Rotate(this);
    }

    public void StopRotatingObject()
    {
      if (rotatedObject == null) return;
      rotatedObject.FinishRotation();
      rotatedObject = null;

      if (_disposeRotateable) _proximityRotateableItem = null;
    }

    public void ScaleObject()
    {
      if (_proximityScaleableItem == null) return;
      if (scaledObject == null) scaledObject = _proximityScaleableItem.GetComponent<Scaleable>();

      scaledObject.Scale(this);
    }

    public void StopScalingObject()
    {
      if (scaledObject == null) return;
      scaledObject.FinishScaling();
      scaledObject = null;

      if (_disposeScaleable) _proximityScaleableItem = null;
    }

    #endregion

    #region Teleportation

    public void StartTeleport() // Should be assigned onHold
    {
      _attemptTeleport = true;
    }

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
          int currentSteps = 0;
          bool rayHit = false;

          Vector3 startingPosition = _position;
          Vector3 pointingDirection = _transform.forward.normalized;
          Vector3 currentPosition = startingPosition;

          List<Vector3> positions = new List<Vector3> { startingPosition };
          lineRenderer.positionCount = 1;


          while (currentSteps <= hmd.ballisticMaxSteps && currentPosition.y >= hmd.floorLevel && !rayHit)
          {
            Vector3 decayedVector = Mathf.Clamp01(hmd.ballisticDecayRate * currentSteps) * Vector3.down +
                                    pointingDirection * Mathf.Clamp01(1 - hmd.ballisticDecayRate * currentSteps);

            decayedVector = decayedVector.normalized * hmd.ballisticStepDistance;


            Ray raycast = new Ray(currentPosition, decayedVector);
            Physics.Raycast(raycast, out RaycastHit hitObject, hmd.ballisticStepDistance);

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
          _onGripPressureDown = true;
          hmd.leftGripClick?.Invoke();
        }
        else if (_onGripPressureDown && _releaseValue > data.gripPressure)
        {
          _onGripPressureDown = false;
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

      _device.TryGetFeatureValue(CommonUsages.deviceAcceleration, out controllerData.acceleration);
      _device.TryGetFeatureValue(CommonUsages.devicePosition, out controllerData.position);
      _device.TryGetFeatureValue(CommonUsages.deviceRotation, out controllerData.rotation);
      _device.TryGetFeatureValue(CommonUsages.deviceVelocity, out controllerData.velocity);
      _device.TryGetFeatureValue(CommonUsages.deviceAcceleration, out controllerData.acceleration);
      _device.TryGetFeatureValue(CommonUsages.deviceAngularVelocity, out controllerData.angularVelocity);
      _device.TryGetFeatureValue(CommonUsages.deviceAngularAcceleration, out controllerData.angularAcceleration);
      _device.TryGetFeatureValue(CommonUsages.grip, out controllerData.gripPressure);
      _device.TryGetFeatureValue(CommonUsages.gripButton, out controllerData.gripHold);
      _device.TryGetFeatureValue(CommonUsages.trigger, out controllerData.triggerPressure);
      _device.TryGetFeatureValue(CommonUsages.triggerButton, out controllerData.triggerHold);
      _device.TryGetFeatureValue(CommonUsages.primaryButton, out controllerData.axButtonHold);
      _device.TryGetFeatureValue(CommonUsages.secondaryButton, out controllerData.byButtonHold);
      _device.TryGetFeatureValue(CommonUsages.primary2DAxis, out controllerData.joystickAxis);
      _device.TryGetFeatureValue(CommonUsages.primary2DAxisClick, out controllerData.joystickHold);

      data = controllerData;
    }

    #endregion
  }
}