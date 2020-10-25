using System.Collections.Generic;
using Lucy.Geodata.Model;
using UnityEngine;

public enum ScanlineMethod { TopDown, LeftRight, UV }

public class CraterUI : MonoBehaviour
{
  private GUIStyle _style;
  private GUIStyle _boxStyle;
  private int BoxSize = 10;
  public float alpha = .666f;
  private List<Crater> scannedCraters = new List<Crater>();
  private Plane _plane;
  private Vector3 _cameraPosition;
  private Vector3 _cameraNormal;
  private Vector3 _moonPosition;
  private Crater _currentCrater;
  public Texture BoxTexture;
  private Texture2D _tex;
  public Camera cam;
  public CraterReader CraterReader;
  public bool Reverse;
  [Range(0f, 1f)] public float Scanline;
  [Range(0.1f, 6f)] public float ScanlineSpeed;

  public bool CullWithPlane;
  public ScanlineMethod ScanlineMethod;
  public int FontSize = 20;

  [ColorUsage(true, true)]
  public Color DetailColor;

  private void OnEnable() => Configure();

  private void OnValidate() => Configure();

  public void Configure()
  {
    _style = new GUIStyle
    {
      fontSize = FontSize
    };

    _style.normal.textColor = DetailColor;
    _tex = MakeTex(1, 1, DetailColor);

    _boxStyle = new GUIStyle();
    _boxStyle.normal.background = _tex;
  }

  public void UpdateColor(Color detailColor)
  {
    DetailColor = detailColor;
    _style.normal.textColor = detailColor;
    _tex = MakeTex(1, 1, DetailColor);

    _boxStyle.normal.background = _tex;
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

  private void Update()
  {
    _moonPosition = CraterReader.transform.position;
    _cameraPosition = cam.transform.position;
    _cameraNormal = (_moonPosition - _cameraPosition).normalized;

    _plane = new Plane(_cameraNormal, _moonPosition);
  }

  private void OnGUI()
  {
    switch (ScanlineMethod)
    {
      case ScanlineMethod.TopDown:
        var scan = Mathf.Lerp(0, Screen.height, Scanline);
        if (Reverse)
        {
          GUI.Box(new Rect(0, scan, Screen.width, 8), _tex, _boxStyle);
        }
        else
        {
          GUI.Box(new Rect(0, Screen.height - scan, Screen.width, 8), _tex, _boxStyle);
        }
        foreach (Crater c in CraterReader.CraterList)
        {
          if (CullWithPlane && _plane.GetSide(c.Position))
          {
            continue;
          }
          Vector3 p = cam.WorldToScreenPoint(c.Position);

          if (p.y > scan)
          {
            Vector2 vec = p;
            GUI.Box(new Rect(vec.x - BoxSize / 4, Screen.height - vec.y + BoxSize / 4, BoxSize, BoxSize / 4), _tex, _boxStyle);
            GUI.Box(new Rect(vec.x + BoxSize / 4, Screen.height - vec.y - BoxSize / 4, BoxSize / 4, BoxSize), _tex, _boxStyle);

            GUI.Label(new Rect(vec.x + 10, Screen.height - vec.y, 100, 20), $"{c.Name}", _style);
            scannedCraters.Add(c);
          }
        }
        break;
    }
  }
}