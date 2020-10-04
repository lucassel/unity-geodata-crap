using System.Collections.Generic;
using System.Linq;
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
  public float DiameterThreshold = 50f;
  private List<Crater> CraterListFull;

  public List<Crater> CraterList;

  [Range(0, 1000)] public int selection;

  public Vector3 Offset = new Vector3(3600f / 2f, 0f, 1800f / 2f);

  public float Multiplier = 10f;
  private GUIStyle _style;

  private List<Crater> GetCratersByDiameter(float threshold)
  {
    var list = CraterListFull.Where(x => x.Diameter > threshold).ToList();
    return list;
  }

  private void OnEnable() => Settings.OnGlobeSettingsUpdate += UpdateCraters;

  private void OnDisable() => Settings.OnGlobeSettingsUpdate -= UpdateCraters;

  private void OnValidate()
  {
    if (CraterListFull == null)
    {
      Configure();
      CraterList = GetCratersByDiameter(DiameterThreshold);
    }

    if (CraterListFull.Count == 0)
    {
      Configure();
      CraterList = GetCratersByDiameter(DiameterThreshold);
    }

    UpdateCraters();
  }

  public void Configure()
  {
    CraterListFull = new List<Crater>();
    var arr = JArray.Parse(CraterData.text);
    for (var i = 0; i < arr.Count; i++)
    {
      var craterName = arr[i]["1. Crater name "].ToString();
      var craterDiameter = float.Parse(arr[i]["2. Diameter [km]"].ToString());
      var lon = float.Parse(arr[i]["4. Longitude [°]"].ToString());
      var lat = float.Parse(arr[i]["3. Latitude [°]"].ToString());
      CraterListFull.Add(new Crater(craterName, craterDiameter, lon, lat, Settings.Radius, Settings.Orientation));
    }

    _style = new GUIStyle
    {
      fontSize = 24
    };
    _style.normal.textColor = Color.red;
  }

  private void Update()
  {
    UpdateCraters();
  }

  public void UpdateCraters()
  {
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

        Gizmos.DrawWireSphere(CraterList[i].Position, CraterList[i].Diameter * Multiplier);
      }
    }
    Gizmos.matrix = Matrix4x4.identity;
  }
}