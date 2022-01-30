using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace TawVR.Editor.VrInputSettings
{
  [CustomEditor(typeof(VrRig), isFallback = true)]
  public class VrRigInputsEditor : OdinEditor
  {
    public VrRig rigInstance;
    private GUIStyle _centeredLabel;
    private GUIStyle _middleLabelStyle;
    private Vector2 _scrollPosition;

    [HideInInspector] public bool editingInput = false;

    [HideInInspector] public bool editingLeftJoystick = false;
    [HideInInspector] public bool editingLeftX = false;
    [HideInInspector] public bool editingLeftY = false;
    [HideInInspector] public bool editingLeftTrigger = false;
    [HideInInspector] public bool editingLeftGrip = false;

    [HideInInspector] public bool editingRightJoystick = false;
    [HideInInspector] public bool editingRightA = false;
    [HideInInspector] public bool editingRightB = false;
    [HideInInspector] public bool editingRightTrigger = false;
    [HideInInspector] public bool editingRightGrip = false;

    private SerializedProperty _leftJoystickCalls;
    private SerializedProperty _leftJoystickClickCalls;
    private SerializedProperty _leftJoystickHoldCalls;
    private SerializedProperty _leftJoystickReleaseCalls;
    private SerializedProperty _leftXCalls;
    private SerializedProperty _leftXHoldCalls;
    private SerializedProperty _leftXReleaseCalls;
    private SerializedProperty _leftYCalls;
    private SerializedProperty _leftYHoldCalls;
    private SerializedProperty _leftYReleaseCalls;
    private SerializedProperty _leftTriggerCalls;
    private SerializedProperty _leftTriggerReleasedCalls;
    private SerializedProperty _leftTriggerHoldCalls;
    private SerializedProperty _leftTriggerPressureCalls;
    private SerializedProperty _leftGripCalls;
    private SerializedProperty _leftGripReleasedCalls;
    private SerializedProperty _leftGripHoldCalls;
    private SerializedProperty _leftGripPressureCalls;

    private SerializedProperty _rightJoystickCalls;
    private SerializedProperty _rightJoystickClickCalls;
    private SerializedProperty _rightJoystickHoldCalls;
    private SerializedProperty _rightJoystickReleaseCalls;
    private SerializedProperty _rightACalls;
    private SerializedProperty _rightAHoldCalls;
    private SerializedProperty _rightAReleaseCalls;
    private SerializedProperty _rightBCalls;
    private SerializedProperty _rightBHoldCalls;
    private SerializedProperty _rightBReleaseCalls;
    private SerializedProperty _rightTriggerCalls;
    private SerializedProperty _rightTriggerReleasedCalls;
    private SerializedProperty _rightTriggerHoldCalls;
    private SerializedProperty _rightTriggerPressureCalls;
    private SerializedProperty _rightGripCalls;
    private SerializedProperty _rightGripReleasedCalls;
    private SerializedProperty _rightGripHoldCalls;
    private SerializedProperty _rightGripPressureCalls;
    
    public void Init(VrRig instance)
    {
      _leftJoystickCalls = serializedObject.FindProperty(nameof(rigInstance.leftJoystickAxis));
      _leftJoystickClickCalls = serializedObject.FindProperty(nameof(rigInstance.leftJoystickClick));
      _leftJoystickHoldCalls = serializedObject.FindProperty(nameof(rigInstance.leftJoystickHold));
      _leftJoystickReleaseCalls = serializedObject.FindProperty(nameof(rigInstance.leftJoystickRelease));
      _leftXCalls = serializedObject.FindProperty(nameof(rigInstance.buttonXClick));
      _leftXHoldCalls = serializedObject.FindProperty(nameof(rigInstance.buttonXHold));
      _leftXReleaseCalls = serializedObject.FindProperty(nameof(rigInstance.buttonXRelease));
      _leftYCalls = serializedObject.FindProperty(nameof(rigInstance.buttonYClick));
      _leftYHoldCalls = serializedObject.FindProperty(nameof(rigInstance.buttonYHold));
      _leftYReleaseCalls = serializedObject.FindProperty(nameof(rigInstance.buttonYRelease));
      _leftTriggerCalls = serializedObject.FindProperty(nameof(rigInstance.leftTriggerClick));
      _leftTriggerReleasedCalls = serializedObject.FindProperty(nameof(rigInstance.leftTriggerRelease));
      _leftTriggerHoldCalls = serializedObject.FindProperty(nameof(rigInstance.leftTriggerHold));
      _leftTriggerPressureCalls = serializedObject.FindProperty(nameof(rigInstance.leftTriggerPressure));
      _leftGripCalls = serializedObject.FindProperty(nameof(rigInstance.leftGripClick));
      _leftGripReleasedCalls = serializedObject.FindProperty(nameof(rigInstance.leftGripRelease));
      _leftGripHoldCalls = serializedObject.FindProperty(nameof(rigInstance.leftGripHold));
      _leftGripPressureCalls = serializedObject.FindProperty(nameof(rigInstance.leftGripPressure));

      _rightJoystickCalls = serializedObject.FindProperty(nameof(rigInstance.rightJoystickAxis));
      _rightJoystickClickCalls = serializedObject.FindProperty(nameof(rigInstance.rightJoystickClick));
      _rightJoystickHoldCalls = serializedObject.FindProperty(nameof(rigInstance.rightJoystickHold));
      _rightJoystickReleaseCalls = serializedObject.FindProperty(nameof(rigInstance.rightJoystickRelease));
      _rightACalls = serializedObject.FindProperty(nameof(rigInstance.buttonAClick));
      _rightAHoldCalls = serializedObject.FindProperty(nameof(rigInstance.buttonAHold));
      _rightAReleaseCalls = serializedObject.FindProperty(nameof(rigInstance.buttonARelease));
      _rightBCalls = serializedObject.FindProperty(nameof(rigInstance.buttonBClick));
      _rightBHoldCalls = serializedObject.FindProperty(nameof(rigInstance.buttonBHold));
      _rightBReleaseCalls = serializedObject.FindProperty(nameof(rigInstance.buttonBRelease));
      _rightTriggerCalls = serializedObject.FindProperty(nameof(rigInstance.rightTriggerClick));
      _rightTriggerReleasedCalls = serializedObject.FindProperty(nameof(rigInstance.rightTriggerRelease));
      _rightTriggerHoldCalls = serializedObject.FindProperty(nameof(rigInstance.rightTriggerHold));
      _rightTriggerPressureCalls = serializedObject.FindProperty(nameof(rigInstance.rightTriggerPressure));
      _rightGripCalls = serializedObject.FindProperty(nameof(rigInstance.rightGripClick));
      _rightGripReleasedCalls = serializedObject.FindProperty(nameof(rigInstance.rightGripRelease));
      _rightGripHoldCalls = serializedObject.FindProperty(nameof(rigInstance.rightGripHold));
      _rightGripPressureCalls = serializedObject.FindProperty(nameof(rigInstance.rightGripPressure));
      
      rigInstance = instance;
    }

    public override void OnInspectorGUI()
    {
      MakeStyles();

      serializedObject.Update();
      
      EditorGUILayout.Space(35);
      SirenixEditorGUI.BeginBox(GUILayoutOptions.Height(700).Width(500));
      SirenixEditorGUI.BeginBoxHeader();
      GUILayout.Label("<b>Input settings</b>", _centeredLabel);
      SirenixEditorGUI.EndBoxHeader();
      if (editingInput) EditInput();
      else
      {
        GUILayout.BeginVertical();
        GUILayout.FlexibleSpace();
        GUILayout.Label("<b>Select a button to set its input. The properties for this button will display here.</b>",
          _centeredLabel);
        GUILayout.FlexibleSpace();
        GUILayout.EndVertical();
      }

      SirenixEditorGUI.EndBox();
    }

    private void EditInput()
    {
      _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition, false, true);
      
      GUILayout.Space(EditorGUIUtility.singleLineHeight / 2);
      GUILayout.Label("<b>Left Controller</b>", _middleLabelStyle);
      EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
      
      EditorGUILayout.Foldout(editingLeftJoystick, "Left Joystick");
      if (editingLeftJoystick)
      {
        IndentedEvent(_leftJoystickCalls);
        IndentedEvent(_leftJoystickClickCalls);
        IndentedEvent(_leftJoystickHoldCalls);
        IndentedEvent(_leftJoystickReleaseCalls);
      }

      EditorGUILayout.Foldout(editingLeftX, "Button X");
      if (editingLeftX)
      {
        IndentedEvent(_leftXCalls);
        IndentedEvent(_leftXHoldCalls);
        IndentedEvent(_leftXReleaseCalls);
      }

      EditorGUILayout.Foldout(editingLeftY, "Button Y");
      if (editingLeftY)
      {
        IndentedEvent(_leftYCalls);
        IndentedEvent(_leftYHoldCalls);
        IndentedEvent(_leftYReleaseCalls);
      }

      EditorGUILayout.Foldout(editingLeftTrigger, "Left Trigger");
      if (editingLeftTrigger)
      {
        IndentedEvent(_leftTriggerCalls);
        IndentedEvent(_leftTriggerReleasedCalls);
        IndentedEvent(_leftTriggerHoldCalls);
        IndentedEvent(_leftTriggerPressureCalls);
      }

      EditorGUILayout.Foldout(editingLeftGrip, "Left Grip");
      if (editingLeftGrip)
      {
        IndentedEvent(_leftGripCalls);
        IndentedEvent(_leftGripReleasedCalls);
        IndentedEvent(_leftGripHoldCalls);
        IndentedEvent(_leftGripPressureCalls);
      }

      GUILayout.Space(EditorGUIUtility.singleLineHeight / 2);
      GUILayout.Label("<b>Right Controller</b>", _middleLabelStyle);
      EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
      
      EditorGUILayout.Foldout(editingRightJoystick, "Right Joystick");
      if (editingRightJoystick)
      {
        IndentedEvent(_rightJoystickCalls);
        IndentedEvent(_rightJoystickClickCalls);
        IndentedEvent(_rightJoystickHoldCalls);
        IndentedEvent(_rightJoystickReleaseCalls);
      }

      EditorGUILayout.Foldout(editingRightA, "Button A");
      if (editingRightA)
      {
        IndentedEvent(_rightACalls);
        IndentedEvent(_rightAHoldCalls);
        IndentedEvent(_rightAReleaseCalls);
      }

      EditorGUILayout.Foldout(editingRightB, "Button B");
      if (editingRightB)
      {
        IndentedEvent(_rightBCalls);
        IndentedEvent(_rightBHoldCalls);
        IndentedEvent(_rightBReleaseCalls);
      }

      EditorGUILayout.Foldout(editingRightTrigger, "Right Trigger");
      if (editingRightTrigger)
      {
        IndentedEvent(_rightTriggerCalls);
        IndentedEvent(_rightTriggerReleasedCalls);
        IndentedEvent(_rightTriggerHoldCalls);
        IndentedEvent(_rightTriggerPressureCalls);
      }

      EditorGUILayout.Foldout(editingRightGrip, "Right Grip");
      if (editingRightGrip)
      {
        IndentedEvent(_rightGripCalls);
        IndentedEvent(_rightGripReleasedCalls);
        IndentedEvent(_rightGripHoldCalls);
        IndentedEvent(_rightGripPressureCalls);
      }

      EditorGUILayout.EndScrollView();
      
      serializedObject.ApplyModifiedProperties();
    }

    private void MakeStyles()
    {
      _centeredLabel = new GUIStyle
      {
        alignment = TextAnchor.MiddleCenter,
        richText = true,
        normal = new GUIStyleState()
        {
          textColor = Color.white
        },
        padding = new RectOffset(0, 0, 4, 4)
      };
      
      _middleLabelStyle = new GUIStyle(GUI.skin.label)
      {
        richText = true,
        alignment = TextAnchor.MiddleCenter,
        fontSize = 16
      };
    }

    private void IndentedEvent(SerializedProperty property)
    {
      EditorGUILayout.BeginHorizontal();
      GUILayout.FlexibleSpace();
      EditorGUILayout.PropertyField(property, options: GUILayoutOptions.Width(450));
      EditorGUILayout.EndHorizontal();
    }
  }
}