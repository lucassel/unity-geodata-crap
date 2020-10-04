using UnityEngine;

[CreateAssetMenu()]
public class GlobeSettings : ScriptableObject
{
  [Range(1f, 2000f)]
  public float Radius;

  [Range(0f, 1f)]
  public float Lerp;

  public PlaneOrientation Orientation;
}