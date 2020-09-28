﻿using System;

public static class MercatorProjection
{
  private static readonly double R_MAJOR = 6378137.0;
  private static readonly double R_MINOR = 6356752.3142;
  private static readonly double RATIO = R_MINOR / R_MAJOR;
  private static readonly double ECCENT = Math.Sqrt(1.0 - (RATIO * RATIO));
  private static readonly double COM = 0.5 * ECCENT;

  private static readonly double DEG2RAD = Math.PI / 180.0;
  private static readonly double RAD2Deg = 180.0 / Math.PI;
  private static readonly double PI_2 = Math.PI / 2.0;

  public static double[] toPixel(double lon, double lat) => new double[] { lonToX(lon), latToY(lat) };

  public static double[] toGeoCoord(double x, double y) => new double[] { xToLon(x), yToLat(y) };

  public static double lonToX(double lon) => R_MAJOR * DegToRad(lon);

  public static double latToY(double lat)
  {
    lat = Math.Min(89.5, Math.Max(lat, -89.5));
    var phi = DegToRad(lat);
    var sinphi = Math.Sin(phi);
    var con = ECCENT * sinphi;
    con = Math.Pow(((1.0 - con) / (1.0 + con)), COM);
    var ts = Math.Tan(0.5 * ((Math.PI * 0.5) - phi)) / con;
    return 0 - R_MAJOR * Math.Log(ts);
  }

  public static double xToLon(double x) => RadToDeg(x) / R_MAJOR;

  public static double yToLat(double y)
  {
    var ts = Math.Exp(-y / R_MAJOR);
    var phi = PI_2 - 2 * Math.Atan(ts);
    var dphi = 1.0;
    var i = 0;
    while ((Math.Abs(dphi) > 0.000000001) && (i < 15))
    {
      var con = ECCENT * Math.Sin(phi);
      dphi = PI_2 - 2 * Math.Atan(ts * Math.Pow((1.0 - con) / (1.0 + con), COM)) - phi;
      phi += dphi;
      i++;
    }
    return RadToDeg(phi);
  }

  private static double RadToDeg(double rad) => rad * RAD2Deg;

  private static double DegToRad(double deg) => deg * DEG2RAD;
}