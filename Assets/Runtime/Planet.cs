using UnityEngine;

public class Planet : MonoBehaviour
{
  [Range(2, 256)]
  public int resolution = 20;

  [SerializeField, HideInInspector]
  private MeshFilter[] meshFilters;

  private TerrainFace[] terrainFaces;

  private void OnValidate()
  {
    Init();
    GenerateMesh();
  }

  private void Init()
  {
    if (meshFilters == null || meshFilters.Length == 0)
    {
      meshFilters = new MeshFilter[6];
    }

    Vector3[] dirs = { Vector3.up, Vector3.down, Vector3.left, Vector3.right, Vector3.forward, Vector3.back };
    terrainFaces = new TerrainFace[6];
    for (var i = 0; i < 6; i++)
    {
      if (meshFilters[i] == null)
      {
        var meshObj = new GameObject("Mesh");
        meshObj.transform.parent = transform;
        meshObj.AddComponent<MeshRenderer>().sharedMaterial = new Material(Shader.Find("HDRP/Lit"));

        meshFilters[i] = meshObj.AddComponent<MeshFilter>();
        meshFilters[i].sharedMesh = new Mesh();
      }

      terrainFaces[i] = new TerrainFace(meshFilters[i].sharedMesh, resolution, dirs[i]);
    }
  }

  private void GenerateMesh()
  {
    foreach (TerrainFace face in terrainFaces)
    {
      face.ConstructMesh();
    }
  }
}