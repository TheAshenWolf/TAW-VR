using System.Collections.Generic;
using TAW_VR.Runtime.Core.VrHandling;
using UnityEditor;
using UnityEngine;

namespace TAW_VR.Editor.GameObjectEditors
{
  [CustomEditor(typeof(Rotateable))]
  public class RotateableEditor : UnityEditor.Editor
  {
    private List<GUIContent> _toolbarIconsList = new List<GUIContent>();
    private GUIContent[] _toolbarIcons;
    private Rotateable _rotateable;
    private int _toolbarTab;

    private SerializedProperty _settings_canBeRotated;

    private SerializedProperty _details_rotatingWith;

    private void OnEnable()
    {
      _rotateable = (Rotateable)target;
      _toolbarIconsList.Add(new GUIContent(Resources.Load("ToolbarIcons/Settings") as Texture, "Settings"));
      _toolbarIconsList.Add(new GUIContent(Resources.Load("ToolbarIcons/Info") as Texture, "Details"));

      _toolbarIcons = _toolbarIconsList.ToArray();

      _settings_canBeRotated = serializedObject.FindProperty(nameof(_rotateable.canBeRotated));
      _details_rotatingWith = serializedObject.FindProperty(nameof(_rotateable.rotatingWith));
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

          EditorGUILayout.PropertyField(_settings_canBeRotated,
            new GUIContent("Can be rotated",
              "If disabled, the object can't be rotated. Can be used to temporarily disable rotation."));
          break;
        case 1:
          GUILayout.Space(EditorGUIUtility.singleLineHeight / 2);
          GUILayout.Label("<b>Details</b>", middleLabelStyle);
          EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

          GUI.enabled = false;
          EditorGUILayout.PropertyField(_details_rotatingWith,
            new GUIContent("Rotating with", "The controller that is currently rotating this object."));
          GUI.enabled = true;
          break;
      }
      
      serializedObject.ApplyModifiedProperties();
    }
  }
}