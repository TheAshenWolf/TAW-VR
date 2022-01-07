using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.XR;

namespace TawVR
{
  public class Controller : MonoBehaviour
  {
    public LineRenderer lineRenderer;
    public InputDevice device;
    public ControllerData data;
    [Required] public VrRig hmd;
    private Transform _transform;
    private bool _teleportReady;
    private bool _canTeleport;
    private bool _attemptTeleport;
    private Vector3 _teleportLocation;
    private Vector3 _position;

    public void Init(InputDevice inputDevice)
    {
      device = inputDevice;
      data = new ControllerData();
    }

    private void Start()
    {
      _transform = transform;
      if (triggerCollider == null) throw new Exception("Trigger collider was not assigned.");
    }

    private void Update()
    {
      Teleportation();
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
      Grabbable grabbable = other.GetComponent<Grabbable>();
      if (grabbable == null) return;

      _proximityItem = other;
    }

    private void OnTriggerExit(Collider other)
    {
      if (_proximityItem == other) _proximityItem = null;
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
        _position = _transform.position;
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

    #endregion
  }
}