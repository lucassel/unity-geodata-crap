using UnityEditor;

public static class GlobeSettingsEditor
{
  public static void DrawSettingsEditor(GlobeSettings settings)
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
}