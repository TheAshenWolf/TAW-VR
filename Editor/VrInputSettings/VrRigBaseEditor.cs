using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace TawVR.Editor.VrInputSettings
{
  [CustomEditor(typeof(VrRig))]
  public class VrRigBaseEditor : OdinEditor // Inspector window
  {
    public override void OnInspectorGUI()
    {
      PropertyTree tree = this.Tree;
      tree.DrawMonoScriptObjectField = false;
      tree.Draw();
      
      if (GUILayout.Button("Setup inputs"))
      {
        VrRigInputEditorWindow inputEditor = (VrRigInputEditorWindow) EditorWindow.GetWindow(typeof(VrRigInputEditorWindow), true, "Input editor");
        inputEditor.rigInstance = (VrRig)target;
        inputEditor.Show();
        inputEditor.Init((VrRig)target);
      }
    }
  }
}