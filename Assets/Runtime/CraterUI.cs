using System.Collections.Generic;

using UnityEngine;

[ExecuteInEditMode]
public class CraterUI : MonoBehaviour
{
  public Texture BoxTexture;
  public Camera cam;
  public CraterReader CraterReader;
  public bool AutoPlay;
  [Range(0f, 1f)] public float Scanline;
  [Range(0.1f, 6f)] public float ScanlineSpeed;

  public bool CullWithPlane;

  private void Start()
  {
    _style = new GUIStyle
    {
      fontSize = 28
    };
    _style.normal.textColor = new Color(1f, 0f, 0f, alpha);
    _boxStyle = new GUIStyle();
    _tex = MakeTex(2, 2, new Color(1f, 0f, 0f, alpha));
    _boxStyle.normal.background = MakeTex(5, 5, new Color(1f, 0f, 0f, alpha));
    //_boxStyle.normal.background = MakeTex(2, 2, new Color(1f, 0f, 0f, alpha));
  }

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

  // Update is called once per frame
  private void Update()
  {
    if (AutoPlay)
    {
      Scanline = Mathf.PingPong(Time.time * ScanlineSpeed, 1f);
    }
  }

  private void OnDrawGizmos()
  {
    _moonPosition = CraterReader.transform.position;
    _cameraPosition = cam.transform.position;
    _cameraNormal = (_moonPosition - _cameraPosition).normalized;

    _plane = new Plane(_cameraNormal, 0f);
    Gizmos.color = Color.red;
    foreach (Crater c in scannedCraters)
    {
      Gizmos.DrawWireSphere(c.Position, 1f);
    }

    if (_currentCrater != null)
    {
      Gizmos.DrawLine(_cameraPosition, _currentCrater.AsTopDown(CraterReader.Offset, CraterReader.Multiplier));
    }
  }

  private void OnGUI()
  {
    if (CullWithPlane)
    {
      Vector3 craterpos = CraterReader.transform.position;
      Vector3 campos = cam.transform.position;
      Vector3 normal = (craterpos - campos).normalized;
      _plane = new Plane(normal, 0f);
    }

    if (scannedCraters.Count > 0)
    {
      var rnd = UnityEngine.Random.Range(0, scannedCraters.Count);
      _currentCrater = scannedCraters[rnd];
      CraterReader.selection = rnd;
    }

    scannedCraters.Clear();
    foreach (Crater c in CraterReader.CraterList)
    {
      if (CullWithPlane)
      {
        if (_plane.GetSide(c.Position))
        {
          return;
        }
        else
        {
          continue;
        }
      }

      Vector3 p = cam.WorldToScreenPoint(c.AsTopDown(CraterReader.Offset, CraterReader.Multiplier));
      var scan = Mathf.Lerp(0, Screen.height, Scanline);
      GUI.Box(new Rect(0, Screen.height - scan, Screen.width, 8), _tex, _boxStyle);
      if (p.y > scan)
      {
        Vector2 vec = p;
        GUI.Box(new Rect(vec.x, Screen.height - vec.y, BoxSize, BoxSize), _tex, _boxStyle);
        GUI.Label(new Rect(vec.x + 20, Screen.height - vec.y, 100, 20), $"{c.Name}", _style);
        scannedCraters.Add(c);
      }
    }
  }

  private GUIStyle _style;
  private GUIStyle _boxStyle;
  private Texture _tex;
  private int BoxSize = 10;
  private float alpha = .666f;

  private List<Crater> scannedCraters = new List<Crater>();
  private Plane _plane;
  private Vector3 _cameraPosition;
  private Vector3 _cameraNormal;
  private Vector3 _moonPosition;
  private Crater _currentCrater;
}