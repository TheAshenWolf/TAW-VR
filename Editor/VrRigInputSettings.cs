using UnityEditor;
using UnityEngine;

namespace TawVR.Editor
{
  public class VrRigInputSettings : EditorWindow// Custom editor
  {
    public VrRig rigInstance;
    
    private Rect _drawingRect;
    private Texture _image;
    private bool _initialized = false;

    private void Init()
    {
      position = new Rect(100, 100, 1600, 800);
      minSize = new Vector2(1600, 800);
      maxSize = new Vector2(1600, 800);
      _image = Resources.Load("controllers") as Texture;
      _drawingRect = new Rect(200, 100, _image.width, _image.height);

      _initialized = true;
    }
    private void OnGUI()
    {
      if (!_initialized) Init();
      
      Color guiColor = GUI.color;
      GUI.color = Color.clear;
      EditorGUI.DrawTextureTransparent(_drawingRect, _image);
      GUI.color = guiColor;
      
      /*for (int x = 0; x < 1600; x += 100)
      {
        for (int y = 0; y < 1600; y += 100)
        {
          Handles.DrawWireDisc(new Vector3(x, y, 0), new Vector3(0, 0, 1), 5);
          Handles.Label(new Vector3(x, y, 0), x + " " + y);
        }
      }*/
      
      // Left Controller
      Handles.color = Color.red;
      Handles.DrawWireDisc(new Vector3(347, 238, 0),new Vector3(0, 0, 1), 5); // joystick
      Handles.DrawWireDisc(new Vector3(365, 285, 0),new Vector3(0, 0, 1), 5); // x
      Handles.DrawWireDisc(new Vector3(395, 270, 0),new Vector3(0, 0, 1), 5); // y
      Handles.DrawWireDisc(new Vector3(410, 330, 0),new Vector3(0, 0, 1), 5); // trigger
      Handles.DrawWireDisc(new Vector3(350, 370, 0),new Vector3(0, 0, 1), 5); // grip
      
      // Right Controller
      Handles.DrawWireDisc(new Vector3(650, 237, 0),new Vector3(0, 0, 1), 5); // joystick
      Handles.DrawWireDisc(new Vector3(635, 285, 0),new Vector3(0, 0, 1), 5); // a
      Handles.DrawWireDisc(new Vector3(605, 275, 0),new Vector3(0, 0, 1), 5); // b
      Handles.DrawWireDisc(new Vector3(590, 330, 0),new Vector3(0, 0, 1), 5); // trigger
      Handles.DrawWireDisc(new Vector3(650, 370, 0),new Vector3(0, 0, 1), 5); // grip
      
      //Handles.DrawLine();
      
      GUILayout.BeginArea(new Rect(1000, 0, 300, 700));
      VrRigSettings editor = (VrRigSettings)UnityEditor.Editor.CreateEditor(rigInstance, typeof(VrRigSettings));
      editor.OnInspectorGUI();
      GUILayout.EndArea();
      
      
    }
  }
}