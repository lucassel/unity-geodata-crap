using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CraterReader))]
public class CraterEditor : Editor
{
  private GUIStyle _style;
  private CraterReader reader;

  private void OnEnable()
  {
    _style = new GUIStyle
    {
      fontSize = 18
    };
    _style.normal.textColor = Color.red;
    reader = (CraterReader)target;
  }

  public override void OnInspectorGUI()
  {
    DrawSettingsEditor(reader.Settings, reader.Settings.OnGlobeSettingsUpdate);

    if (GUILayout.Button("Configure"))
    {
      reader.Configure();
    }
    GlobeSettingsEditor.DrawSettingsEditor(reader.Settings);
    base.OnInspectorGUI();
  }

  private void DrawSettingsEditor(Object settings, System.Action onSettingsUpdated)
  {
    using (var check = new EditorGUI.ChangeCheckScope())
    {
      Editor editor = CreateEditor(settings);
      editor.OnInspectorGUI();

      if (check.changed)
      {
        onSettingsUpdated?.Invoke();
      }
    }
  }

  public void OnSceneGUI()
  {
    if (reader.CraterList != null)
    {
      if (reader.selection > reader.CraterList.Count)
      {
        return;
      }
      Handles.matrix = reader.transform.localToWorldMatrix;
      Handles.Label(reader.CraterList[reader.selection].Position, reader.CraterList[reader.selection].Name, _style);
      Handles.matrix = Matrix4x4.identity;
    }
  }
}