using UnityEngine;

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
  public bool ShowDivisions;
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
    coords = CreateCoordinates(Settings.Orientation, Settings.Radius, Settings.Lerp);
  }

  private void OnValidate() => UpdateGlobe();

  private GeoCoord[,] CreateCoordinates(PlaneOrientation orientation, float radius, float lerp)
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
        newCoords[x, y] = new GeoCoord(new Vector2(Settings.LowLongitude + x * multi, Settings.LowLatitude + y * multi), orientation, radius, 0f);
      }
    }
    return newCoords;
  }

  private Vector3[,] CoordinatesToPositions(GeoCoord[,] coords, PlaneOrientation orientation, float radius, float lerp)
  {
    var pts = new Vector3[coords.GetLength(0), coords.GetLength(1)];
    for (var x = 0; x < coords.GetLength(0); x++)
    {
      for (var y = 0; y < coords.GetLength(1); y++)
      {
        coords[x, y].UpdateCoordinate(orientation, radius, 0f);
        pts[x, y] = Vector3.Lerp(coords[x, y].worldPosition, coords[x, y].spherePosition, lerp);
      }
    }
    return pts;
  }

  private void UpdateGlobe()
  {
    coords = CreateCoordinates(Settings.Orientation, Settings.Radius, Settings.Lerp);
    pointsReal = CoordinatesToPositions(coords, Settings.Orientation, Settings.Radius, Settings.Lerp);

    if (UpdateMesh)
    {
      MeshData m = SphereMeshGenerator.GenerateFromGeoCoords(pointsReal, coords, Settings);
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
        Vector3 p = pointsReal[i, j];
        if (DrawGrids)
        {
          //Gizmos.DrawWireSphere(pointsReal[i, j], 1f);
          if (i > 0)
          {
            Gizmos.DrawLine(p, pointsReal[i - 1, j]);
          }

          if (j > 0)
          {
            Gizmos.DrawLine(p, pointsReal[i, j - 1]);
          }
        }

        if (ShowDivisions)
        {
          var x = Mathf.Abs(p.x);
          var y = Mathf.Abs(p.y);
          var z = Mathf.Abs(p.z);
          if (x > y)
          {
            if (x > z)
              Gizmos.color = Color.red;
            else
              Gizmos.color = Color.cyan;
          }
          else
          {
            if (y > z)
              Gizmos.color = Color.green;
            else
              Gizmos.color = Color.blue;
          }

          Gizmos.DrawWireSphere(p, .01f * Settings.Radius);
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