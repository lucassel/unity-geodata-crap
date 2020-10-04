using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Globe))]
public class GlobeEditor : Editor
{
  private Globe sphere;
  private GUIStyle _style;

  private void OnEnable()
  {
    _style = new GUIStyle
    {
      fontSize = 8,
    };
    _style.normal.textColor = Color.white;
    sphere = (Globe)target;
  }

  private void DrawSettingsEditor(Object settings, System.Action onSettingsUpdated)
  {
    using (var check = new EditorGUI.ChangeCheckScope())
    {
      Editor editor = CreateEditor(settings);
      editor.OnInspectorGUI();

      var so = settings as GlobeSettings;
      EditorGUILayout.LabelField("Longitude");
      EditorGUILayout.LabelField("Min Val:", so.Longi.x.ToString());
      EditorGUILayout.LabelField("Max Val:", so.Longi.y.ToString());
      EditorGUILayout.MinMaxSlider(ref so.LongitudeLow, ref so.LongitudeHigh, so.LongitudeMinimum, so.LongitudeMaximum);

      EditorGUILayout.LabelField("Latitude");
      EditorGUILayout.LabelField("Min Val:", so.Lati.x.ToString());
      EditorGUILayout.LabelField("Max Val:", so.Lati.y.ToString());
      EditorGUILayout.MinMaxSlider(ref so.LatitudeLow, ref so.LatitudeHigh, so.LatitudeMinimum, so.LatitudeMaximum);

      if (check.changed)
      {
        onSettingsUpdated?.Invoke();
      }
    }
  }

  public override void OnInspectorGUI()
  {
    DrawSettingsEditor(sphere.Settings, sphere.Settings.OnGlobeSettingsUpdate);
    base.OnInspectorGUI();
  }

  public void OnSceneGUI()
  {
    if (sphere.pointsReal != null)
    {
      for (var x = 0; x < sphere.pointsReal.GetLength(0); x++)
      {
        for (var y = 0; y < sphere.pointsReal.GetLength(1); y++)
        {
          if (sphere.DrawCoords)
          {
            Handles.Label(sphere.pointsReal[x, y], sphere.coords[x, y].polarCoordinate.ToString(), _style);
          }

          if (sphere.DrawIndices)
          {
            Handles.Label(sphere.pointsReal[x, y], $"{x}_{y}", _style);
          }
        }
      }
    }
  }
}