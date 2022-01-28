using UnityEditor;

namespace TAW_VR.Editor
{
  [InitializeOnLoad]
  public class OnInitEditor : UnityEditor.Editor
  {
    static OnInitEditor()
    {
      if (!EditorPrefs.HasKey("TawVRInit") || !EditorPrefs.GetBool("TawVRInit"))
      {
        InitWindow.Init();
        EditorPrefs.SetBool("TawVRInit", true);
      }
    }
  }
}