using System;
using System.Collections.Generic;
using TAW_VR.Runtime.Core.GameObjectScripts;
using UnityEditor;
using UnityEngine;

namespace TAW_VR.Editor.GameObjectEditors
{
  [CustomEditor(typeof(Scaleable))]
  public class ScaleableEditor : UnityEditor.Editor
  {
    private List<GUIContent> _toolbarIconsList = new List<GUIContent>();
    private GUIContent[] _toolbarIcons;
    private Scaleable _scaleable;
    private int _toolbarTab;
    
    private SerializedProperty _settings_canBeScaled;
    
    private SerializedProperty _details_scalingWith;

    private void OnEnable()
    {
      _scaleable = (Scaleable)target;
      _toolbarIconsList.Add(new GUIContent(Resources.Load("ToolbarIcons/Settings") as Texture, "Settings"));
      _toolbarIconsList.Add(new GUIContent(Resources.Load("ToolbarIcons/Info") as Texture, "Details"));

      _toolbarIcons = _toolbarIconsList.ToArray();

      _settings_canBeScaled = serializedObject.FindProperty(nameof(_scaleable.canBeScaled));
      _details_scalingWith = serializedObject.FindProperty(nameof(_scaleable.scalingWith));
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

          EditorGUILayout.PropertyField(_settings_canBeScaled,
            new GUIContent("Can be scaled",
              "If disabled, the object can't be scaled. Can be used to temporarily disable scaling."));
          break;
        case 1:
          GUILayout.Space(EditorGUIUtility.singleLineHeight / 2);
          GUILayout.Label("<b>Details</b>", middleLabelStyle);
          EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

          GUI.enabled = false;
          EditorGUILayout.PropertyField(_details_scalingWith,
            new GUIContent("Scaling with", "The controller that is currently scaling this object."));
          GUI.enabled = true;
          break;
      }
      
      serializedObject.ApplyModifiedProperties();
    }
  }
}