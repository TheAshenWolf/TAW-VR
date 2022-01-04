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

    private void Init()
    {
      position = new Rect(100, 100, 1600, 800);
      minSize = new Vector2(1600, 800);
      maxSize = new Vector2(1600, 800);
      _image = Resources.Load("controllers") as Texture;
      _drawingRect = new Rect(200, 100, _image.width, _image.height);
      _editor = (VrInputSettings)UnityEditor.Editor.CreateEditor(rigInstance, typeof(VrInputSettings));
      
      _initialized = true;
    }
    private void OnGUI()
    {
      if (!_initialized) Init();
      
      GUILayout.BeginArea(new Rect(1000, 0, 600, 800));
      _editor.OnInspectorGUI();
      GUILayout.EndArea();
      
      Color guiColor = GUI.color;
      GUI.color = Color.clear;
      EditorGUI.DrawTextureTransparent(_drawingRect, _image);
      GUI.color = guiColor;
      
      /*Handles.color = Color.green;
      for (int x = 0; x < 1600; x += 100)
      {
        for (int y = 0; y < 1600; y += 100)
        {
          Handles.DrawWireDisc(new Vector3(x, y, 0), new Vector3(0, 0, 1), 5);
          Handles.Label(new Vector3(x, y, 0), x + " " + y);
        }
      }*/

      Color color;
      
      // Left Controller
      // joystick
      color = _editor.editingLeftJoystick ? Color.green : Color.white;
      DrawDiscWithColor(new Vector3(347, 238, 0), 5, color);
      DrawLineWithColor(new Vector3(347, 238, 0), new Vector3(197, 270, 0), color);
      DrawLineWithColor(new Vector3(197, 270, 0), new Vector3(97, 270, 0) , color);
      if (GUI.Button(new Rect(97, 245, 100, 20), "<b>Left joystick</b>"))
      {
        SwitchEditing(ref _editor.editingLeftJoystick);
      }
      
      
      // x
      color = _editor.editingLeftX ? Color.green : Color.white;
      DrawDiscWithColor(new Vector3(365, 285, 0), 5, color);
      DrawLineWithColor(new Vector3(365, 285, 0), new Vector3(197, 370, 0), color);
      DrawLineWithColor(new Vector3(197, 370, 0), new Vector3(97, 370, 0) , color);
      if (GUI.Button(new Rect(97, 345, 100, 20), "<b>Button X</b>"))
      {
        SwitchEditing(ref _editor.editingLeftX);
      }
      
      DrawDiscWithColor(new Vector3(395, 270, 0), 5, Color.white); // y
      DrawDiscWithColor(new Vector3(410, 330, 0), 5, Color.white); // trigger
      DrawDiscWithColor(new Vector3(350, 370, 0), 5, Color.white); // grip
      
      // Right Controller
      DrawDiscWithColor(new Vector3(650, 237, 0), 5, Color.white); // joystick
      DrawDiscWithColor(new Vector3(635, 285, 0), 5, Color.white); // a
      DrawDiscWithColor(new Vector3(605, 275, 0), 5, Color.white); // b
      DrawDiscWithColor(new Vector3(590, 330, 0), 5, Color.white); // trigger
      DrawDiscWithColor(new Vector3(650, 370, 0), 5, Color.white); // grip
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