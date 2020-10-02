﻿using System;

using UnityEngine;

public static class WebTileHelper
{
  public struct BoundingBox
  {
    public double North;
    public double West;
    public double South;
    public double East;
  }

  /// <summary>
  /// Converts a WGS84 longitude in decimal degrees to an X coordinate of a webmap tile at a given
  /// zoom level.
  /// </summary>
  /// <param name="lon"> Longitude inn WGS84 notation </param>
  /// <param name="z"> Zoom level </param>
  /// <returns> </returns>
  public static int Longitude2TileX(double lon, int z) => (int)(Math.Floor((lon + 180.0) / 360.0 * Math.Pow(2.0, z)));

  public static int Latitude2TileY(double lat, int z) => (int)(Math.Floor(
      (1.0 - Math.Log(Math.Tan(lat * Mathf.PI / 180.0) + 1.0 / Math.Cos(lat * Mathf.PI / 180.0)) / Mathf.PI) / 2.0 *
      Math.Pow(2.0, z)));

  public static double Tile2Lon(int x, int z) => x / Math.Pow(2.0, z) * 360.0 - 180;

  public static double Tile2Lat(int y, int z) => (Math.Atan(Math.Sinh(Math.PI - (2.0 * Math.PI * y) / Math.Pow(2.0, z)))) * Mathf.Rad2Deg;

  public static BoundingBox Tile2BoundingBox(int x, int y, int zoom)
  {
    var bb = new BoundingBox
    {
      North = Tile2Lat(y, zoom),
      South = Tile2Lat(y + 1, zoom),
      West = Tile2Lon(x, zoom),
      East = Tile2Lon(x + 1, zoom)
    };
    return bb;
  }
}