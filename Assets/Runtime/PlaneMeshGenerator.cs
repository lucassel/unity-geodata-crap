using UnityEngine;

public static class PlaneMeshGenerator
{
  public static Mesh CreatePlaneMesh(Vector2 start, Vector2 end, PlaneOrientation orientation)
  {
    var m = new Mesh();
    switch (orientation)
    {
      case PlaneOrientation.XY:
        m.vertices = new Vector3[]{
                 new Vector3(start.x, 0.01f, start.y),
                 new Vector3(start.x, 0.01f, end.y),
                 new Vector3(end.x, 0.01f, end.y),
                 new Vector3(end.x, 0.01f, start.y)
             };
        break;

      case PlaneOrientation.XZ:
        m.vertices = new Vector3[]{
                 new Vector3(start.x, start.y, 0f),
                 new Vector3(start.x, end.y, 0f),
                 new Vector3(end.x, end.y, 0f),
                 new Vector3(end.x, start.y, 0f)
             };
        break;
    }
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

  private static Mesh CreatePreviewMesh(Vector3 start, Vector3 end)
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