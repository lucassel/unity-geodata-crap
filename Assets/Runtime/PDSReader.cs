using System;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using Random = UnityEngine.Random;

/// <summary>
/// Reads PDS data from disk, constructs mesh.
/// </summary>
public class PDSReader : MonoBehaviour
{
  public PDSData Data;
  public TextAsset LabelFile;

  public float Radius = 90f;
  public bool DrawQuads;
  public bool DrawTexture;
  public Renderer _renderer;
  public bool DrawMesh;

  public int Width;
  public int Height;
  public Material MoonMaterial;

  private void OnValidate()
  {
    Configure();
  }

  private void Start()
  {
    Configure();
  }

  private void Configure()
  {
    _style = new GUIStyle
    {
      fontSize = 38
    };
    _style.normal.textColor = new Color(1f, 0f, 0f, 1f);
  }

  public void Read()
  {
    Data = new PDSData(LabelFile);
    Width = Data.ColumnCount;
    Height = Data.RowCount;
  }

  public void Build()
  {
    Data = new PDSData(LabelFile);
    ReadBinaryFile(LabelFile.name, Data);
  }

  private void OnDrawGizmos()
  {
    if (DrawQuads)
    {
      DivideQuad(Radius * 2, new Vector3(0, 0f, -Radius));
      DivideQuad(Radius * 2, new Vector3(-Radius * 2, 0f, -Radius));

      DrawQuadrant(Radius * 2, new Vector3(0, 0f, -Radius), Color.red);
      DrawQuadrant(Radius * 2, new Vector3(-Radius * 2, 0f, -Radius), Color.green);
    }
  }

  private void DrawQuadrant(float size, Vector3 offset, UnityEngine.Color color)
  {
    Gizmos.color = color;
    Gizmos.DrawLine(offset, offset + new Vector3(0, 0, size));
    Gizmos.DrawLine(offset, offset + new Vector3(size, 0, 0));
    Gizmos.DrawLine(offset + new Vector3(size, 0, 0), offset + new Vector3(size, 0, size));
    Gizmos.DrawLine(offset + new Vector3(0f, 0, size), offset + new Vector3(size, 0, size));
  }

  private void DivideQuad(float size, Vector3 offset)
  {
    var res = 4;
    var step = size / res;
    Gizmos.color = Color.blue;
    for (var x = 0; x < res / 2 + 2; x++)
    {
      for (var y = 0; y < res / 2 + 2; y++)
      {
        if (DrawQuads)
        {
          Gizmos.DrawSphere(new Vector3(offset.x + x * step, 0, offset.z + y * step), 2f);
        }

        DivideQuadFinal(size / res, new Vector3(offset.x + x * step, 0, offset.z + y * step));
      }
    }
  }

  private void DivideQuadFinal(float size, Vector3 offset)
  {
    var res = 4;
    var step = size / res;
    for (var x = 0; x < res / 2 + 2; x++)
    {
      for (var y = 0; y < res / 2 + 2; y++)
      {
        if (DrawQuads)
        {
          Gizmos.DrawSphere(new Vector3(offset.x + x * step, 0, offset.z + y * step), 1f);
          DrawQuadrant(size / res, new Vector3(offset.x + x * step, 0, offset.z + y * step),
            Random.ColorHSV(0f, 1f, 1f, 1f, 1f, 1f, 1f, 1f));
        }
      }
    }
  }

  private void OnGUI()
  {
    if (Data != null)
    {
      GUI.Label(new Rect(20, 40, 100, 20), $"File: {Data.DatasetID}", _style);
      GUI.Label(new Rect(20, 90, 100, 20), $"Resolution: {Data.MapResolution} pixels/degree", _style);
      GUI.Label(new Rect(20, 140, 100, 20), $"Sample bits: {Data.SampleBits} bit depth", _style);
      GUI.Label(new Rect(20, 190, 100, 20), $"Sample type: {Data.SampleType}", _style);
    }
  }

  private void ReadBinaryFile(string fileName, PDSData data)
  {
    _imgData = File.ReadAllBytes(Path.Combine(Application.streamingAssetsPath, $"{fileName}.IMG"));

    var floatData = new float[_imgData.Length / 4];
    var floatDataDimensional = new float[data.ColumnCount, data.RowCount];

    for (var y = 0; y < data.RowCount; y++)
    {
      for (var x = 0; x < data.ColumnCount; x++)
      {
        var i = x + Width * y;
        floatData[i] = BitConverter.ToSingle(_imgData, i * 4);
        floatDataDimensional[x, y] = floatData[i];
      }
    }

    var min = floatData.Min();
    var max = floatData.Max();

    print($"min: {min}, max: {max}");
    if (DrawTexture)
    {
      _renderer.sharedMaterial = new Material(Shader.Find("Standard"));
      _texture = new Texture2D(data.ColumnCount, data.RowCount, TextureFormat.RGBA32, false);
      for (var y = 0; y < data.RowCount; y++)
      {
        for (var x = 0; x < data.ColumnCount; x++)
        {
          var c = (byte)RemapValue(floatDataDimensional[x, y], min, max, 0,
            255);

          var color = new Color32(c, c, c, 255);
          _texture.SetPixel(x, y, color, 0);
        }
      }

      _texture.Apply();
      _renderer.sharedMaterial.mainTexture = _texture;
    }

    if (DrawMesh)
    {
      var verts = new Vector3[data.ColumnCount, data.RowCount];
      print($"width: {verts.GetLength(0)}");
      print($"height: {verts.GetLength(1)}");
      Assert.AreEqual(verts.GetLength(0), Width);
      Assert.AreEqual(verts.GetLength(1), Height);
      for (var y = 0; y < data.RowCount; y++)
      {
        for (var x = 0; x < data.ColumnCount; x++)
        {
          var z = RemapValue(floatDataDimensional[x, data.RowCount - 1 - y], min, max,
            MoonConstants.LowestPointOnTheMoon,
            MoonConstants.HighestPointOnTheMoon);
          verts[x, y] = (new Vector3(x, z, -y) + new Vector3(-data.ColumnCount / 2f, 0, data.RowCount / 2f)) *
                        (Radius * 2f / data.RowCount);
        }
      }

      MeshFilter mf = GetComponent<MeshFilter>();
      MeshRenderer mr = GetComponent<MeshRenderer>();
      if (!DrawTexture)
      {
        mr.material = MoonMaterial;
      }

      MeshData d = SphereMeshGenerator.GenerateTerrainMesh(verts);
      Mesh mesh = d.CreateMesh32Bit();
      mesh = FlipMesh(mesh);
      mf.sharedMesh = mesh;
    }

    if (DrawChunk)
    {
      var verts = new Vector3[Height, Height];

      for (var i = 0; i < 2; i++)
      {
        for (var y = 0; y < data.RowCount; y++)
        {
          for (var x = 0; x < data.ColumnCount / 2; x++)
          {
            var offset = i * Height;
            var z = RemapValue(floatDataDimensional[x + offset, data.RowCount - 1 - y], min, max,
              MoonConstants.LowestPointOnTheMoon,
              MoonConstants.HighestPointOnTheMoon);
            verts[x, y] = (new Vector3(x + offset, z, -y) + new Vector3(-data.ColumnCount / 2f, 0, data.RowCount / 2f)) *
                          (Radius * 2f / data.RowCount);
          }
        }

        if (i == 0)
        {
          MeshData a = SphereMeshGenerator.GenerateTerrainMesh(verts);
          Mesh mesh = a.CreateMesh32Bit();
          mesh = FlipMesh(mesh);
          meshA.sharedMesh = mesh;
        }
        else
        {
          MeshData b = SphereMeshGenerator.GenerateTerrainMesh(verts);
          Mesh mesh = b.CreateMesh32Bit();
          mesh = FlipMesh(mesh);
          meshB.sharedMesh = mesh;
        }
      }
    }
  }

  public bool DrawChunk;

  private static Mesh FlipMesh(Mesh mesh)
  {
    Vector3[] normals = mesh.normals;
    for (var i = 0; i < normals.Length; i++)
      normals[i] = -normals[i];
    mesh.normals = normals;

    var triangles = mesh.GetTriangles(0);
    for (var i = 0; i < triangles.Length; i += 3)
    {
      var temp = triangles[i + 0];
      triangles[i + 0] = triangles[i + 1];
      triangles[i + 1] = temp;
    }

    mesh.SetTriangles(triangles, 0);
    return mesh;
  }

  private static float RemapValue(float val, float min1, float max1, float min2, float max2)
  {
    return Mathf.Lerp(min2, max2, Mathf.InverseLerp(min1, max1, val));
  }

  public MeshFilter meshA, meshB;

  private byte[] _imgData;
  private string _path;
  private Texture2D _texture;
  private GUIStyle _style;
}