using UnityEngine;

public class GeoCoord
{
  public Vector2 coordinate { get; private set; }
  public Vector3 _cartesian { get; private set; }
  public Vector3 _spherical { get; private set; }

  private Vector3 CalculateCartesian(PlaneOrientation orientation) => orientation == PlaneOrientation.XY ? new Vector3(coordinate.x, coordinate.y, 0f) : new Vector3(coordinate.x, 0f, coordinate.y);

  private Vector3 CalculateSpherical(float radius) => SphericalCoordinates.SphericalToCartesian(radius, coordinate.x, coordinate.y);

  public GeoCoord(Vector2 coord, PlaneOrientation orientation, float radius)
  {
    coordinate = coord;
    UpdateCoordinate(orientation, radius);
  }

  public void UpdateCoordinate(PlaneOrientation orientation, float radius)
  {
    _cartesian = CalculateCartesian(orientation);
    _spherical = CalculateSpherical(radius);
  }
}