using System.Collections.Generic;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using TAW_VR.Runtime.Core.Drawing;
using UnityEditor;
using UnityEngine;

namespace TAW_VR.Editor.GameObjectEditors
{
  [CustomEditor(typeof(Drawable))]
  public class DrawableEditor : UnityEditor.Editor
  {
    private List<GUIContent> _toolbarIconsList = new List<GUIContent>();
    private GUIContent[] _toolbarIcons;
    private Drawable _drawable;
    private int _toolbarTab;
    private int _texturePower;

    private void OnEnable()
    {
      _drawable = (Drawable)target;
      _toolbarIconsList.Add(new GUIContent(Resources.Load("ToolbarIcons/Settings") as Texture, "Settings"));
      _toolbarIconsList.Add(new GUIContent(Resources.Load("ToolbarIcons/Info") as Texture, "Details"));
      
      _toolbarIcons = _toolbarIconsList.ToArray();

      _texturePower = _drawable.texturePower;
    }

    public override void OnInspectorGUI()
    {
      GUIStyle middleLabelStyle = new GUIStyle(GUI.skin.label)
      {
        richText = true,
        alignment = TextAnchor.MiddleCenter,
        fontSize = 16
      };
      
      serializedObject.Update();
      
      _toolbarTab = GUILayout.Toolbar(_toolbarTab, _toolbarIcons);
      
      switch (_toolbarTab)
      {
        case 0:
          GUILayout.Space(EditorGUIUtility.singleLineHeight / 2);
          GUILayout.Label("<b>Settings</b>", middleLabelStyle);
          EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

          Rect lastRect = GUILayoutUtility.GetLastRect();
          float fullWidth = lastRect.width;
          
          Rect sliderRect = lastRect;
          sliderRect.position += new Vector2(fullWidth / 4f, EditorGUIUtility.singleLineHeight);
          sliderRect.width *= 0.5f;

          Rect labelRect = sliderRect;
          labelRect.position += new Vector2(fullWidth / 2f, 0);
          labelRect.width *= 0.25f;

          EditorGUILayout.BeginHorizontal();
          GUILayout.Label("Texture size");
          _texturePower = (int)GUI.HorizontalSlider(sliderRect, _texturePower, 5, 11);
          GUI.Label(labelRect,( 2 << _texturePower - 1).ToString(), SirenixGUIStyles.TitleRight);
          EditorGUILayout.EndHorizontal();
          break;
        case 1:
          GUILayout.Space(EditorGUIUtility.singleLineHeight / 2);
          GUILayout.Label("<b>Details</b>", middleLabelStyle);
          EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

          if (_drawable.splatMap == null)
          {
            EditorGUILayout.HelpBox("A texture preview will appear here in the play mode.", MessageType.Info);
          }
          else
          {
            EditorGUILayout.BeginVertical(GUILayoutOptions.MinHeight(256));
            GUILayout.Label("");
            Rect textureRect = GUILayoutUtility.GetLastRect();
            textureRect.position += new Vector2((textureRect.width - 256) / 2 , EditorGUIUtility.singleLineHeight);
            textureRect.width = 256;
            textureRect.height = 256;
            EditorGUI.DrawPreviewTexture(textureRect, _drawable.splatMap);
            EditorGUILayout.EndVertical();
          }
          break;
      }
      
      serializedObject.ApplyModifiedProperties();
    }
  }
}