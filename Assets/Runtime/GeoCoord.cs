using UnityEngine;

public class GeoCoord
{
  private Vector2 coordinate;
  private Vector3 _cartesian;
  private Vector3 _spherical;

  public Vector3 AsCartesian(PlaneOrientation orientation) => orientation == PlaneOrientation.XY ? new Vector3(coordinate.x, coordinate.y, 0f) : new Vector3(coordinate.x, 0f, coordinate.y);

  public Vector3 AsSpherical(float radius) => SphericalCoordinates.SphericalToCartesian(radius, coordinate.x, coordinate.y);

  public Vector3 AsBlendedCoordinate(float lerp) => Vector3.Lerp(_cartesian, _spherical, lerp);

  public GeoCoord(Vector2 coord, PlaneOrientation orientation, float radius)
  {
    coordinate = coord;
    _cartesian = AsCartesian(orientation);
    _spherical = AsSpherical(radius);
  }
}