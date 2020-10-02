using UnityEngine;

public static class PlaneMeshGenerator
{
  public static Mesh CreatePreviewMesh(Vector3 start, Vector3 end)
  {
    var m = new Mesh
    {
      name = "PreviewMesh",
      vertices = new Vector3[]{
                 new Vector3(start.x, 0.01f, start.z),
                 new Vector3(start.x, 0.01f, end.z),
                 new Vector3(end.x, 0.01f, end.z),
                 new Vector3(end.x, 0.01f, start.z)
             }
    };
    Vector3 point1 = m.vertices[0];
    Vector3 point2 = m.vertices[1];
    Vector3 point3 = m.vertices[2];
    Vector3 point4 = m.vertices[3];

    m.uv = new Vector2[]{
                 new Vector2 (0, 0),
                 new Vector2 (0, 1),
                 new Vector2(1, 1),
                 new Vector2 (1, 0)
             };
    m.triangles = new int[] { 0, 1, 2, 0, 2, 3 };
    m.RecalculateNormals();
    return m;
  }
}