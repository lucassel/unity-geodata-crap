using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Globe))]
public class GlobeEditor : Editor
{
  private Globe sphere;
  private GUIStyle _style;

  private void OnEnable()
  {
    _style = new GUIStyle
    {
      fontSize = 8,
    };
    _style.normal.textColor = Color.white;
    sphere = (Globe)target;
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
            Handles.Label(sphere.pointsReal[x, y], sphere.coords[x, y].polarCoordinate.ToString(), _style);
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