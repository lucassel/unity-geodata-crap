using System;
using UnityEngine;

public static class SphericalCoordinates
{
  private static float ConvertToRadians(float angle) => (float)(Math.PI / 180) * angle;

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
}