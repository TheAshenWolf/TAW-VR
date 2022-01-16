using System.Collections.Generic;
using System.Linq;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

namespace TawVR.Editor.VrInputSettings
{
  public class InstallationGuideEditorWindow : EditorWindow
  {
    private GUIStyle _richText;
    private GUIStyle _label;
    public List<GraphicsDeviceType> graphicsAPIs;
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
        "Your current selected platform is " + EditorUserBuildSettings.selectedBuildTargetGroup.ToString() +
        ". \nIf you want to set up Oculus link debugging, you need to switch to Standalone.", MessageType.Info);
      if (GUILayout.Button("Switch platform in build settings"))
      {
        GetWindow(System.Type.GetType("UnityEditor.BuildPlayerWindow,UnityEditor"));
      }

      GUILayout.Space(EditorGUIUtility.singleLineHeight);

      SirenixEditorGUI.BeginBox();
      SirenixEditorGUI.BeginBoxHeader();
      GUILayout.Space(4);
      GUILayout.Label("<b>Enable VR input</b>\n- The first step is to enable the VR support", _label);
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
      
      ///////////////////////////////
      GUILayout.Space(EditorGUIUtility.singleLineHeight);
      
      SirenixEditorGUI.BeginBox();
      SirenixEditorGUI.BeginBoxHeader();
      GUILayout.Space(4);
      GUILayout.Label("<b>Setup Graphical APIs</b>\n- If you see \"Vulkan\" in this list, you need to remove it.", _label);
      GUILayout.Space(4);
      SirenixEditorGUI.EndBoxHeader();
      GUILayout.Space(EditorGUIUtility.singleLineHeight / 2);
      EditorGUILayout.BeginHorizontal();
      GUILayout.Space(EditorGUIUtility.singleLineHeight);
      EditorGUILayout.BeginVertical();
      GUILayout.Label("<b>Current Graphical APIs:</b>", _richText);
      GUILayout.Space(EditorGUIUtility.singleLineHeight / 2);
      
      List<GraphicsDeviceType> apis = PlayerSettings.GetGraphicsAPIs(BuildTarget.Android).ToList();
      foreach (GraphicsDeviceType api in apis)
      {
        if (api.ToString() == "Vulkan")
        {
          GUILayout.Label("  <color=red>" + api + "</color>", _richText);
        }
        else
        {
          GUILayout.Label("  " + api, _richText);
        }
        
      }
      EditorGUILayout.EndVertical();
      EditorGUILayout.EndHorizontal();
      GUILayout.Space(EditorGUIUtility.singleLineHeight / 2);

      if (!apis.Contains(GraphicsDeviceType.Vulkan)) GUI.enabled = false;
      if (GUILayout.Button(new GUIContent("Remove Vulkan", apis.Contains(GraphicsDeviceType.Vulkan) ? "Removes the Vulkan api from the selection. Might take a second." : "Vulkan is not present. You are safe to proceed.")))
      {
        
        apis.Remove(GraphicsDeviceType.Vulkan);
        PlayerSettings.SetGraphicsAPIs(BuildTarget.Android, apis.ToArray());
      }
      GUI.enabled = true;
      SirenixEditorGUI.EndBox();
      
      ///////////////////////////////
    }

    private void OnWindows()
    {
      EditorGUILayout.HelpBox(
        "Your current selected platform is " + EditorUserBuildSettings.selectedBuildTargetGroup.ToString() +
        ". \n If you want to set up the library, you need to switch to the Android platform. \n However, here you can set up the Oculus Link debugging.",
        MessageType.Warning);

      if (GUILayout.Button("Switch platform in build settings"))
      {
        GetWindow(System.Type.GetType("UnityEditor.BuildPlayerWindow,UnityEditor"));
      }

      
      GUILayout.Space(EditorGUIUtility.singleLineHeight);

      SirenixEditorGUI.BeginBox();
      SirenixEditorGUI.BeginBoxHeader();
      GUILayout.Space(4);
      GUILayout.Label("<b>Enable VR input</b>\n- Enable VR support to enable Oculus Link", _label);
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


    private void OnOther()
    {
      EditorGUILayout.HelpBox(
        "Your current selected platform is " + EditorUserBuildSettings.selectedBuildTargetGroup.ToString() +
        ". \n If you want to set up the library, you need to switch to the Android platform. \n In order to set up Oculus Link debugging, switch to Standalone.",
        MessageType.Error);

      GUILayout.BeginHorizontal();
      if (GUILayout.Button("Switch platform in build settings"))
      {
        GetWindow(System.Type.GetType("UnityEditor.BuildPlayerWindow,UnityEditor"));
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