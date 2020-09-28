using UnityEngine;
using UnityEngine.Rendering;

public static class SphereMeshGenerator
{
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