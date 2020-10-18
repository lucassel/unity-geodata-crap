using System;
using UnityEngine;

[Serializable]
public class Crater
{
  public string Name;
  public float Diameter;
  public float Longitude;
  public float Latitude;

  public Vector3 Position;

  public GeoCoord Coordinate;

  public Crater(string name, float diameter, float longitude, float latitude, float sphereRadius, PlaneOrientation orientation)
  {
    Name = name;
    Diameter = diameter;
    Longitude = longitude;
    Latitude = latitude;
    Coordinate = new GeoCoord(new Vector2(longitude, latitude), orientation, sphereRadius, 0f);
  }

  public Vector3 AsSpherical(float radius, Vector3 offset) => SphericalCoordinates.SphericalToCartesian(radius, Longitude, Latitude) + offset;

  public void UpdatePosition(float lerp) => Position = Vector3.Lerp(Coordinate.worldPosition, Coordinate.spherePosition, lerp);

  public void UpdateCrater(PlaneOrientation orientation, float radius)
  {
    if (Coordinate == null)
    {
      Coordinate = new GeoCoord(new Vector2(Longitude, Latitude), orientation, radius, 0f);
    }
    Coordinate.UpdateCoordinate(orientation, radius, 0f);
  }
}