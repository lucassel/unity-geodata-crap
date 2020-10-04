using System;
using UnityEngine;

[CreateAssetMenu()]
public class GlobeSettings : ScriptableObject
{
  [Range(1f, 2000f)]
  public float Radius;

  [Range(0f, 1f)]
  public float Lerp;

  public PlaneOrientation Orientation;

  public Action OnGlobeSettingsUpdate;

  public float LongitudeLow = -9;
  public float LongitudeMinimum = -18;
  public float LongitudeHigh = 9;
  public float LongitudeMaximum = 18;

  public float LatitudeLow = 0;
  public float LatitudeMinimum = -9;
  public float LatitudeHigh = 1;
  public float LatitudeMaximum = 9;

  public int LowLongitude => (int)LongitudeLow * 10;
  public int HighLongitude => (int)LongitudeHigh * 10;

  public int LowLatitude => (int)LatitudeLow * 10;
  public int HighLatitude => (int)LatitudeHigh * 10;

  //public Vector2Int Longi => new Vector2Int((int)LongitudeLow, (int)LongitudeHigh);

  //public Vector2Int Lati => new Vector2Int((int)LatitudeLow, (int)LatitudeHigh);
}