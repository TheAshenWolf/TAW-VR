using Sirenix.OdinInspector;
using TawVR.Runtime.VrHandling;
using UnityEngine;

namespace TAW_VR.Runtime.Core.VrHandling
{
  [DisallowMultipleComponent, RequireComponent(typeof(Collider)), RequireComponent(typeof(Rigidbody))]
  public class Rotateable : MonoBehaviour
  {
    [Title("Settings")] 
    public bool canBeRotated;

    public bool rotateAlongX;
    public bool rotateAlongY;
    public bool rotateAlongZ;

    [Title("Details")] 
    public Controller rotatingWith;

    [Title("Privates")] 
    private bool _isRotating;
    private Quaternion _startingRotation;
    private Vector3 _startingPoint;
    private Vector3 _rotationPoint;
    private Transform _transform;
    private Vector3 _difference;

    private void Start()
    {
      _transform = transform;
    }

    public void Rotate(Controller controller)
    {
      if (!_isRotating)
      {
        _isRotating = true;
        _rotationPoint = _transform.position;
        _startingRotation = _transform.rotation;
        _startingPoint = controller.transform.position - _rotationPoint;
        _difference = _startingRotation.eulerAngles - _startingPoint;
        rotatingWith = controller;
      }
      Vector3 newPoint = controller.transform.position - _rotationPoint;
      Debug.LogError(newPoint);
      Quaternion newRotation = Quaternion.Euler(_difference + newPoint);
      Debug.LogError(newRotation);

      _transform.rotation = newRotation;
    }

    public void FinishRotation()
    {
      _isRotating = false;
      rotatingWith = null;
    }
  }
}