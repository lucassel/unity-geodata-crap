using UnityEngine;

public class GeoCoord
{
  public Vector2 polarCoordinate { get; private set; }
  public Vector2 worldCoordinate { get; private set; }
  public Vector3 worldPosition { get; private set; }
  public Vector3 spherePosition { get; private set; }

  private Vector3 CalculateCartesian(PlaneOrientation orientation) => orientation == PlaneOrientation.XY ? new Vector3(worldCoordinate.x, worldCoordinate.y, 0f) : new Vector3(worldCoordinate.x, 0f, worldCoordinate.y);

  private Vector3 CalculateSpherical(float radius) => SphericalCoordinates.SphericalToCartesian(radius, polarCoordinate.x, polarCoordinate.y);

  public GeoCoord(Vector2 coord, PlaneOrientation orientation, float radius)
  {
    polarCoordinate = coord;
    UpdateCoordinate(orientation, radius);
  }

  public void UpdateCoordinate(PlaneOrientation orientation, float radius)
  {
    worldCoordinate = new Vector2(polarCoordinate.x, polarCoordinate.y) * radius / 90f;
    worldPosition = CalculateCartesian(orientation);
    spherePosition = CalculateSpherical(radius);
  }
}