using Sirenix.OdinInspector;
using TawVR.Runtime.VrHandling;
using UnityEngine;

namespace TAW_VR.Runtime.Core.GameObjectScripts
{
  [DisallowMultipleComponent, RequireComponent(typeof(Collider), typeof(Rigidbody))]
  public class Scaleable : MonoBehaviour
  {
    [Title("Settings")] 
    public bool canBeScaled;

    [Title("Details")] 
    public Controller scalingWith;

    [Title("Privates")] 
    private bool _isScaling;
    private Transform _transform;
    private Vector3 _startingPoint;
    private float _baseVelocity;

    private void Start()
    {
      _transform = transform;
    }

    public void Scale(Controller controller)
    {
      if (!canBeScaled) return;

      if (!_isScaling)
      {
        _baseVelocity = Vector3.Distance(controller.transform.position, _transform.position);
        scalingWith = controller;
      }

      float currentVelocity = Vector3.Distance(controller.transform.position, _transform.position);
      _transform.localScale = Vector3.one * (currentVelocity / _baseVelocity);
    }

    public void FinishScaling()
    {
      _isScaling = false;
      scalingWith = null;
    }
  }
}