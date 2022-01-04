using System;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace TawVR.Editor
{
  [CustomEditor(typeof(VrRig))]
  public class VrRigSettings : OdinEditor // Inspector window
  {
    public override void OnInspectorGUI()
    {
      PropertyTree tree = this.Tree;
      tree.DrawMonoScriptObjectField = false;
      tree.Draw();
      
      if (GUILayout.Button("Setup inputs"))
      {
        VrRigInputEditor inputEditor = (VrRigInputEditor) EditorWindow.GetWindow(typeof(VrRigInputEditor), true, "Input editor");
        inputEditor.rigInstance = (VrRig)target;
        inputEditor.Show();
      }
    }
  }
}