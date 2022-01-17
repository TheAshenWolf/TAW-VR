using Sirenix.OdinInspector;
using TawVR.Runtime.VrHandling;
using UnityEngine;

namespace TAW_VR.Runtime.Core.GameObjectScripts
{
  [DisallowMultipleComponent, RequireComponent(typeof(Collider), typeof(Rigidbody))]
  public class Scaleable : MonoBehaviour
  {
    [Title("Settings")] 
    public bool canBeScaled = true;

    [Title("Details")] 
    public Controller scalingWith;

    [Title("Privates")] 
    private bool _isScaling;
    private Transform _transform;
    private Vector3 _startingPoint;
    private float _baseVelocity;
    private Vector3 _originalScale;

    private void Start()
    {
      _transform = transform;
    }

    public void Scale(Controller controller)
    {
      if (!canBeScaled) return;

      if (!_isScaling)
      {
        _baseVelocity = Vector3.Distance(controller.pointerPosition, _transform.position);
        _originalScale = _transform.localScale;
        scalingWith = controller;
        _isScaling = true;
      }

      float currentVelocity = Vector3.Distance(controller.pointerPosition, _transform.position);
      _transform.localScale = _originalScale * (currentVelocity / _baseVelocity);
    }

    public void FinishScaling()
    {
      _isScaling = false;
      scalingWith = null;
    }
  }
}