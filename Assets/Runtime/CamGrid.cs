using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class CamGrid : MonoBehaviour
{
  public GameObject prefab;
  public readonly Color GridColor = Color.white;
  public int columns, rows;

  private Vector3[,] grid;

  [Range(0.00001f, 0.001f)]
  public float LineWidth = 0.001f;

  [Range(0.0001f, .01f)]
  public float DotScale = .001f;

  private GUIStyle _boxStyle;

  private Texture2D _tex;

  private Vector3[] bounds;
  private List<GameObject> dots = new List<GameObject>();
  private List<LineRenderer> horizontal = new List<LineRenderer>();
  private List<LineRenderer> vertical = new List<LineRenderer>();

  private Camera _cam;

  [Range(1, 20)]
  public int IntersectionSize = 8;

  // Use this for initialization
  private void Start() => Configure();

  private void OnValidate() => Configure();

  private Texture2D MakeTex(int width, int height, Color col)
  {
    var pix = new Color[width * height];
    for (var i = 0; i < pix.Length; ++i)
    {
      pix[i] = col;
    }

    var result = new Texture2D(width, height);
    result.SetPixels(pix);
    result.Apply();
    return result;
  }

  public void UpdateGridColor(Color gridColor)
  {
    _boxStyle.normal.background = MakeTex(1, 1, gridColor);
  }

  private void Configure()
  {
    _boxStyle = new GUIStyle();
    _boxStyle.normal.background = MakeTex(1, 1, GridColor);
    _cam = GetComponent<Camera>();
    GenerateGrid(_cam);
  }

  

  private void OnGUI()
  {
    if (grid != null)
    {
      GUILines(grid);
      GUIIntersections(grid);
    }
  }

  private void GUILines(Vector3[,] grid)
  {
    Gizmos.color = Color.white;
    var res_x = Screen.width / columns;
    for (var x = 1; x < grid.GetLength(0); x++)
    {
      GUI.Box(new Rect(x * res_x, 0, 1, Screen.height), _tex, _boxStyle);
    }
    var res_y = Screen.height / rows;
    for (var y = 1; y < grid.GetLength(1); y++)
    {
      GUI.Box(new Rect(0, y * res_y, Screen.width, 1), _tex, _boxStyle);
    }
  }

  private void GUIIntersections(Vector3[,] grid)
  {
    var res_x = Screen.width / columns;
    var res_y = Screen.height / rows;

    for (var x = 1; x < grid.GetLength(0); x++)
    {
      for (var y = 1; y < grid.GetLength(1); y++)
      {
        GUI.Box(new Rect(x * res_x - IntersectionSize * 4, y * res_y - IntersectionSize / 2, IntersectionSize * 8, IntersectionSize), _tex, _boxStyle);
        GUI.Box(new Rect(x * res_x - IntersectionSize / 2, y * res_y - IntersectionSize * 4, IntersectionSize, IntersectionSize * 8), _tex, _boxStyle);
      }
    }
  }

  // Update is called once per frame
  private void Update()
  {
    GenerateGrid(_cam);
    //Place();
    //PlaceLines();
  }

  private void GenerateGrid(Camera cam)
  {
    bounds = new Vector3[4];
    bounds[0] = cam.ViewportToWorldPoint(new Vector3(0, 0, cam.nearClipPlane)); // left bottom
    bounds[1] = cam.ViewportToWorldPoint(new Vector3(1, 0, cam.nearClipPlane)); // right bottom
    bounds[2] = cam.ViewportToWorldPoint(new Vector3(1, 1, cam.nearClipPlane)); // right top
    bounds[3] = cam.ViewportToWorldPoint(new Vector3(0, 1, cam.nearClipPlane)); // left top

    var res_x = Vector3.Distance(bounds[0], bounds[1]) / columns;
    var res_y = Vector3.Distance(bounds[0], bounds[3]) / rows;

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
    //GizmoIntersections(grid);
  }
}