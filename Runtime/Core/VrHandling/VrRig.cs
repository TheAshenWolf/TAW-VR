using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TawVR.Runtime.VrHandling;
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

    [ShowIf(nameof(advancedColliderDetection)), BoxGroup("ACD"), Tooltip("The bottom-most floor. Doesn't allow movement past this point. Set to NaN to disable")]
    public float floorLevel = float.NaN;

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

    [ShowIf(nameof(useBallisticTeleportation)), BoxGroup, Range(0.01f, 0.99f)] public float ballisticDecayRate = 0.025f;
    [ShowIf(nameof(useBallisticTeleportation)), BoxGroup] public float ballisticStepDistance = 1f;
    [ShowIf(nameof(useBallisticTeleportation)), BoxGroup] public int ballisticMaxSteps = 128;

    [BoxGroup, Tooltip("If false, user can not move on the horizontal plane.")]
    public bool horizontalMovementEnabled;

    [BoxGroup, Tooltip("If false, user can not move along the vertical axis.")]
    public bool verticalMovementEnabled;
    
    [Title("Privates")] 
    private List<InputDevice> _inputDevices = new List<InputDevice>();

    [Title("Calls")]
    [HideInInspector] public UnityVector2Event leftJoystickAxis;
    [HideInInspector] public UnityEvent leftJoystickClick;
    [HideInInspector] public UnityEvent leftJoystickHold;
    [HideInInspector] public UnityEvent leftJoystickRelease;
    [HideInInspector] public UnityEvent buttonXClick;
    [HideInInspector] public UnityEvent buttonXHold;
    [HideInInspector] public UnityEvent buttonXRelease;
    [HideInInspector] public UnityEvent buttonYClick;
    [HideInInspector] public UnityEvent buttonYHold;
    [HideInInspector] public UnityEvent buttonYRelease;
    [HideInInspector] public UnityEvent leftTriggerClick;
    [HideInInspector] public UnityEvent leftTriggerRelease;
    [HideInInspector] public UnityEvent leftTriggerHold;
    [HideInInspector] public UnityFloatEvent leftTriggerPressure;
    [HideInInspector] public UnityEvent leftGripClick;
    [HideInInspector] public UnityEvent leftGripRelease;
    [HideInInspector] public UnityEvent leftGripHold;
    [HideInInspector] public UnityFloatEvent leftGripPressure;
    // Right controller
    [HideInInspector] public UnityVector2Event rightJoystickAxis;
    [HideInInspector] public UnityEvent rightJoystickClick;
    [HideInInspector] public UnityEvent rightJoystickHold;
    [HideInInspector] public UnityEvent rightJoystickRelease;
    [HideInInspector] public UnityEvent buttonAClick;
    [HideInInspector] public UnityEvent buttonAHold;
    [HideInInspector] public UnityEvent buttonARelease;
    [HideInInspector] public UnityEvent buttonBClick;
    [HideInInspector] public UnityEvent buttonBHold;
    [HideInInspector] public UnityEvent buttonBRelease;
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
      XRDevice.SetTrackingSpaceType(TrackingSpaceType.RoomScale);
    }

    private void Update()
    {
      if (!leftController.initialized || !rightController.initialized)
      {
        UpdateDevices();
      }
      GetControllerData();
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
      }
    }

    private void GetControllerData()
    {
      if (leftController != null)
      {
        leftController.UpdateControllerData();
      }

      if (rightController != null)
      {
        rightController.UpdateControllerData();
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

      if (float.IsNaN(floorLevel)) return;
      
      if (mainCamera.transform.position.y > (floorLevel + .5f) || input.y > 0)
      {
        transform.Translate(coordinates * movementSpeed * Time.deltaTime);
      }
    }
  }
}