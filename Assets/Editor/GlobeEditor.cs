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

  private void DrawSettingsEditor(GlobeSettings settings)
  {
    using (var check = new EditorGUI.ChangeCheckScope())
    {
      var so = settings as GlobeSettings;

      so.Lerp = EditorGUILayout.Slider("Lerp:", so.Lerp, 0f, 1f);
      so.Radius = EditorGUILayout.Slider("Radius:", so.Radius, 1f, 1700f);

      EditorGUILayout.LabelField("Longitude");
      EditorGUILayout.LabelField($"Low: {so.LowLongitude.ToString()}", $"High: {so.HighLongitude.ToString()}");
      EditorGUILayout.MinMaxSlider(ref so.LongitudeLow, ref so.LongitudeHigh, so.LongitudeMinimum, so.LongitudeMaximum);

      EditorGUILayout.LabelField("Latitude");
      EditorGUILayout.LabelField($"Low: {so.LowLatitude.ToString()}", $"High: {so.HighLatitude.ToString()}");
      EditorGUILayout.MinMaxSlider(ref so.LatitudeLow, ref so.LatitudeHigh, so.LatitudeMinimum, so.LatitudeMaximum);

      if (check.changed)
      {
        so.OnGlobeSettingsUpdate.Invoke();
      }
    }
  }

  public override void OnInspectorGUI()
  {
    GlobeSettingsEditor.DrawSettingsEditor(sphere.Settings);
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