using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEngine;

public enum PlaneOrientation
{
  XY,
  XZ
}

[ExecuteInEditMode]
public class CraterReader : MonoBehaviour
{
  public GlobeSettings Settings;
  public TextAsset CraterData;
  private List<Crater> CraterListFull = new List<Crater>();

  public List<Crater> CraterList;

  [Range(0, 1000)] public int selection;

  public Vector3 Offset = new Vector3(3600f / 2f, 0f, 1800f / 2f);

  public bool LimitCratersByDiameter = true;

  [Range(10f, 500f)]
  public float DiameterThreshold = 50f;

  public bool LimitCratersByCoordinates;

  private GUIStyle _style;

  private void OnEnable() => Settings.OnGlobeSettingsUpdate += UpdateCraters;

  private void OnDisable() => Settings.OnGlobeSettingsUpdate -= UpdateCraters;

  private List<Crater> StripCraters(List<Crater> input, float threshold, GlobeSettings settings)
  {
    var result = new List<Crater>();

    for (var i = 0; i < input.Count; i++)
    {
      Crater c = input[i];
      if (c.Diameter < DiameterThreshold)
      { continue; }

      if (c.Longitude < settings.LongitudeLow * 10f)
      {
        continue;
      }

      if (c.Longitude > settings.LongitudeHigh * 10f)
      {
        continue;
      }

      if (c.Latitude < settings.LatitudeLow * 10f)
      {
        continue;
      }

      if (c.Latitude > settings.LatitudeHigh * 10f)
      { continue; }
      Debug.Log($"crater {c.Name} is valid");
      result.Add(c);
    }
    return result;
  }

  private void Awake()
  {
    Configure();
  }

  private void OnValidate()
  {
    Configure();
  }

  public void Configure()
  {
    var arr = JArray.Parse(CraterData.text);
    for (var i = 0; i < arr.Count; i++)
    {
      var craterName = arr[i]["1. Crater name "].ToString();
      var craterDiameter = float.Parse(arr[i]["2. Diameter [km]"].ToString());
      var lon = float.Parse(arr[i]["4. Longitude [°]"].ToString());
      var lat = float.Parse(arr[i]["3. Latitude [°]"].ToString());
      CraterListFull.Add(new Crater(craterName, craterDiameter, lon, lat, Settings.Radius, Settings.Orientation));
    }

    UpdateCraters();

    _style = new GUIStyle
    {
      fontSize = 24
    };
    _style.normal.textColor = Color.red;
  }

  public void UpdateCraters()
  {
    CraterList = StripCraters(CraterListFull, DiameterThreshold, Settings);
    Debug.Log($"serving {CraterList.Count} craters");

    for (var i = 0; i < CraterList.Count; i++)
    {
      CraterList[i].UpdateCrater(Settings.Orientation, Settings.Radius);
      CraterList[i].UpdatePosition(Settings.Lerp);
    }
  }

  private void OnDrawGizmosSelected()
  {
    Gizmos.matrix = transform.localToWorldMatrix;
    if (CraterList != null)
    {
      for (var i = 0; i < CraterList.Count; i++)
      {
        Gizmos.color = i == selection ? Color.red : CraterList[i].Name == "Tycho" ? Color.green : Color.white;

        Gizmos.DrawWireSphere(CraterList[i].Position, CraterList[i].Diameter * Settings.Radius * .0001f);
      }
    }
    Gizmos.matrix = Matrix4x4.identity;
  }
}