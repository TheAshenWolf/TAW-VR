using Sirenix.OdinInspector;
using TAW_VR.VrHandling;
using UnityEditor;
using UnityEngine;
namespace TAW_VR.Editor
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
      base.OnInspectorGUI();
      if (GUILayout.Button("Setup inputs"))
      {
        VrRigInputSettings inputSettings = (VrRigInputSettings) EditorWindow.GetWindow(typeof(VrRigInputSettings));
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