using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR;

namespace TawVR
{
  public class VrRig : MonoBehaviour
  {
    [Title("Components")]
    [BoxGroup, Required] public Camera mainCamera;
    [BoxGroup] public Controller leftController;
    [BoxGroup] public Controller rightController;

    [Title("General Settings")]
    [BoxGroup, Tooltip("Checks for stairs, steep hills and colliders, doesnt allow passage thru walls and floors.")]
    public bool advancedColliderDetection;

    [ShowIf(nameof(advancedColliderDetection)), BoxGroup("ACD"), Tooltip("Radius around the main position to consider as body.")]
    public float colliderDifferenceThreshold = 0.125f;

    [ShowIf(nameof(advancedColliderDetection)), BoxGroup("ACD"), Tooltip("The bottom-most floor. Doesn't allow movement past this point. Set to -1 to disable.")]
    public float floorLevel;

    [BoxGroup, Tooltip("Pointer color when user can teleport"), ColorUsage(true, false)]
    public Color canTeleportColor;

    [BoxGroup, Tooltip("Pointer color when user can NOT teleport"), ColorUsage(true, false)]
    public Color cannotTeleportColor;

    [BoxGroup, Tooltip("Speed used for movement")]
    public float movementSpeed;

    [BoxGroup, Range(0, 1)] 
    public float releaseValue = 0.2f;
    [BoxGroup, Range(0, 1)] 
    public float clickValue = 0.8f;
    

    // -----
    [Title("Controls"), BoxGroup, Tooltip("If false, user can not teleport.")]
    public bool teleportationEnabled;

    [ShowIf(nameof(teleportationEnabled)), BoxGroup, Tooltip("true - uses a ballistic calculation to get the final position of the teleport; false - uses a straight line")]
    public bool useBallisticTeleportation;

    [BoxGroup, Tooltip("If false, user can not move on the horizontal plane.")]
    public bool horizontalMovementEnabled;

    [BoxGroup, Tooltip("If false, user can not move along the vertical axis.")]
    public bool verticalMovementEnabled;
    
    [Title("Privates")] 
    private List<InputDevice> _inputDevices = new List<InputDevice>();
    private Controller _leftController = null;
    private Controller _rightController = null;

    [Title("Calls")]
    [HideInInspector] public UnityVector2Event leftJoystickAxis;
    [HideInInspector] public UnityEvent leftJoystickClick;
    [HideInInspector] public UnityEvent buttonXClick;
    [HideInInspector] public UnityEvent buttonYClick;
    [HideInInspector] public UnityEvent leftTriggerClick;
    [HideInInspector] public UnityEvent leftTriggerRelease;
    [HideInInspector] public UnityEvent leftTriggerHold;
    [HideInInspector] public UnityFloatEvent leftTriggerPressure;
    [HideInInspector] public UnityEvent leftGripClick;
    [HideInInspector] public UnityEvent leftGripRelease;
    [HideInInspector] public UnityEvent leftGripHold;
    [HideInInspector] public UnityFloatEvent leftGripPressure;
    
    [HideInInspector] public UnityVector2Event rightJoystickAxis;
    [HideInInspector] public UnityEvent rightJoystickClick;
    [HideInInspector] public UnityEvent buttonAClick;
    [HideInInspector] public UnityEvent buttonBClick;
    [HideInInspector] public UnityEvent rightTriggerClick;
    [HideInInspector] public UnityEvent rightTriggerRelease;
    [HideInInspector] public UnityEvent rightTriggerHold;
    [HideInInspector] public UnityFloatEvent rightTriggerPressure;
    [HideInInspector] public UnityEvent rightGripClick;
    [HideInInspector] public UnityEvent rightGripRelease;
    [HideInInspector] public UnityEvent rightGripHold;
    [HideInInspector] public UnityFloatEvent rightGripPressure;
    

    private void Awake()
    {
      UpdateDevices();
    }

    [Button]
    public void UpdateDevices()
    {
      InputDevices.GetDevices(_inputDevices);
      if (_inputDevices.Count == 0)
      {
        _inputDevices.Add(new InputDevice());
      }

      foreach (InputDevice inputDevice in _inputDevices)
      {
        if ((inputDevice.characteristics & InputDeviceCharacteristics.Left) != 0)
        {
          _leftController.Init(inputDevice, VrHardware.LeftController);
        }

        if ((inputDevice.characteristics & InputDeviceCharacteristics.Right) != 0)
        {
          _rightController.Init(inputDevice, VrHardware.RightController);
        }
      }
    }

    private void Update()
    {
      GetControllerData();
    }

    private void GetControllerData()
    {
      if (_leftController != null)
      {
        ControllerData controllerData = new ControllerData();

        _leftController.device.TryGetFeatureValue(CommonUsages.deviceAcceleration, out controllerData.acceleration);
        _leftController.device.TryGetFeatureValue(CommonUsages.devicePosition, out controllerData.position);
        _leftController.device.TryGetFeatureValue(CommonUsages.deviceRotation, out controllerData.rotation);
        _leftController.device.TryGetFeatureValue(CommonUsages.deviceVelocity, out controllerData.velocity);
        _leftController.device.TryGetFeatureValue(CommonUsages.deviceAcceleration, out controllerData.acceleration);
        _leftController.device.TryGetFeatureValue(CommonUsages.deviceAngularVelocity,
          out controllerData.angularVelocity);
        _leftController.device.TryGetFeatureValue(CommonUsages.deviceAngularAcceleration,
          out controllerData.angularAcceleration);

        _leftController.device.TryGetFeatureValue(CommonUsages.grip, out controllerData.gripPressure);
        _leftController.device.TryGetFeatureValue(CommonUsages.gripButton, out controllerData.gripClick);
        _leftController.device.TryGetFeatureValue(CommonUsages.trigger, out controllerData.triggerPressure);
        _leftController.device.TryGetFeatureValue(CommonUsages.triggerButton, out controllerData.triggerClick);
        _leftController.device.TryGetFeatureValue(CommonUsages.primaryButton, out controllerData.axButtonClick);
        _leftController.device.TryGetFeatureValue(CommonUsages.secondaryButton, out controllerData.byButtonClick);
        _leftController.device.TryGetFeatureValue(CommonUsages.primary2DAxis, out controllerData.joystickAxis);
        _leftController.device.TryGetFeatureValue(CommonUsages.primary2DAxisClick, out controllerData.joystickClick);

        _leftController.data = controllerData;
      }

      if (_rightController != null)
      {
        ControllerData controllerData = new ControllerData();

        _rightController.device.TryGetFeatureValue(CommonUsages.deviceAcceleration, out controllerData.acceleration);
        _rightController.device.TryGetFeatureValue(CommonUsages.devicePosition, out controllerData.position);
        _rightController.device.TryGetFeatureValue(CommonUsages.deviceRotation, out controllerData.rotation);
        _rightController.device.TryGetFeatureValue(CommonUsages.deviceVelocity, out controllerData.velocity);
        _rightController.device.TryGetFeatureValue(CommonUsages.deviceAcceleration, out controllerData.acceleration);
        _rightController.device.TryGetFeatureValue(CommonUsages.deviceAngularVelocity,
          out controllerData.angularVelocity);
        _rightController.device.TryGetFeatureValue(CommonUsages.deviceAngularAcceleration,
          out controllerData.angularAcceleration);

        _rightController.device.TryGetFeatureValue(CommonUsages.grip, out controllerData.gripPressure);
        _rightController.device.TryGetFeatureValue(CommonUsages.gripButton, out controllerData.gripClick);
        _rightController.device.TryGetFeatureValue(CommonUsages.trigger, out controllerData.triggerPressure);
        _rightController.device.TryGetFeatureValue(CommonUsages.triggerButton, out controllerData.triggerClick);
        _rightController.device.TryGetFeatureValue(CommonUsages.primaryButton, out controllerData.axButtonClick);
        _rightController.device.TryGetFeatureValue(CommonUsages.secondaryButton, out controllerData.byButtonClick);
        _rightController.device.TryGetFeatureValue(CommonUsages.primary2DAxis, out controllerData.joystickAxis);
        _rightController.device.TryGetFeatureValue(CommonUsages.primary2DAxisClick, out controllerData.joystickClick);

        _rightController.data = controllerData;
      }
    }

    public void HorizontalMovement(Vector2 input)
    {
      if (!horizontalMovementEnabled) return;

      Transform mainCameraTransform = mainCamera.transform;

      Vector3 forward = mainCameraTransform.InverseTransformPoint(mainCameraTransform.forward);
      forward.y = 0f;
      forward.Normalize();

      Vector3 right = mainCameraTransform.InverseTransformPoint(mainCameraTransform.right);
      right.y = 0f;
      right.Normalize();

      Vector3 coordinates = right * input.x + forward * input.y;

      if (advancedColliderDetection)
      {
        Vector3 cameraPosition = mainCameraTransform.transform.position;

        // walls
        Ray raycast = new Ray(cameraPosition, coordinates);
        if (Physics.SphereCast(raycast, .1f, .5f))
        {
          return;
        }

        // slanted surfaces / stairs
        Vector3 movementPoint = new Vector3(coordinates.x, transform.position.y, coordinates.z);
        Vector3 onTheGround = new Vector3(movementPoint.x / 2f, movementPoint.y - cameraPosition.y,
          movementPoint.z / 2f);

        float cameraHeight = mainCameraTransform.transform.localPosition.y;

        Ray surfaceRay = new Ray(cameraPosition, onTheGround);

        Physics.SphereCast(surfaceRay, .1f, out RaycastHit hit);

        float difference = cameraHeight - hit.distance;

        if (Math.Abs(difference) >= colliderDifferenceThreshold && hit.point != Vector3.zero)
        {
          coordinates = new Vector3(coordinates.x, coordinates.y + difference, coordinates.z);
        }
      }

      transform.Translate(coordinates * movementSpeed * Time.deltaTime);
    }

    public void VerticalMovement(Vector2 input)
    {
      Transform mainCameraTransform = mainCamera.transform;

      if (!verticalMovementEnabled) return;
      Vector3 coordinates = Vector3.up * input.y;

      if (advancedColliderDetection)
      {
        float cameraHeight = mainCameraTransform.transform.localPosition.y;

        // floor
        if (Physics.SphereCast(new Ray(mainCameraTransform.position, -mainCameraTransform.up), .1f,
              out RaycastHit hitDown) && input.y < 0)
        {
          if (hitDown.distance < cameraHeight) return;
        }
        // ceiling
        else if (Physics.SphereCast(new Ray(mainCameraTransform.position, mainCameraTransform.up), .1f,
                   out RaycastHit hitUp) && input.y < 0)
        {
          if (hitUp.distance < .1f) return;
        }
      }

      if (mainCamera.transform.position.y > (floorLevel + .5f) || input.y > 0)
      {
        transform.Translate(coordinates * movementSpeed * Time.deltaTime);
      }
    }
  }
}