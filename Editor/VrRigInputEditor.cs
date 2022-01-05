using UnityEditor;
using UnityEditor.Graphs;
using UnityEngine;

namespace TawVR.Editor
{
  public class VrRigInputEditor : EditorWindow// Custom editor
  {
    public VrRig rigInstance;
    
    private Rect _drawingRect;
    private Texture _image;
    private bool _initialized = false;
    private VrInputSettings _editor;

    public void Init(VrRig instance)
    {
      rigInstance = instance;
      position = new Rect(100, 100, 1600, 800);
      minSize = new Vector2(1600, 800);
      maxSize = new Vector2(1600, 800);
      _image = Resources.Load("controllers") as Texture;
      _drawingRect = new Rect(200, 100, _image.width, _image.height);
      _editor = (VrInputSettings)UnityEditor.Editor.CreateEditor(rigInstance, typeof(VrInputSettings));
      _editor.Init(rigInstance);
      _initialized = true;
    }
    private void OnGUI()
    {
      GUI.skin.button.richText = true;
      
      GUILayout.BeginArea(new Rect(1000, 0, 600, 800));
      _editor.OnInspectorGUI();
      GUILayout.EndArea();
      
      Color guiColor = GUI.color;
      GUI.color = Color.clear;
      EditorGUI.DrawTextureTransparent(_drawingRect, _image);
      GUI.color = guiColor;

      // Left Controller
      // joystick
      Color color = _editor.editingLeftJoystick ? Color.green : Color.white;
      DrawDiscWithColor(new Vector3(347, 238, 0), 5, color);
      DrawLineWithColor(new Vector3(347, 238, 0), new Vector3(197, 170, 0), color);
      DrawLineWithColor(new Vector3(197, 170, 0), new Vector3(97, 170, 0) , color);
      if (GUI.Button(new Rect(97, 145, 100, 20), "<b>Left joystick</b>"))
      {
        SwitchEditing(ref _editor.editingLeftJoystick);
      }

      // y
      color = _editor.editingLeftY ? Color.green : Color.white;
      DrawDiscWithColor(new Vector3(395, 270, 0), 5, color);
      DrawLineWithColor(new Vector3(395, 270, 0), new Vector3(197, 240, 0), color);
      DrawLineWithColor(new Vector3(197, 240, 0), new Vector3(97, 240, 0) , color);
      if (GUI.Button(new Rect(97, 215, 100, 20), "<b>Button Y</b>"))
      {
        SwitchEditing(ref _editor.editingLeftY);
      }
      
      // x
      color = _editor.editingLeftX ? Color.green : Color.white;
      DrawDiscWithColor(new Vector3(365, 285, 0), 5, color);
      DrawLineWithColor(new Vector3(365, 285, 0), new Vector3(197, 320, 0), color);
      DrawLineWithColor(new Vector3(197, 320, 0), new Vector3(97, 320, 0) , color);
      if (GUI.Button(new Rect(97, 295, 100, 20), "<b>Button X</b>"))
      {
        SwitchEditing(ref _editor.editingLeftX);
      }
      
      // trigger
      color = _editor.editingLeftTrigger ? Color.green : Color.white;
      DrawDiscWithColor(new Vector3(410, 330, 0), 5, color);
      DrawLineWithColor(new Vector3(410, 330, 0), new Vector3(350, 525, 0), color);
      DrawLineWithColor(new Vector3(350, 525, 0), new Vector3(250, 525, 0) , color);
      if (GUI.Button(new Rect(250, 500, 100, 20), "<b>Left trigger</b>"))
      {
        SwitchEditing(ref _editor.editingLeftTrigger);
      }
      
      // grip
      color = _editor.editingLeftGrip ? Color.green : Color.white;
      DrawDiscWithColor(new Vector3(350, 370, 0), 5, color);
      DrawLineWithColor(new Vector3(350, 370, 0), new Vector3(197, 420, 0), color);
      DrawLineWithColor(new Vector3(197, 420, 0), new Vector3(97, 420, 0) , color);
      if (GUI.Button(new Rect(97, 395, 100, 20), "<b>Left grip</b>"))
      {
        SwitchEditing(ref _editor.editingLeftGrip);
      }

      // Right Controller
      // joystick
      color = _editor.editingRightJoystick ? Color.green : Color.white;
      DrawDiscWithColor(new Vector3(650, 238, 0), 5, color);
      DrawLineWithColor(new Vector3(650, 238, 0), new Vector3(800, 170, 0), color);
      DrawLineWithColor(new Vector3(800, 170, 0), new Vector3(900, 170, 0) , color);
      if (GUI.Button(new Rect(800, 145, 100, 20), "<b>Right joystick</b>"))
      {
        SwitchEditing(ref _editor.editingRightJoystick);
      }
      
      // b
      color = _editor.editingRightB ? Color.green : Color.white;
      DrawDiscWithColor(new Vector3(605, 275, 0), 5, color);
      DrawLineWithColor(new Vector3(605, 275, 0), new Vector3(800, 240, 0), color);
      DrawLineWithColor(new Vector3(800, 240, 0), new Vector3(900, 240, 0) , color);
      if (GUI.Button(new Rect(800, 215, 100, 20), "<b>Button B</b>"))
      {
        SwitchEditing(ref _editor.editingRightB);
      }
      
      // a
      color = _editor.editingRightA ? Color.green : Color.white;
      DrawDiscWithColor(new Vector3(635, 285, 0), 5, color);
      DrawLineWithColor(new Vector3(635, 285, 0), new Vector3(800, 320, 0), color);
      DrawLineWithColor(new Vector3(800, 320, 0), new Vector3(900, 320, 0) , color);
      if (GUI.Button(new Rect(800, 295, 100, 20), "<b>Button A</b>"))
      {
        SwitchEditing(ref _editor.editingRightA);
      }
      
      // trigger
      color = _editor.editingRightTrigger ? Color.green : Color.white;
      DrawDiscWithColor(new Vector3(590, 330, 0), 5, color);
      DrawLineWithColor(new Vector3(590, 330, 0), new Vector3(650, 525, 0), color);
      DrawLineWithColor(new Vector3(650, 525, 0), new Vector3(750, 525, 0) , color);
      if (GUI.Button(new Rect(650, 500, 100, 20), "<b>Right trigger</b>"))
      {
        SwitchEditing(ref _editor.editingRightTrigger);
      }
      
      // grip
      color = _editor.editingRightGrip ? Color.green : Color.white;
      DrawDiscWithColor(new Vector3(650, 370, 0), 5, color);
      DrawLineWithColor(new Vector3(650, 370, 0), new Vector3(800, 420, 0), color);
      DrawLineWithColor(new Vector3(800, 420, 0), new Vector3(900, 420, 0) , color);
      if (GUI.Button(new Rect(800, 395, 100, 20), "<b>Right grip</b>"))
      {
        SwitchEditing(ref _editor.editingRightGrip);
      }
    }

    private void DrawDiscWithColor(Vector3 center, int radius, Color color)
    {
      Color previousColor = Handles.color;
      Handles.color = color;
      Handles.DrawWireDisc(center,new Vector3(0, 0, 1), radius);
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
      if (!_editor.editingInput)
      {
        _editor.editingInput = true;
        part = true;
      }
      else if (part)
      {
        part = false;
        _editor.editingInput = false;
      }
      else
      {
        DisableAllEditing();
        part = true;
      }
    }

    private void DisableAllEditing()
    { 
      _editor.editingLeftJoystick = false;
      _editor.editingLeftX = false;
      _editor.editingLeftY = false;
      _editor.editingLeftTrigger = false;
      _editor.editingLeftGrip = false;
    
      _editor.editingRightJoystick = false;
      _editor.editingRightA = false;
      _editor.editingRightB = false;
      _editor.editingRightTrigger = false;
      _editor.editingRightGrip = false;
    }
  }
}