using Sirenix.OdinInspector;
using TawVR.Runtime.VrHandling;
using UnityEngine;

namespace TAW_VR.Runtime.Core.VrHandling
{
  [DisallowMultipleComponent, RequireComponent(typeof(Collider)), RequireComponent(typeof(Rigidbody))]
  public class Rotateable : MonoBehaviour
  {
    [Title("Settings")] public bool canBeRotated = true;

    [Title("Details")] public Controller rotatingWith;

    [Title("Privates")] private bool _isRotating;
    private Transform _transform;
    private Vector3 _startingPoint;
    private Quaternion _currentRotation;


    private void Start()
    {
      _transform = transform;
    }

    public void Rotate(Controller controller)
    {
      if (!canBeRotated) return;

      if (!_isRotating)
      {
        _startingPoint = Vector3.Normalize(controller.pointerPosition - _transform.position);
        _currentRotation = _transform.rotation;
        rotatingWith = controller;
        _isRotating = true;
      }

      Vector3 closestPoint = Vector3.Normalize(controller.pointerPosition - _transform.position);
      _transform.rotation = Quaternion.FromToRotation(_startingPoint, closestPoint) * _currentRotation;
    }

    public void FinishRotation()
    {
      _isRotating = false;
      rotatingWith = null;
    }
  }
}