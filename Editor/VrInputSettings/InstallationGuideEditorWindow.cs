using System;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace TawVR.Editor.VrInputSettings
{
  public class InstallationGuideEditorWindow : EditorWindow
  {
    private GUIStyle _richText;
    private GUIStyle _label;

    public void Init()
    {
      position = new Rect(Screen.width / 2f, Screen.height / 2f, 500, 800);
      minSize = new Vector2(500, 800);
      maxSize = new Vector2(500, 800);
    }

    public void OnGUI()
    {
      MakeStyles();

      switch (EditorUserBuildSettings.activeBuildTarget)
      {
        case BuildTarget.Android:
          OnAndroid();
          break;
        case BuildTarget.StandaloneWindows:
        case BuildTarget.StandaloneWindows64:
          OnWindows();
          break;
        default:
          OnOther();
          break;
      }
    }

    private void OnAndroid()
    {
      EditorGUILayout.HelpBox(
        "Your current selected platform is " + EditorUserBuildSettings.activeBuildTarget.ToString() +
        ". \n If you want to set up Oculus link debugging, you need to switch to StandaloneWindows.", MessageType.Info);
      if (GUILayout.Button("Switch to StandaloneWindows"))
      {
        EditorUserBuildSettings.selectedStandaloneTarget = BuildTarget.StandaloneWindows;
      }

      GUILayout.Space(EditorGUIUtility.singleLineHeight);

      SirenixEditorGUI.BeginBox();

      SirenixEditorGUI.BeginBoxHeader();
      GUILayout.Space(4);
      GUILayout.Label("<b>Enable VR input</b> - The first step is to enable the VR support", _label);
      GUILayout.Space(4);
      SirenixEditorGUI.EndBoxHeader();

      GUILayout.Space(EditorGUIUtility.singleLineHeight / 2);
      EditorGUILayout.BeginHorizontal();
      GUILayout.Space(EditorGUIUtility.singleLineHeight);
      PlayerSettings.virtualRealitySupported =
        GUILayout.Toggle(PlayerSettings.virtualRealitySupported, "Virtual Reality Supported");
      EditorGUILayout.EndHorizontal();
      GUILayout.Space(EditorGUIUtility.singleLineHeight / 2);
      SirenixEditorGUI.EndBox();
    }


    private void OnWindows()
    {
      EditorGUILayout.HelpBox(
        "Your current selected platform is " + EditorUserBuildSettings.activeBuildTarget.ToString() +
        ". \n If you want to set up the library, you need to switch to the Android platform. \n However, here you can set up the Oculus Link debugging.",
        MessageType.Warning);

      if (GUILayout.Button("Switch to Android"))
      {
        EditorUserBuildSettings.selectedStandaloneTarget = BuildTarget.Android;
      }
    }


    private void OnOther()
    {
      EditorGUILayout.HelpBox(
        "Your current selected platform is " + EditorUserBuildSettings.activeBuildTarget.ToString() +
        ". \n If you want to set up the library, you need to switch to the Android platform. \n In order to set up Oculus Link debugging, switch to StandaloneWindows.",
        MessageType.Error);

      GUILayout.BeginHorizontal();
      if (GUILayout.Button("Switch to Android"))
      {
        EditorUserBuildSettings.selectedStandaloneTarget = BuildTarget.Android;
      }

      if (GUILayout.Button("Switch to StandaloneWindows"))
      {
        EditorUserBuildSettings.selectedStandaloneTarget = BuildTarget.StandaloneWindows;
      }

      GUILayout.EndHorizontal();
    }

    private void MakeStyles()
    {
      _richText = new GUIStyle
      {
        richText = true,
        normal = new GUIStyleState()
        {
          textColor = Color.white
        },
      };
      
      _label = new GUIStyle
      {
        richText = true,
        normal = new GUIStyleState()
        {
          textColor = Color.white
        },
        padding = new RectOffset(0, 0, 4, 4)
      };
    }
  }
}