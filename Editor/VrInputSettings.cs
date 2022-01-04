using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace TawVR.Editor
{
  public class VrInputSettings : OdinEditor
  {
    private GUIStyle _centeredLabel;
    
    
    [HideInInspector] public bool editingInput = false;
    
    [HideInInspector] public bool editingLeftJoystick = false;
    [HideInInspector] public bool editingLeftX = false;
    [HideInInspector] public bool editingLeftY = false;
    [HideInInspector] public bool editingLeftTrigger = false;
    [HideInInspector] public bool editingLeftGrip = false;
    
    [HideInInspector] public bool editingRightJoystick = false;
    [HideInInspector] public bool editingRightA = false;
    [HideInInspector] public bool editingRightB = false;
    [HideInInspector] public bool editingRightTrigger = false;
    [HideInInspector] public bool editingRightGrip = false;


    public bool EditingLeftController => editingLeftJoystick || editingLeftX || editingLeftY || editingLeftTrigger || editingLeftGrip;
    public bool EditingRightController => editingRightJoystick || editingRightA || editingRightB || editingRightTrigger || editingRightGrip;

    public override void OnInspectorGUI()
    {
      MakeStyles();
      
      EditorGUILayout.Space(35);

      SirenixEditorGUI.BeginBox(GUILayoutOptions.Height(700).Width(500));
      SirenixEditorGUI.BeginBoxHeader();
      GUILayout.Label("<b>Input settings</b>", _centeredLabel);
      SirenixEditorGUI.EndBoxHeader();
      if (editingInput) EditInput();
      else
      {
        GUILayout.BeginVertical();
        GUILayout.FlexibleSpace();
        GUILayout.Label("<b>Select a button to set its input. The properties for this button will display here.</b>", _centeredLabel);
        GUILayout.FlexibleSpace();
        GUILayout.EndVertical();
      }
      SirenixEditorGUI.EndBox();
    }

    private void EditInput()
    {
      
      EditorGUILayout.Foldout(EditingLeftController, "Left Controller");
      if (EditingLeftController)
      {
      }

      EditorGUILayout.Foldout(EditingRightController, "Right Controller");
      if (EditingRightController)
      {
      }
    }

    private void MakeStyles()
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
    }
  }
}