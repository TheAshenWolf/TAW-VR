using System.Collections.Generic;
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
    
    // SerializedProperties
    

    private void OnEnable()
    {
      _toolbarIconsList.Add(new GUIContent(Resources.Load("VrRigToolbar/Puzzle") as Texture, "Components"));
      _toolbarIconsList.Add(new GUIContent(Resources.Load("VrRigToolbar/Settings") as Texture, "Settings"));
      _toolbarIconsList.Add(new GUIContent(Resources.Load("VrRigToolbar/Input") as Texture, "Controls"));

      _toolbarIcons = _toolbarIconsList.ToArray();
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


          break;
        case 1:
          GUILayout.Space(EditorGUIUtility.singleLineHeight / 2);
          GUILayout.Label("<b>Settings</b>", middleLabelStyle);
          EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
          break;

        case 2:
          GUILayout.Space(EditorGUIUtility.singleLineHeight / 2);
          GUILayout.Label("<b>Controls</b>", middleLabelStyle);
          EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

          if (GUILayout.Button(new GUIContent("<b>Open controls editor</b>", Resources.Load("VrRigToolbar/Input") as Texture, "Opens the controls editor"), buttonWithIcon))
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