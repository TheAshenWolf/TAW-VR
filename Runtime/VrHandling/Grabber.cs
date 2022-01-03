using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace TawVR
{
  public class Grabber : MonoBehaviour
  {
    [Title("Components")] 
    public Collider triggerCollider;

    [Title("Privates")] 
    private Grabbable _grabbedObject;
    private Collider _proximityItem;
    
    private void Start()
    {
      if (triggerCollider == null) throw new Exception("Trigger collider was not assigned.");
    }

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

    public void Grab() // Attempts to grab the item that is being interacted with
    {
      if (_proximityItem == null) return;
      
      _grabbedObject = _proximityItem.GetComponent<Grabbable>();
      _grabbedObject.OnGrabbed(this);
    }

    public void Regrab(Grabber grabber) // Transfers grabbed item from one hand to another
    {
      if (grabber == this) return;
    }

    public void Release() // Drops the held item
    {
      if (_grabbedObject == null) return;

      _grabbedObject.OnReleased();
      _grabbedObject = null;
    }
  }
}