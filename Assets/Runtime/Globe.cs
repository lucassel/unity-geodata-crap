using UnityEngine;

using Vector3 = UnityEngine.Vector3;

[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class Globe : MonoBehaviour
{
  [Range(0f, 1f)] public float Lerp;
  [Range(0f, 1700f)] public float Radius = 100;
  public PlaneOrientation Orientation;
  public GeoCoord[,] coords;
  public Vector3[,] pointsReal;

  public bool Cartesian;
  public bool DrawCoords;
  public bool DrawIndices;
  public bool DrawGrids;

  private MeshFilter mf;
  private Vector3 _pos;

  private void Start() => Configure();

  private void OnEnable() => Configure();

  private void Configure()
  {
    mf = GetComponent<MeshFilter>();
    CreateCoordinates(Orientation, Radius, Lerp);
  }

  private Vector3[,] CreateCoordinates(PlaneOrientation orientation, float radius, float lerp)
  {
    coords = new GeoCoord[37, 19];
    for (var x = 0; x < 361; x += 10)
    {
      for (var y = 0; y < 181; y += 10)
      {
        var offset = new Vector2(Radius * 2, Radius);
        var pos = new Vector2(x - 180, y - 90);

        coords[x / 10, y / 10] = new GeoCoord(pos, Orientation, Radius);
      }
    }
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

  private void Update()
  {
    pointsReal = CreateCoordinates(Orientation, Radius, Lerp);
    MeshData m = SphereMeshGenerator.GenerateTerrainMesh(pointsReal);
    mf.sharedMesh = m.CreateMesh();
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
      Gizmos.DrawLine(new Vector3(-Radius * 2f, Radius, 0), new Vector3(Radius * 2f, Radius, 0));
      Gizmos.DrawLine(new Vector3(-Radius * 2f, -Radius, 0), new Vector3(Radius * 2f, -Radius, 0));

      Gizmos.DrawLine(new Vector3(-Radius * 2f, Radius, 0), new Vector3(-Radius * 2f, -Radius, 0));
      Gizmos.DrawLine(new Vector3(Radius * 2f, Radius, 0), new Vector3(Radius * 2f, -Radius, 0));
    }
    Gizmos.matrix = Matrix4x4.identity;
  }
}