using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace TawVR.Editor
{
  [CustomEditor(typeof(VrRig), isFallback = true)]
  public class VrInputSettings : OdinEditor
  {
    public VrRig rigInstance;
    private GUIStyle _centeredLabel;

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
    private SerializedProperty _leftXCalls;
    private SerializedProperty _leftYCalls;
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
    private SerializedProperty _rightACalls;
    private SerializedProperty _rightBCalls;
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
      _leftXCalls = serializedObject.FindProperty(nameof(rigInstance.buttonXClick));
      _leftYCalls = serializedObject.FindProperty(nameof(rigInstance.buttonYClick));
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
      _rightACalls = serializedObject.FindProperty(nameof(rigInstance.buttonAClick));
      _rightBCalls = serializedObject.FindProperty(nameof(rigInstance.buttonBClick));
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
      EditorGUILayout.Foldout(true, "Left Controller");
      EditorGUI.indentLevel++;
      EditorGUILayout.Foldout(editingLeftJoystick, "Left Joystick");
      if (editingLeftJoystick)
      {
        IndentedEvent(_leftJoystickCalls);
        IndentedEvent(_leftJoystickClickCalls);
      }

      EditorGUILayout.Foldout(editingLeftX, "Button X");
      if (editingLeftX)
      {
        IndentedEvent(_leftXCalls);
      }

      EditorGUILayout.Foldout(editingLeftY, "Button Y");
      if (editingLeftY)
      {
        IndentedEvent(_leftYCalls);
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

      EditorGUI.indentLevel--;

      EditorGUILayout.Foldout(true, "Right Controller");
      EditorGUI.indentLevel++;
      EditorGUILayout.Foldout(editingRightJoystick, "Right Joystick");
      if (editingRightJoystick)
      {
        IndentedEvent(_rightJoystickCalls);
        IndentedEvent(_rightJoystickClickCalls);
      }

      EditorGUILayout.Foldout(editingRightA, "Button A");
      if (editingRightA)
      {
        IndentedEvent(_rightACalls);
      }

      EditorGUILayout.Foldout(editingRightB, "Button B");
      if (editingRightB)
      {
        IndentedEvent(_rightBCalls);
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

      EditorGUI.indentLevel--;
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