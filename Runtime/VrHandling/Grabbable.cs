using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace TawVR
{
  /// <summary>
  /// Attached to a gameObject to make it grabbable by the controllers
  /// </summary>
  [RequireComponent(typeof(Rigidbody), typeof(Collider))]
  public class Grabbable : MonoBehaviour
  {
    [Title("Settings")] public bool canBeGrabbed = true;
    public bool worldPositionStays = true;
    public bool returnToOriginalSpotOnRelease = false;

    public UnityEvent onGrabbed;
    public UnityEvent onReleased;

    [Title("Dynamic")] 
    [ReadOnly] public Controller grabbedBy;

    [Title("Privates")] 
    private Vector3 _originalPosition;
    private Quaternion _originalRotation;
    private Transform _originalParent;
    private bool _originalKinematicState;
    private bool _originalGravityState;

    [Title("Components")] 
    private Rigidbody _rigidbody;
    private Collider _collider;
    private Transform _transform;

    public bool IsGrabbed => grabbedBy != null;

    private void Start() // Save all components, these will come in handy later.
    {
      _rigidbody = GetComponent<Rigidbody>();
      _collider = GetComponent<Collider>();
      _transform = transform; // "transform" is actually a getter method. By doing this, we are saving resources.
    }

    /// <summary>
    /// Called when the item is picked up
    /// </summary>
    public void OnGrabbed(Controller grabber)
    {
      if (!canBeGrabbed) return; // If it can't be grabbed, we have nothing to do here

      if (IsGrabbed) // Logic for when the item is being held by other grabber -> transfer from one hand to another
      {
        grabbedBy.RegrabObject(grabber); // Changes the grabbing events to the second grabber
      }
      else
      {
        _originalPosition = _transform.position;
        _originalRotation = _transform.rotation;
        _originalParent = _transform.parent;
        _originalKinematicState = _rigidbody.isKinematic;
        _originalGravityState = _rigidbody.useGravity;
      }


      grabbedBy = grabber;
      _transform.SetParent(grabbedBy.transform, worldPositionStays);
      _rigidbody.useGravity = false;
      _rigidbody.isKinematic = true;
      
      onGrabbed?.Invoke(); // if user supplied another method to be executed, execute it
    }

    /// <summary>
    /// Called when the item is dropped again; not called on Re-grab
    /// </summary>
    public void OnReleased()
    {
      _rigidbody.useGravity = _originalGravityState;
           _rigidbody.isKinematic = _originalKinematicState;
           _transform.SetParent(_originalParent, worldPositionStays);
      // if we are not to return the item to the original position, it should just drop down
      // or stay floating, if it was set that way
      if (returnToOriginalSpotOnRelease) 
      {
        _transform.position = _originalPosition;
        _transform.rotation = _originalRotation;
      }
      else
      {
        _rigidbody.velocity = grabbedBy.data.velocity;
        _rigidbody.angularVelocity = grabbedBy.data.angularVelocity;
      }

      onReleased?.Invoke();
    }
  }
}