using System;
using UnityEditor;
using UnityEngine;

namespace TAW_VR.Editor
{
  public class InitWindow : EditorWindow
  {
    [MenuItem("Tools/TAW VR")]
    public static void Init()
    {
      InitWindow window = (InitWindow)EditorWindow.GetWindow(typeof(InitWindow), true, "Welcome");
      window.maxSize = new Vector2(350, 635);
      window.minSize = new Vector2(350, 635);
      window.Show();
    }

    private void OnGUI()
    {
      GUIStyle middleLabelStyle = new GUIStyle(GUI.skin.label)
      {
        richText = true,
        alignment = TextAnchor.MiddleCenter,
        fontSize = 16
      };

      GUIStyle centeredText = new GUIStyle(GUI.skin.label)
      {
        richText = true,
        alignment = TextAnchor.MiddleCenter
      };
      
      
      GUILayout.Space(EditorGUIUtility.singleLineHeight / 2);
      GUILayout.Label("<b>The Ashen Wolf VR</b>", middleLabelStyle);
      EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
      
      GUILayout.Label("First of all, let me thank you for using this library.", centeredText);
      GUILayout.Label("<color=\"orange\">Please, note, that it requires Sirenix - Odin to run.</color>", centeredText);
      GUILayout.Label("In case you find any bugs, please, report them on GitHub", centeredText);
      GUILayout.Label("https://github.com/TheAshenWolf/TAW-VR/issues", centeredText);
      GUILayout.Label("You can open this window any time from the menu", centeredText);
      GUILayout.Label("Tools -> TAW VR", centeredText);
      
      GUILayout.Space(EditorGUIUtility.singleLineHeight / 2);
      GUILayout.Label("<b>Installation</b>", middleLabelStyle);
      EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
      
      GUILayout.Label("In order to use the library, go to your project window.", centeredText);
      GUILayout.Label("TAW-VR -> Runtime", centeredText);
      GUILayout.Label("Remove the Main Camera from your scene and drag in", centeredText);
      GUILayout.Label("the VrRig prefab.", centeredText);
      GUILayout.Label("Then select the object in your hierarchy and set up", centeredText);
      GUILayout.Label("the inputs. There is the Installation Guide", centeredText);
      GUILayout.Label("which will guide you further.", centeredText);
      
      GUILayout.Space(EditorGUIUtility.singleLineHeight / 2);
      GUILayout.Label("<b>Multiplayer</b>", middleLabelStyle);
      EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
      
      GUILayout.Label("TAW-VR also includes the support for multiplayer.", centeredText);
      GUILayout.Label("In order to use it, you need to include Mirror.", centeredText);
      if (GUILayout.Button("Mirror on the asset store"))
      {
        Application.OpenURL("https://assetstore.unity.com/packages/tools/network/mirror-129321");
      }
      GUILayout.Label("To use the multiplayer, go to your project window.", centeredText);
      GUILayout.Label("TAW-VR -> Runtime -> Multiplayer", centeredText);
      GUILayout.Label("Drag the Lobby scene into your build settings.", centeredText);
      GUILayout.Label("Feel free to edit this scene as much as you need.", centeredText);
      GUILayout.Label("To add your Multiplayer scene, click on the Network", centeredText);
      GUILayout.Label("Manager and assign your scene as the Online Scene.", centeredText);
      
      GUILayout.Space(EditorGUIUtility.singleLineHeight / 2);
      EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
      
      GUILayout.Label("You should be all set now", centeredText);
    }
  }
}