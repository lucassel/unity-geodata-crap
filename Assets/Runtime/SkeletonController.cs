using System.Collections.Generic;
using UnityEngine;

public class SkeletonController : MonoBehaviour
{
  private List<Vector3> bones = new List<Vector3>();

  private void Start()
  {
    for (var i = 0; i < transform.childCount; i++)
    {
      Transform bone = transform.GetChild(i);
      Vector2 rand = Random.insideUnitCircle * Random.Range(0f, 4f);
      var height = Random.Range(0f, 2f);
      bones.Add(bone.localPosition);

      transform.GetChild(i).localPosition = new Vector3(rand.x, height, rand.y);
    }
  }

  private void Update()
  {
    for (var i = 0; i < transform.childCount; i++)
    {
    }
  }
}