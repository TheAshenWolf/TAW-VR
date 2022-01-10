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
    private InputDevice _cameraDevice;

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
      _inputDevices = new List<InputDevice>();
      InputDevices.GetDevices(_inputDevices);

        foreach (InputDevice inputDevice in _inputDevices)
      {
        if ((inputDevice.characteristics & InputDeviceCharacteristics.Left) != 0)
        {
          leftController.Init(inputDevice, VrHardware.LeftController);
        }

        if ((inputDevice.characteristics & InputDeviceCharacteristics.Right) != 0)
        {
          rightController.Init(inputDevice, VrHardware.RightController);
        }

        if ((inputDevice.characteristics & InputDeviceCharacteristics.Camera) != 0)
        {
          _cameraDevice = inputDevice;
        }
      }
    }

    private void Update()
    {
      GetControllerData();
      SetCameraHeight();
    }

    private void SetCameraHeight()
    {
      _cameraDevice.TryGetFeatureValue(CommonUsages.devicePosition, out Vector3 cameraPosition);
      mainCamera.transform.localPosition = cameraPosition;
    }

    private void GetControllerData()
    {
      if (leftController != null)
      {
        ControllerData controllerData = new ControllerData();

        leftController.device.TryGetFeatureValue(CommonUsages.deviceAcceleration, out controllerData.acceleration);
        leftController.device.TryGetFeatureValue(CommonUsages.devicePosition, out controllerData.position);
        leftController.device.TryGetFeatureValue(CommonUsages.deviceRotation, out controllerData.rotation);
        leftController.device.TryGetFeatureValue(CommonUsages.deviceVelocity, out controllerData.velocity);
        leftController.device.TryGetFeatureValue(CommonUsages.deviceAcceleration, out controllerData.acceleration);
        leftController.device.TryGetFeatureValue(CommonUsages.deviceAngularVelocity,
          out controllerData.angularVelocity);
        leftController.device.TryGetFeatureValue(CommonUsages.deviceAngularAcceleration,
          out controllerData.angularAcceleration);

        leftController.device.TryGetFeatureValue(CommonUsages.grip, out controllerData.gripPressure);
        leftController.device.TryGetFeatureValue(CommonUsages.gripButton, out controllerData.gripClick);
        leftController.device.TryGetFeatureValue(CommonUsages.trigger, out controllerData.triggerPressure);
        leftController.device.TryGetFeatureValue(CommonUsages.triggerButton, out controllerData.triggerClick);
        leftController.device.TryGetFeatureValue(CommonUsages.primaryButton, out controllerData.axButtonClick);
        leftController.device.TryGetFeatureValue(CommonUsages.secondaryButton, out controllerData.byButtonClick);
        leftController.device.TryGetFeatureValue(CommonUsages.primary2DAxis, out controllerData.joystickAxis);
        leftController.device.TryGetFeatureValue(CommonUsages.primary2DAxisClick, out controllerData.joystickClick);

        leftController.data = controllerData;
      }

      if (rightController != null)
      {
        ControllerData controllerData = new ControllerData();

        rightController.device.TryGetFeatureValue(CommonUsages.deviceAcceleration, out controllerData.acceleration);
        rightController.device.TryGetFeatureValue(CommonUsages.devicePosition, out controllerData.position);
        rightController.device.TryGetFeatureValue(CommonUsages.deviceRotation, out controllerData.rotation);
        rightController.device.TryGetFeatureValue(CommonUsages.deviceVelocity, out controllerData.velocity);
        rightController.device.TryGetFeatureValue(CommonUsages.deviceAcceleration, out controllerData.acceleration);
        rightController.device.TryGetFeatureValue(CommonUsages.deviceAngularVelocity,
          out controllerData.angularVelocity);
        rightController.device.TryGetFeatureValue(CommonUsages.deviceAngularAcceleration,
          out controllerData.angularAcceleration);

        rightController.device.TryGetFeatureValue(CommonUsages.grip, out controllerData.gripPressure);
        rightController.device.TryGetFeatureValue(CommonUsages.gripButton, out controllerData.gripClick);
        rightController.device.TryGetFeatureValue(CommonUsages.trigger, out controllerData.triggerPressure);
        rightController.device.TryGetFeatureValue(CommonUsages.triggerButton, out controllerData.triggerClick);
        rightController.device.TryGetFeatureValue(CommonUsages.primaryButton, out controllerData.axButtonClick);
        rightController.device.TryGetFeatureValue(CommonUsages.secondaryButton, out controllerData.byButtonClick);
        rightController.device.TryGetFeatureValue(CommonUsages.primary2DAxis, out controllerData.joystickAxis);
        rightController.device.TryGetFeatureValue(CommonUsages.primary2DAxisClick, out controllerData.joystickClick);

        rightController.data = controllerData;
      }
    }

    public void HorizontalMovement(Vector2 input)
    {
      if (!horizontalMovementEnabled) return;

      Transform mainCameraTransform = mainCamera.transform;

      Vector3 forward = mainCameraTransform.forward;
      forward.y = 0f;
      forward.Normalize();

      Vector3 right = mainCameraTransform.right;
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