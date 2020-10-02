using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GlobeTest))]
public class GlobeEditor : Editor
{
  private GlobeTest sphere;
  private GUIStyle _style;

  private void OnEnable()
  {
    _style = new GUIStyle
    {
      fontSize = 8,
    };
    _style.normal.textColor = Color.white;
    sphere = (GlobeTest)target;
  }

  public void OnSceneGUI()
  {
    if (sphere.pointsReal != null)
    {
      for (var x = 0; x < sphere.pointsReal.GetLength(0); x++)
      {
        for (var y = 0; y < sphere.pointsReal.GetLength(1); y++)
        {
          if (sphere.DrawCoords)
          {
            Handles.Label(sphere.pointsReal[x, y], sphere.points[x, y].ToString(), _style);
          }

          if (sphere.DrawIndices)
          {
            Handles.Label(sphere.pointsReal[x, y], $"{x}_{y}", _style);
          }
        }
      }
    }
  }
}