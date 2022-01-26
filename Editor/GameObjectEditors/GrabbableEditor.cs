using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace TAW_VR.Editor.GameObjectEditors
{
  [CustomEditor(typeof(TawVR.Runtime.Core.VrHandling.Grabbable))]
  public class GrabbableEditor : UnityEditor.Editor
  {
    private List<GUIContent> _toolbarIconsList = new List<GUIContent>();
    private GUIContent[] _toolbarIcons;
    private TawVR.Runtime.Core.VrHandling.Grabbable _grabbable;
    private int _toolbarTab;

    private SerializedProperty _settings_canBeGrabbed;
    private SerializedProperty _settings_worldPositionStays;
    private SerializedProperty _settings_returnToOriginalSpotOnRelease;

    private SerializedProperty _settings_onGrabbed;
    private SerializedProperty _settings_onReleased;

    private SerializedProperty _details_grabbedBy;
    
    private void OnEnable()
    {
      _grabbable = (TawVR.Runtime.Core.VrHandling.Grabbable)target;
      _toolbarIconsList.Add(new GUIContent(Resources.Load("ToolbarIcons/Settings") as Texture, "Settings"));
      _toolbarIconsList.Add(new GUIContent(Resources.Load("ToolbarIcons/Info") as Texture, "Details"));

      _toolbarIcons = _toolbarIconsList.ToArray();

      _settings_canBeGrabbed = serializedObject.FindProperty(nameof(_grabbable.canBeGrabbed));
      _settings_worldPositionStays = serializedObject.FindProperty(nameof(_grabbable.worldPositionStays));
      _settings_returnToOriginalSpotOnRelease = serializedObject.FindProperty(nameof(_grabbable.returnToOriginalSpotOnRelease));
      _settings_onGrabbed = serializedObject.FindProperty(nameof(_grabbable.onGrabbed));
      _settings_onReleased = serializedObject.FindProperty(nameof(_grabbable.onReleased));

      _details_grabbedBy = serializedObject.FindProperty(nameof(_grabbable.grabbedBy));
    }

    public override void OnInspectorGUI()
    {
      GUIStyle middleLabelStyle = new GUIStyle(GUI.skin.label)
      {
        richText = true,
        alignment = TextAnchor.MiddleCenter,
        fontSize = 16
      };
      
      serializedObject.Update();
      
      _toolbarTab = GUILayout.Toolbar(_toolbarTab, _toolbarIcons);
      switch (_toolbarTab)
      {
        case 0:
          GUILayout.Space(EditorGUIUtility.singleLineHeight / 2);
          GUILayout.Label("<b>Settings</b>", middleLabelStyle);
          EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
          
          EditorGUILayout.PropertyField(_settings_canBeGrabbed, new GUIContent("Can be grabbed", "If disabled, the object can't be grabbed. Can be used to temporarily disable grabbing."));
          GUILayout.Space(EditorGUIUtility.singleLineHeight / 2);
          EditorGUILayout.PropertyField(_settings_worldPositionStays, new GUIContent("World position stays", "Whether the transform relative to the world stays the same when the item is grabbed. There should be no need to disable this setting."));
          EditorGUILayout.PropertyField(_settings_returnToOriginalSpotOnRelease, new GUIContent("Return on release", "If enabled, the item will teleport back to the place where it was picked up from."));
          
          GUILayout.Space(EditorGUIUtility.singleLineHeight / 2);
          GUILayout.Label("<b>Callbacks</b>", middleLabelStyle);
          EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
          EditorGUILayout.PropertyField(_settings_onGrabbed, new GUIContent("On grabbed callback", "Assign methods here to have them called when the item is picked up."));
          EditorGUILayout.PropertyField(_settings_onReleased, new GUIContent("On released callback", "Assign methods here to have them called when the item is relesed."));
          break;
        case 1:
          GUILayout.Space(EditorGUIUtility.singleLineHeight / 2);
          GUILayout.Label("<b>Details</b>", middleLabelStyle);
          EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

          GUI.enabled = false;
          EditorGUILayout.PropertyField(_details_grabbedBy, new GUIContent("Grabbed by", "The controller that is currently holding this object."));
          GUI.enabled = true;
          break;
      }
      serializedObject.ApplyModifiedProperties();
    }
  }
}