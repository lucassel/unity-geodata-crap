using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Rendering;

public static class SphereMeshGenerator
{
  public static MeshData GenerateFromGeoCoords(Vector3[,] points, GeoCoord[,] coords, GlobeSettings settings)
  {
    Assert.AreEqual(points.GetLength(0), coords.GetLength(0));
    Assert.AreEqual(points.GetLength(1), coords.GetLength(1));

    var width = points.GetLength(0);
    var height = points.GetLength(1);

    var vertexIndex = 0;

    var meshData = new MeshData(width, height);

    for (var y = 0; y < height; y++)
    {
      for (var x = 0; x < width; x++)
      {
        meshData.vertices[vertexIndex] = points[x, y];

        var x_temp = Mathf.InverseLerp(-180f, 180f, coords[x, y].polarCoordinate.x);
        var uv_x = Mathf.Lerp(0f, 1f, x_temp);

        var y_temp = Mathf.InverseLerp(-90f, 90f, coords[x, y].polarCoordinate.y);
        var uv_y = Mathf.Lerp(0f, 1f, y_temp);

        meshData.uvs[vertexIndex] = new Vector2(uv_x, uv_y);
        if (x < width - 1 && y < height - 1)
        {
          meshData.AddTriangle(vertexIndex, vertexIndex + width, vertexIndex + width + 1);
          meshData.AddTriangle(vertexIndex + width + 1, vertexIndex + 1, vertexIndex);
        }
        vertexIndex++;
      }
    }
    return meshData;
  }

  public static MeshData GenerateTerrainMesh(Vector3[,] points)
  {
    var width = points.GetLength(0);
    var height = points.GetLength(1);

    var vertexIndex = 0;

    var meshData = new MeshData(width, height);

    for (var y = 0; y < height; y++)
    {
      for (var x = 0; x < width; x++)
      {
        meshData.vertices[vertexIndex] = points[x, y];

        meshData.uvs[vertexIndex] = new Vector2(x / (float)width, 1f - y / (float)height);

        if (x < width - 1 && y < height - 1)
        {
          meshData.AddTriangle(vertexIndex, vertexIndex + width, vertexIndex + width + 1);
          meshData.AddTriangle(vertexIndex + width + 1, vertexIndex + 1, vertexIndex);
        }

        vertexIndex++;
      }
    }

    return meshData;
  }
}

public class MeshData
{
  public Vector3[] vertices;
  public int[] triangles;
  public Vector2[] uvs;
  private int triangleIndex;

  public MeshData(int meshWidth, int meshHeight)
  {
    vertices = new Vector3[meshWidth * meshHeight];
    uvs = new Vector2[meshWidth * meshHeight];
    triangles = new int[(meshWidth - 1) * (meshHeight - 1) * 6];
  }

  public void AddTriangle(int a, int b, int c)
  {
    triangles[triangleIndex] = a;
    triangles[triangleIndex + 1] = b;
    triangles[triangleIndex + 2] = c;
    triangleIndex += 3;
  }

  public Mesh CreateMesh()
  {
    var mesh = new Mesh
    {
      vertices = vertices,
      triangles = triangles,
      uv = uvs
    };
    mesh.RecalculateNormals();
    mesh.RecalculateTangents();
    return mesh;
  }

  public Mesh CreateMesh32Bit()
  {
    var mesh = new Mesh
    {
      indexFormat = IndexFormat.UInt32,
      vertices = vertices,
      triangles = triangles,
      uv = uvs
    };
    mesh.RecalculateNormals();
    mesh.RecalculateTangents();
    return mesh;
  }
}