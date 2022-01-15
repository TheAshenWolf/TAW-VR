using System.Collections.Generic;
using Sirenix.Utilities;
using UnityEditor;
using UnityEngine;

namespace TawVR.Editor.VrInputSettings
{
  [CustomEditor(typeof(VrRig))]
  public class VrRigBaseEditor : UnityEditor.Editor // Inspector window
  {
    private List<GUIContent> _toolbarIconsList = new List<GUIContent>();
    private GUIContent[] _toolbarIcons;
    private int _toolbarTab = 0;

    private VrRig _rig;
    
    // SerializedProperties
    // Components
    private SerializedProperty _component_mainCamera;
    private SerializedProperty _component_leftController;
    private SerializedProperty _component_rightController;
    
    // Settings
    private SerializedProperty _settings_canTeleportColor;
    private SerializedProperty _settings_cannotTeleportColor;
    private SerializedProperty _settings_movementSpeed;
    
    // Advanced settings
    private SerializedProperty _advancedSettings_advancedColliderDetection;
    private SerializedProperty _advancedSettings_colliderDifferenceThreshold;
    private SerializedProperty _advancedSettings_floorLevel;
    private SerializedProperty _advancedSettings_useBallisticTeleportation;

    // Controls
    private SerializedProperty _controls_clickValue;
    private SerializedProperty _controls_releaseValue;
    private SerializedProperty _controls_teleportationEnabled;
    private SerializedProperty _controls_horizontalMovementEnabled;
    private SerializedProperty _controls_verticalMovementEnabled;
    

    private void OnEnable()
    {
      _rig = (VrRig)target;
      _toolbarIconsList.Add(new GUIContent(Resources.Load("ToolbarIcons/Puzzle") as Texture, "Components"));
      _toolbarIconsList.Add(new GUIContent(Resources.Load("ToolbarIcons/Settings") as Texture, "Settings"));
      _toolbarIconsList.Add(new GUIContent(Resources.Load("ToolbarIcons/Input") as Texture, "Controls"));

      _toolbarIcons = _toolbarIconsList.ToArray();
      
      // Loading serialized properties
      
      _component_mainCamera = serializedObject.FindProperty(nameof(_rig.mainCamera));
      _component_leftController = serializedObject.FindProperty(nameof(_rig.leftController));
      _component_rightController = serializedObject.FindProperty(nameof(_rig.rightController));

      _settings_canTeleportColor = serializedObject.FindProperty(nameof(_rig.canTeleportColor));
      _settings_cannotTeleportColor = serializedObject.FindProperty(nameof(_rig.cannotTeleportColor));
      _settings_movementSpeed = serializedObject.FindProperty(nameof(_rig.movementSpeed));

      _advancedSettings_advancedColliderDetection = serializedObject.FindProperty(nameof(_rig.advancedColliderDetection));
      _advancedSettings_colliderDifferenceThreshold =
        serializedObject.FindProperty(nameof(_rig.colliderDifferenceThreshold));
      _advancedSettings_floorLevel = serializedObject.FindProperty(nameof(_rig.floorLevel));
      _advancedSettings_useBallisticTeleportation =
        serializedObject.FindProperty(nameof(_rig.useBallisticTeleportation));

      _controls_clickValue = serializedObject.FindProperty(nameof(_rig.clickValue));
      _controls_releaseValue = serializedObject.FindProperty(nameof(_rig.releaseValue));
      _controls_teleportationEnabled = serializedObject.FindProperty(nameof(_rig.teleportationEnabled));
      _controls_horizontalMovementEnabled = serializedObject.FindProperty(nameof(_rig.horizontalMovementEnabled));
      _controls_verticalMovementEnabled = serializedObject.FindProperty(nameof(_rig.verticalMovementEnabled));
    }

    public override void OnInspectorGUI()
    {
      GUIStyle middleLabelStyle = new GUIStyle(GUI.skin.label)
      {
        richText = true,
        alignment = TextAnchor.MiddleCenter,
        fontSize = 16
      };

      GUIStyle buttonWithIcon = new GUIStyle(GUI.skin.button)
      {
        imagePosition = ImagePosition.ImageLeft,
        alignment = TextAnchor.MiddleCenter,
        richText = true
      };

      _toolbarTab = GUILayout.Toolbar(_toolbarTab, _toolbarIcons);
      switch (_toolbarTab)
      {
        case 0:
          GUILayout.Space(EditorGUIUtility.singleLineHeight / 2);
          GUILayout.Label("<b>Components</b>", middleLabelStyle);
          EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

          EditorGUILayout.PropertyField(_component_mainCamera, new GUIContent("Main Camera"));
          EditorGUILayout.PropertyField(_component_leftController, new GUIContent("Left Controller"));
          EditorGUILayout.PropertyField(_component_rightController, new GUIContent("Right Controller"));
          break;
        case 1:
          GUILayout.Space(EditorGUIUtility.singleLineHeight / 2);
          GUILayout.Label("<b>Settings</b>", middleLabelStyle);
          EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

          EditorGUILayout.PropertyField(_settings_canTeleportColor, new GUIContent("Can teleport color"));
          EditorGUILayout.PropertyField(_settings_cannotTeleportColor, new GUIContent("Can not teleport color"));
          GUILayout.Space(EditorGUIUtility.singleLineHeight);
          EditorGUILayout.PropertyField(_settings_movementSpeed, new GUIContent("Movement speed"));

          GUILayout.Space(EditorGUIUtility.singleLineHeight / 2);
          GUILayout.Label("<b>Advanced Settings</b>", middleLabelStyle);
          EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
          
          EditorGUILayout.PropertyField(_advancedSettings_advancedColliderDetection, new GUIContent("Advanced collider detection"));
          if (_rig.advancedColliderDetection)
          {
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(_advancedSettings_colliderDifferenceThreshold, new GUIContent("Collider difference threshold"));
            EditorGUI.indentLevel--;
          }
          EditorGUILayout.PropertyField(_advancedSettings_floorLevel, new GUIContent("Floor level"));
          if (_rig.teleportationEnabled) GUI.enabled = false;
          EditorGUILayout.PropertyField(_advancedSettings_useBallisticTeleportation, new GUIContent("Use ballistic teleportation"));
          GUI.enabled = true;
          
          break;

        case 2:
          GUILayout.Space(EditorGUIUtility.singleLineHeight / 2);
          GUILayout.Label("<b>Controls</b>", middleLabelStyle);
          EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
          
          EditorGUILayout.PropertyField(_controls_clickValue, new GUIContent("Click value"));
          EditorGUILayout.PropertyField(_controls_releaseValue, new GUIContent("Release value"));
          GUILayout.Space(EditorGUIUtility.singleLineHeight);
          EditorGUILayout.PropertyField(_controls_teleportationEnabled, new GUIContent("Teleportation enabled"));
          EditorGUILayout.PropertyField(_controls_horizontalMovementEnabled, new GUIContent("Horizontal movement enabled"));
          EditorGUILayout.PropertyField(_controls_verticalMovementEnabled, new GUIContent("Vertical movement enabled"));
          
          /*_controls_clickValue;
    private SerializedProperty _controls_releaseValue;
    private SerializedProperty _controls_teleportationEnabled;
    private SerializedProperty _controls_horizontalMovementEnabled;
    private SerializedProperty _controls_verticalMovementEnabled;*/
          
          GUILayout.Space(EditorGUIUtility.singleLineHeight);
          if (GUILayout.Button(new GUIContent("<b>Open controls editor</b>", Resources.Load("ToolbarIcons/Input") as Texture, "Opens the controls editor"), buttonWithIcon))
          {
            VrRigInputEditorWindow inputEditor =
              (VrRigInputEditorWindow)EditorWindow.GetWindow(typeof(VrRigInputEditorWindow), true, "Controls editor");
            inputEditor.rigInstance = (VrRig)target;
            inputEditor.Show();
            inputEditor.Init((VrRig)target);
          }

          break;
      }

      serializedObject.ApplyModifiedProperties();
    }
  }
}