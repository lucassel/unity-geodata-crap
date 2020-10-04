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

  [HideInInspector] public float LongitudeLow = -90;
  [HideInInspector] public float LongitudeMinimum = -180;
  [HideInInspector] public float LongitudeHigh = 90;
  [HideInInspector] public float LongitudeMaximum = 180;

  [HideInInspector] public float LatitudeLow = 0;
  [HideInInspector] public float LatitudeMinimum = -90;
  [HideInInspector] public float LatitudeHigh = 10;
  [HideInInspector] public float LatitudeMaximum = 90;

  public Vector2Int Longi => new Vector2Int((int)LongitudeLow, (int)LongitudeHigh);

  public Vector2Int Lati => new Vector2Int((int)LatitudeLow, (int)LatitudeHigh);
}