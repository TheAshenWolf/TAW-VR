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
    private bool _initialized;

    public void Init(InputDevice inputDevice, VrHardware hwPart)
    {
      device = inputDevice;
      data = new ControllerData();
      hardwarePart = hwPart;

      _clickValue = hmd.clickValue;
      _releaseValue = hmd.releaseValue;
      _initialized = true;
    }

    private void Start()
    {
      _transform = transform;
      if (triggerCollider == null) throw new Exception("Trigger collider was not assigned.");
    }

    private void Update()
    {
      if (!_initialized) return;
      
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

    private Grabbable _grabbedObject;
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

      _grabbedObject = _proximityItem.GetComponent<Grabbable>();
      _grabbedObject.OnGrabbed(this);
    }

    public void RegrabObject(Controller grabber) // Transfers grabbed item from one hand to another
    {
      if (grabber == this) return;
    }

    public void ReleaseObject() // Drops the held item
    {
      if (_grabbedObject == null) return;

      _grabbedObject.OnReleased();
      _grabbedObject = null;
    }

    #endregion

    #region Teleportation

    [Button]
    public void ToggleTeleport() // For use with a button without release handler
    {
      if (!_attemptTeleport)
      {
        StartTeleport();
      }
      else
      {
        ConfirmTeleport();
      }
    }

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
        if (data.gripClick) hmd.leftGripClick?.Invoke();
        if (data.joystickClick) hmd.leftJoystickClick?.Invoke();
        if (data.triggerClick) hmd.leftTriggerClick?.Invoke();
        if (data.axButtonClick) hmd.buttonXClick?.Invoke();
        if (data.byButtonClick) hmd.buttonYClick?.Invoke();

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

        if (_clickValue < data.triggerPressure) hmd.leftTriggerHold?.Invoke();
        if (_clickValue < data.gripPressure) hmd.leftGripHold?.Invoke();

        hmd.leftJoystickAxis?.Invoke(data.joystickAxis);
      }
      else if (hardwarePart == VrHardware.RightController)
      {
        if (data.gripClick) hmd.rightGripClick?.Invoke();
        if (data.joystickClick) hmd.rightJoystickClick?.Invoke();
        if (data.triggerClick) hmd.rightTriggerClick?.Invoke();
        if (data.axButtonClick) hmd.buttonAClick?.Invoke();
        if (data.byButtonClick) hmd.buttonBClick?.Invoke();

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

        if (_clickValue < data.triggerPressure) hmd.rightTriggerHold?.Invoke();
        if (_clickValue < data.gripPressure) hmd.rightGripHold?.Invoke();

        hmd.rightJoystickAxis?.Invoke(data.joystickAxis);
      }
    }

    #endregion
  }
}