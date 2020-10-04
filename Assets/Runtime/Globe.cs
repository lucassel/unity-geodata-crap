﻿using UnityEngine;

using Vector3 = UnityEngine.Vector3;

[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class Globe : MonoBehaviour
{
  [Range(1, 10)] public int DivisionLevel = 10;

  public GlobeSettings Settings;

  public GeoCoord[,] coords;

  public Vector3[,] pointsReal;

  public bool Cartesian;
  public bool DrawCoords;
  public bool DrawIndices;
  public bool DrawGrids;

  public bool UpdateMesh;

  private MeshFilter mf;

  private void OnEnable()
  {
    Settings.OnGlobeSettingsUpdate += UpdateGlobe;
  }

  private void OnDisable()
  {
    Settings.OnGlobeSettingsUpdate -= UpdateGlobe;
  }

  private void Start() => Configure();

  private void Configure()
  {
    mf = GetComponent<MeshFilter>();
    CreateCoordinates(Settings.Orientation, Settings.Radius, Settings.Lerp);
  }

  private void OnValidate() => UpdateGlobe();

  private Vector3[,] CreateCoordinates(PlaneOrientation orientation, float radius, float lerp)
  {
    var multi = 10;

    var width = Settings.HighLongitude - Settings.LowLongitude;
    var height = Settings.HighLatitude - Settings.LowLatitude;
    var offset = new Vector2(radius * 2, radius);

    var newCoords = new GeoCoord[(width / multi) + 1, (height / multi) + 1];

    for (var x = 0; x < newCoords.GetLength(0); x++)
    {
      for (var y = 0; y < newCoords.GetLength(1); y++)
      {
        newCoords[x, y] = new GeoCoord(new Vector2((x + Settings.LowLongitude) + x * multi, (y + Settings.LowLatitude) + y * multi), orientation, radius);
      }
    }
    return CoordinatesToPositions(newCoords, orientation, radius, lerp);
  }

  private Vector3[,] CoordinatesToPositions(GeoCoord[,] coords, PlaneOrientation orientation, float radius, float lerp)
  {
    var pts = new Vector3[coords.GetLength(0), coords.GetLength(1)];
    for (var x = 0; x < coords.GetLength(0); x++)
    {
      for (var y = 0; y < coords.GetLength(1); y++)
      {
        coords[x, y].UpdateCoordinate(orientation, radius);
        pts[x, y] = Vector3.Lerp(coords[x, y].worldPosition, coords[x, y].spherePosition, lerp);
      }
    }
    return pts;
  }

  private void UpdateGlobe()
  {
    pointsReal = CreateCoordinates(Settings.Orientation, Settings.Radius, Settings.Lerp);

    if (UpdateMesh)
    {
      MeshData m = SphereMeshGenerator.GenerateTerrainMesh(pointsReal);
      if (mf is null)
        return;
      mf.sharedMesh = m.CreateMesh();
    }
  }

  private void OnDrawGizmos()
  {
    Gizmos.color = Color.white;
    Gizmos.matrix = transform.localToWorldMatrix;

    if (pointsReal == null)
      return;

    for (var i = 0; i < pointsReal.GetLength(0); i++)
    {
      for (var j = 0; j < pointsReal.GetLength(1); j++)
      {
        if (DrawGrids)
        {
          Gizmos.DrawWireSphere(pointsReal[i, j], 1f);
          if (i > 0)
          {
            Gizmos.DrawLine(pointsReal[i, j], pointsReal[i - 1, j]);
          }

          if (j > 0)
          {
            Gizmos.DrawLine(pointsReal[i, j], pointsReal[i, j - 1]);
          }
        }
      }
    }

    Gizmos.color = Color.white;
    if (Cartesian)
    {
      Gizmos.DrawLine(new Vector3(-Settings.Radius * 2f, Settings.Radius, 0), new Vector3(Settings.Radius * 2f, Settings.Radius, 0));
      Gizmos.DrawLine(new Vector3(-Settings.Radius * 2f, -Settings.Radius, 0), new Vector3(Settings.Radius * 2f, -Settings.Radius, 0));

      Gizmos.DrawLine(new Vector3(-Settings.Radius * 2f, Settings.Radius, 0), new Vector3(-Settings.Radius * 2f, -Settings.Radius, 0));
      Gizmos.DrawLine(new Vector3(Settings.Radius * 2f, Settings.Radius, 0), new Vector3(Settings.Radius * 2f, -Settings.Radius, 0));
    }
    Gizmos.matrix = Matrix4x4.identity;
  }
}