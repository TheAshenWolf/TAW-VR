using System;
using Sirenix.OdinInspector;
using TawVR.Runtime.VrHandling;
using UnityEngine;

namespace TawVR.Runtime.Core.VrHandling
{
  [DisallowMultipleComponent]
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
        _startingPoint = Vector3.Normalize(controller.data.position - _rotationPoint);
        _difference = _startingRotation.eulerAngles - _startingPoint;
      }
      Vector3 newPoint = Vector3.Normalize(controller.data.position - _rotationPoint);
      Quaternion newRotation = Quaternion.Euler(_difference + newPoint);

      _transform.rotation = newRotation;
    }

    public void FinishRotation()
    {
      _isRotating = false;
    }
  }
}