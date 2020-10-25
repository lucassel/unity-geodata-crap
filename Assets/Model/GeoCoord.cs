using UnityEngine;

/// <summary>
/// Position on geoid surface.
/// </summary>
public class GeoCoord
{
  /// <summary>
  /// WGS84 notation with single float accuracy gives about 11cm precision on terrestial scale.
  /// </summary>
  public readonly Vector2 polarCoordinate;

  public Vector3 worldPosition { get; private set; }
  public Vector3 spherePosition { get; private set; }

  public GeoCoord(Vector2 coord, PlaneOrientation orientation, float radius, float elevation)
  {
    polarCoordinate = coord;
    UpdateCoordinate(orientation, radius, elevation);
  }

  public void UpdateCoordinate(PlaneOrientation orientation, float radius, float elevation)
  {
    Vector2 worldCoordinate = SphericalCoordinates.PolarCoordinatesToWorldCoordinates(polarCoordinate, radius);
    worldPosition = SphericalCoordinates.CalculateCartesian(worldCoordinate, orientation, elevation);
    spherePosition = SphericalCoordinates.SphericalToCartesian(radius + elevation, polarCoordinate.x, polarCoordinate.y);
  }
}