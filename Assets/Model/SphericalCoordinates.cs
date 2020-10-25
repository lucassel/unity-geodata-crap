using System;
using UnityEngine;

public static class SphericalCoordinates
{
  /// <summary>
  /// Converts WGS notation into Vector3 sphere position.
  /// </summary>
  /// <param name="radius"> </param>
  /// <param name="polar"> </param>
  /// <param name="elevation"> </param>
  /// <returns> </returns>
  public static Vector3 SphericalToCartesian(float radius, float polar, float elevation)
  {
    polar = ConvertToRadians(polar);
    elevation = ConvertToRadians(elevation);
    var a = radius * Mathf.Cos(elevation);
    Vector3 outCart = Vector3.zero;
    outCart.x = a * Mathf.Cos(polar);
    outCart.y = radius * Mathf.Sin(elevation);
    outCart.z = a * Mathf.Sin(polar);
    return outCart;
  }

  /// <summary>
  /// Converts WGS84 Vector2 coordinate into a world-space Vector2, with a supplied globe radius.
  /// </summary>
  /// <param name="polar"> </param>
  /// <param name="radius"> </param>
  /// <returns> </returns>
  public static Vector2 PolarCoordinatesToWorldCoordinates(Vector2 polar, float radius) => new Vector2(polar.x, polar.y) * radius / 90f;

  /// <summary>
  /// Converts a Vector2 world coordinate into a Vector3 world coordinate with a specific
  /// orientation and elevation.
  /// </summary>
  /// <param name="worldCoordinate"> </param>
  /// <param name="orientation"> </param>
  /// <param name="elevation"> </param>
  /// <returns> Vector3 world coordinate </returns>
  public static Vector3 CalculateCartesian(Vector2 worldCoordinate, PlaneOrientation orientation, float elevation) => orientation == PlaneOrientation.XY ? new Vector3(worldCoordinate.x, worldCoordinate.y, elevation) : new Vector3(worldCoordinate.x, elevation, worldCoordinate.y);

  private static float ConvertToRadians(float angle) => (float)(Math.PI / 180) * angle;
}