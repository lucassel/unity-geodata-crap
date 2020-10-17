using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CamGrid : MonoBehaviour
{
  public GameObject prefab;

  public int columns, rows;

  public int ScreenWidth, ScreenHeight;

  private Vector3[,] grid;

  [Range(0.00001f, 0.001f)]
  public float LineWidth = 0.001f;

  [Range(0.0001f, .01f)]
  public float DotScale = .001f;

  private Vector3[] vertices;
  private Vector3[] bounds;
  private List<GameObject> dots = new List<GameObject>();
  private List<LineRenderer> horizontal = new List<LineRenderer>();
  private List<LineRenderer> vertical = new List<LineRenderer>();

  private Camera _cam;

  // Use this for initialization
  private void Start()
  {
    Configure();
  }

  private void OnValidate()
  {
    Configure();
  }

  private void Configure()
  {
    ScreenWidth = Screen.width;
    ScreenHeight = Screen.height;
    _cam = GetComponent<Camera>();
    GenerateGrid(_cam);
  }

  private void SpawnDots()
  {
    foreach (Vector3 item in vertices)
    {
      GameObject go = Instantiate(prefab);
      go.transform.parent = transform;
      Vector3 p = _cam.ViewportToWorldPoint(new Vector3(0, 0, _cam.nearClipPlane));
      go.transform.position = item - transform.position + p;
      go.transform.parent = transform;
      dots.Add(go);
    }
  }

  // Update is called once per frame
  private void Update()
  {
    ScreenWidth = Screen.width;
    ScreenHeight = Screen.height;

    GenerateGrid(_cam);
    //Place();
    //PlaceLines();
  }

  private void GenerateGrid(Camera cam)
  {
    //camDepth = cam.ViewportToWorldPoint(new Vector3(0, 0, cam.nearClipPlane));
    bounds = new Vector3[4];
    bounds[0] = cam.ViewportToWorldPoint(new Vector3(0, 0, cam.nearClipPlane)); // left bottom
    bounds[1] = cam.ViewportToWorldPoint(new Vector3(1, 0, cam.nearClipPlane)); // right bottom
    bounds[2] = cam.ViewportToWorldPoint(new Vector3(1, 1, cam.nearClipPlane)); // right top
    bounds[3] = cam.ViewportToWorldPoint(new Vector3(0, 1, cam.nearClipPlane)); // left top

    var res_x = Vector3.Distance(bounds[0], bounds[1]) / columns;
    var res_y = Vector3.Distance(bounds[0], bounds[3]) / rows;

    //vertices = new Vector3[(columns + 1) * (rows + 1)];
    grid = new Vector3[columns, rows];

    for (var y = 0; y < rows; y++)
    {
      for (var x = 0; x < columns; x++)
      {
        //vertices[i] = new Vector3(x * res_x, y * res_y, 0);
        grid[x, y] = new Vector3(x * res_x, y * res_y, 0f);
      }
    }
  }

  public void Place()
  {
    for (var i = 0; i < dots.Count; i++)
    {
      dots[i].transform.position = vertices[i] + _cam.ViewportToWorldPoint(new Vector3(0, 0, _cam.nearClipPlane));
      dots[i].transform.localScale = Vector3.one * DotScale;
    }
  }

  public void SpawnLines()
  {
    for (var i = 1; i < columns; i++)
    {
      var go = new GameObject("vertical" + i, typeof(LineRenderer));
      go.transform.parent = transform;
      LineRenderer lr = go.GetComponent<LineRenderer>();
      vertical.Add(lr);
      lr.material = new Material(Shader.Find("Unlit/Color"));
    }

    for (var i = 1; i < rows; i++)
    {
      var go = new GameObject("horizontal" + i, typeof(LineRenderer));
      go.transform.parent = transform;
      LineRenderer lr = go.GetComponent<LineRenderer>();
      horizontal.Add(lr);
      lr.material = new Material(Shader.Find("Unlit/Color"));
    }
  }

  /*private void PlaceLines()
  {
    for (var i = 0; i < vertical.Count; i++)
    {
      vertical[i].SetPosition(0, vertices[1 + i] + camDepth);
      vertical[i].SetPosition(1, vertices[vertices.Length - (columns - i)] + camDepth);
      vertical[i].widthMultiplier = LineWidth;
    }

    for (var i = 0; i < horizontal.Count; i++)
    {
      horizontal[i].SetPosition(0, vertices[(columns + 1) * (i + 1)] + camDepth);
      horizontal[i].SetPosition(1, vertices[columns + ((columns + 1) * (i + 1))] + camDepth);
      horizontal[i].widthMultiplier = LineWidth;
    }
  }
  */

  private void GizmoLines(Vector3[,] grid)
  {
    Gizmos.color = Color.white;
    var res_x = 1f / columns;
    for (var x = 1; x < grid.GetLength(0); x++)
    {
      Gizmos.DrawLine(_cam.ViewportToWorldPoint(new Vector3(x * res_x, 0, _cam.nearClipPlane * 1.01f)), _cam.ViewportToWorldPoint(new Vector3(x * res_x, 1, _cam.nearClipPlane * 1.01f)));
    }
    var res_y = 1f / rows;
    for (var y = 1; y < grid.GetLength(1); y++)
    {
      Gizmos.DrawLine(_cam.ViewportToWorldPoint(new Vector3(0, y * res_y, _cam.nearClipPlane * 1.01f)), _cam.ViewportToWorldPoint(new Vector3(1, y * res_y, _cam.nearClipPlane * 1.01f)));
    }
  }

  private void GizmoIntersections(Vector3[,] grid)
  {
    Gizmos.color = Color.white;
    var res_x = 1f / columns;
    var res_y = 1f / rows;

    for (var x = 1; x < grid.GetLength(0); x++)
    {
      for (var y = 1; y < grid.GetLength(1); y++)
      {
        Gizmos.DrawWireCube(_cam.ViewportToWorldPoint(new Vector3(x * res_x, y * res_y, _cam.nearClipPlane * 1.01f)), Vector3.one * .5f);
      }
    }
  }

  private void GizmoGridBounds(Vector3[,] grid)
  {
    Gizmos.color = Color.cyan;
    Gizmos.DrawLine(_cam.ViewportToWorldPoint(new Vector3(0f, 0f, _cam.nearClipPlane * 1.01f)), _cam.ViewportToWorldPoint(new Vector3(0f, 1f, _cam.nearClipPlane * 1.01f)));
    Gizmos.DrawLine(_cam.ViewportToWorldPoint(new Vector3(0f, 0f, _cam.nearClipPlane * 1.01f)), _cam.ViewportToWorldPoint(new Vector3(1f, 0f, _cam.nearClipPlane * 1.01f)));
    Gizmos.DrawLine(_cam.ViewportToWorldPoint(new Vector3(0f, 1f, _cam.nearClipPlane * 1.01f)), _cam.ViewportToWorldPoint(new Vector3(1f, 1f, _cam.nearClipPlane * 1.01f)));
    Gizmos.DrawLine(_cam.ViewportToWorldPoint(new Vector3(1f, 0f, _cam.nearClipPlane * 1.01f)), _cam.ViewportToWorldPoint(new Vector3(1f, 1f, _cam.nearClipPlane * 1.01f)));
  }

  private void OnDrawGizmos()
  {
    _cam = GetComponent<Camera>();
    GenerateGrid(_cam);

    GizmoGridBounds(grid);
    GizmoLines(grid);
    GizmoIntersections(grid);
  }
}