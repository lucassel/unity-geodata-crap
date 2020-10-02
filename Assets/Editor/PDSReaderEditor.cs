using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PDSReader))]
public class PDSReaderEditor : Editor
{
  private PDSReader reader;

  private void OnEnable() => reader = (PDSReader)target;

  public override void OnInspectorGUI()
  {
    if (GUILayout.Button("Read"))
    {
      reader.Read();
    }

    if (GUILayout.Button("Build"))
    {
      EditorHelper.DeleteChildren(reader.gameObject);
      reader.Build();
    }

    if (GUILayout.Button("Clear"))
    {
      EditorHelper.DeleteChildren(reader.gameObject);
    }

    base.OnInspectorGUI();
  }
}