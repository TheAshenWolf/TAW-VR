using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace TawVR.Editor
{
  [CustomEditor(typeof(VrRig))]
  public class VrRigSettings : UnityEditor.Editor // Inspector window
  {
    [Title("Privates")] 
    private bool editingInput = false;

    private bool editingLeftController;
    private bool editingRightController;
    public override void OnInspectorGUI()
    {
      DrawDefaultInspector();
      if (GUILayout.Button("Setup inputs"))
      {
        VrRigInputSettings inputSettings = (VrRigInputSettings) EditorWindow.GetWindow(typeof(VrRigInputSettings), true, "Input editor");
        inputSettings.rigInstance = (VrRig)target;
        inputSettings.Show();
      }

      EditorGUILayout.Foldout(editingInput, "Input Settings");
      if (editingInput)
      {
        EditorGUILayout.Foldout(editingLeftController, "Left Controller");
        if (editingLeftController)
        {
          
        }
        
        EditorGUILayout.Foldout(editingRightController, "Right Controller");
        if (editingRightController)
        {
        
        }
      }
    }
  }
}