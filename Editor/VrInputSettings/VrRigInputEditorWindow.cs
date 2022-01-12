using System;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEditor.Presets;
using UnityEngine;
using Object = UnityEngine.Object;

namespace TawVR.Editor.VrInputSettings
{
  public class VrRigInputEditorWindow : EditorWindow // Custom editor
  {
    public VrRig rigInstance;

    private Rect _drawingRect;
    private Texture _image;
    private VrRigInputsEditor _inputCallsEditor;
    private GUIStyle _centeredLabel;

    public void Init(VrRig instance)
    {
      rigInstance = instance;

      position = new Rect(100, 100, 1600, 800);
      minSize = new Vector2(1600, 800);
      maxSize = new Vector2(1600, 800);

      _image = Resources.Load("controllers") as Texture;
      _drawingRect = new Rect(200, 100, _image.width, _image.height);
      _inputCallsEditor = (VrRigInputsEditor)UnityEditor.Editor.CreateEditor(rigInstance, typeof(VrRigInputsEditor));
      _inputCallsEditor.Init(rigInstance);
    }


    private void OnGUI()
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

      GUI.skin.label.richText = true;
      GUI.skin.button.richText = true;

      // Input calls editor
      GUILayout.BeginArea(new Rect(1000, 0, 600, 800));
      _inputCallsEditor.OnInspectorGUI();
      GUILayout.EndArea();

      ToolboxModule();
      MethodSourcesModule();
      DiagramModule();
    }

    private void DrawDiscWithColor(Vector3 center, int radius, Color color)
    {
      Color previousColor = Handles.color;
      Handles.color = color;
      Handles.DrawWireDisc(center, new Vector3(0, 0, 1), radius);
      Handles.color = previousColor;
    }

    private void DrawLineWithColor(Vector3 start, Vector3 end, Color color)
    {
      Color previousColor = Handles.color;
      Handles.color = color;
      Handles.DrawLine(start, end);
      Handles.color = previousColor;
    }

    private void SwitchEditing(ref bool part)
    {
      if (!_inputCallsEditor.editingInput)
      {
        _inputCallsEditor.editingInput = true;
        part = true;
      }
      else if (part)
      {
        part = false;
        _inputCallsEditor.editingInput = false;
      }
      else
      {
        DisableAllEditing();
        part = true;
      }
    }

    private void DisableAllEditing()
    {
      _inputCallsEditor.editingLeftJoystick = false;
      _inputCallsEditor.editingLeftX = false;
      _inputCallsEditor.editingLeftY = false;
      _inputCallsEditor.editingLeftTrigger = false;
      _inputCallsEditor.editingLeftGrip = false;

      _inputCallsEditor.editingRightJoystick = false;
      _inputCallsEditor.editingRightA = false;
      _inputCallsEditor.editingRightB = false;
      _inputCallsEditor.editingRightTrigger = false;
      _inputCallsEditor.editingRightGrip = false;
    }


    #region DiagramModule
    private void DiagramModule()
    {
      Color guiColor = GUI.color;
      GUI.color = Color.clear;
      EditorGUI.DrawTextureTransparent(_drawingRect, _image);
      GUI.color = guiColor;

      // Left Controller
      // joystick
      Color color = _inputCallsEditor.editingLeftJoystick ? Color.green : Color.white;
      DrawDiscWithColor(new Vector3(347, 238, 0), 5, color);
      DrawLineWithColor(new Vector3(347, 238, 0), new Vector3(197, 170, 0), color);
      DrawLineWithColor(new Vector3(197, 170, 0), new Vector3(97, 170, 0), color);
      if (GUI.Button(new Rect(97, 145, 100, 20), "<b>Left joystick</b>"))
      {
        SwitchEditing(ref _inputCallsEditor.editingLeftJoystick);
      }

      // y
      color = _inputCallsEditor.editingLeftY ? Color.green : Color.white;
      DrawDiscWithColor(new Vector3(395, 270, 0), 5, color);
      DrawLineWithColor(new Vector3(395, 270, 0), new Vector3(197, 240, 0), color);
      DrawLineWithColor(new Vector3(197, 240, 0), new Vector3(97, 240, 0), color);
      if (GUI.Button(new Rect(97, 215, 100, 20), "<b>Button Y</b>"))
      {
        SwitchEditing(ref _inputCallsEditor.editingLeftY);
      }

      // x
      color = _inputCallsEditor.editingLeftX ? Color.green : Color.white;
      DrawDiscWithColor(new Vector3(365, 285, 0), 5, color);
      DrawLineWithColor(new Vector3(365, 285, 0), new Vector3(197, 320, 0), color);
      DrawLineWithColor(new Vector3(197, 320, 0), new Vector3(97, 320, 0), color);
      if (GUI.Button(new Rect(97, 295, 100, 20), "<b>Button X</b>"))
      {
        SwitchEditing(ref _inputCallsEditor.editingLeftX);
      }

      // trigger
      color = _inputCallsEditor.editingLeftTrigger ? Color.green : Color.white;
      DrawDiscWithColor(new Vector3(410, 330, 0), 5, color);
      DrawLineWithColor(new Vector3(410, 330, 0), new Vector3(350, 525, 0), color);
      DrawLineWithColor(new Vector3(350, 525, 0), new Vector3(250, 525, 0), color);
      if (GUI.Button(new Rect(250, 500, 100, 20), "<b>Left trigger</b>"))
      {
        SwitchEditing(ref _inputCallsEditor.editingLeftTrigger);
      }

      // grip
      color = _inputCallsEditor.editingLeftGrip ? Color.green : Color.white;
      DrawDiscWithColor(new Vector3(350, 370, 0), 5, color);
      DrawLineWithColor(new Vector3(350, 370, 0), new Vector3(197, 420, 0), color);
      DrawLineWithColor(new Vector3(197, 420, 0), new Vector3(97, 420, 0), color);
      if (GUI.Button(new Rect(97, 395, 100, 20), "<b>Left grip</b>"))
      {
        SwitchEditing(ref _inputCallsEditor.editingLeftGrip);
      }

      // Right Controller
      // joystick
      color = _inputCallsEditor.editingRightJoystick ? Color.green : Color.white;
      DrawDiscWithColor(new Vector3(650, 238, 0), 5, color);
      DrawLineWithColor(new Vector3(650, 238, 0), new Vector3(800, 170, 0), color);
      DrawLineWithColor(new Vector3(800, 170, 0), new Vector3(900, 170, 0), color);
      if (GUI.Button(new Rect(800, 145, 100, 20), "<b>Right joystick</b>"))
      {
        SwitchEditing(ref _inputCallsEditor.editingRightJoystick);
      }

      // b
      color = _inputCallsEditor.editingRightB ? Color.green : Color.white;
      DrawDiscWithColor(new Vector3(605, 275, 0), 5, color);
      DrawLineWithColor(new Vector3(605, 275, 0), new Vector3(800, 240, 0), color);
      DrawLineWithColor(new Vector3(800, 240, 0), new Vector3(900, 240, 0), color);
      if (GUI.Button(new Rect(800, 215, 100, 20), "<b>Button B</b>"))
      {
        SwitchEditing(ref _inputCallsEditor.editingRightB);
      }

      // a
      color = _inputCallsEditor.editingRightA ? Color.green : Color.white;
      DrawDiscWithColor(new Vector3(635, 285, 0), 5, color);
      DrawLineWithColor(new Vector3(635, 285, 0), new Vector3(800, 320, 0), color);
      DrawLineWithColor(new Vector3(800, 320, 0), new Vector3(900, 320, 0), color);
      if (GUI.Button(new Rect(800, 295, 100, 20), "<b>Button A</b>"))
      {
        SwitchEditing(ref _inputCallsEditor.editingRightA);
      }

      // trigger
      color = _inputCallsEditor.editingRightTrigger ? Color.green : Color.white;
      DrawDiscWithColor(new Vector3(590, 330, 0), 5, color);
      DrawLineWithColor(new Vector3(590, 330, 0), new Vector3(650, 525, 0), color);
      DrawLineWithColor(new Vector3(650, 525, 0), new Vector3(750, 525, 0), color);
      if (GUI.Button(new Rect(650, 500, 100, 20), "<b>Right trigger</b>"))
      {
        SwitchEditing(ref _inputCallsEditor.editingRightTrigger);
      }

      // grip
      color = _inputCallsEditor.editingRightGrip ? Color.green : Color.white;
      DrawDiscWithColor(new Vector3(650, 370, 0), 5, color);
      DrawLineWithColor(new Vector3(650, 370, 0), new Vector3(800, 420, 0), color);
      DrawLineWithColor(new Vector3(800, 420, 0), new Vector3(900, 420, 0), color);
      if (GUI.Button(new Rect(800, 395, 100, 20), "<b>Right grip</b>"))
      {
        SwitchEditing(ref _inputCallsEditor.editingRightGrip);
      }
    }
    #endregion

    private void MethodSourcesModule()
    {
      // Method sources
      GUILayout.BeginArea(new Rect(515, 550, 460, 185));
      SirenixEditorGUI.BeginBox(GUILayoutOptions.Height(185).Width(460));
      SirenixEditorGUI.BeginBoxHeader();
      GUILayout.Label("<b>Method sources</b>", _centeredLabel);
      SirenixEditorGUI.EndBoxHeader();

      GUILayout.Label("<b>Use the following script links to set up the most common calls.</b>");
      EditorGUILayout.Space(10);
      GUI.enabled = false;
      EditorGUILayout.InspectorTitlebar(false, rigInstance);
      GUI.enabled = true;
      GUILayout.Label("      Contains movement and other logic for the VR");
      GUI.enabled = false;
      EditorGUILayout.InspectorTitlebar(false, rigInstance.rightController);
      GUI.enabled = true;
      GUILayout.Label("      Logic for the <b>right</b> controller");
      GUI.enabled = false;
      EditorGUILayout.InspectorTitlebar(false, rigInstance.leftController);
      GUI.enabled = true;
      GUILayout.Label("      Logic for the <b>left</b> controller");
      SirenixEditorGUI.EndBox();
      GUILayout.EndArea();
    }

    private void ToolboxModule()
    {
      GUILayout.BeginArea(new Rect(25, 550, 460, 185));
      SirenixEditorGUI.BeginBox(GUILayoutOptions.Height(185).Width(460));
      SirenixEditorGUI.BeginBoxHeader();
      GUILayout.Label("<b>Toolbox</b>", _centeredLabel);
      SirenixEditorGUI.EndBoxHeader();

      if (GUI.Button(new Rect(16, 40, 200, 48), "Select or save a preset"))
      {
        PresetSelector.ShowSelector(new Object[] { rigInstance }, null, true);
      }
      
      /*
      if (GUI.Button(new Rect(16, 120, 200, 48), "Generate text-based controls tutorial"))
      {
        throw new NotImplementedException()
      }*/
      
      
      if (GUI.Button(new Rect(240, 40, 200, 48), "Installation guide"))
      {
        InstallationGuideEditorWindow installationGuide = (InstallationGuideEditorWindow) EditorWindow.GetWindow(typeof(InstallationGuideEditorWindow), true, "Taw VR installation guide");
        installationGuide.Init();

        // Go to Build Settings -> Player settings -> XR and check VR supported
        // You might need to disable the Vulkan API. In order to do so, go to Player Settings -> Other and remove Vulkan from the Graphics API list
      }
      /*
      if (GUI.Button(new Rect(240, 120, 200, 48), "Tool"))
      {
      }*/

      SirenixEditorGUI.EndBox();
      GUILayout.EndArea();
    }
  }
}