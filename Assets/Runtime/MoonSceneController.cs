using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class MoonSceneController : MonoBehaviour
{
  public CraterUI CraterUI;

  [Range(0.01f, .5f)]
  public float Multiplier = .2f;

  public Material _globeMat;
  public bool Reverse;
  public Color DetailColor;

  public Color BackgroundColor;
  public Color GridColor;
  public Camera Camera;

  public void UpdateScene(Color detailColor, Color backgroundColor, Color gridColor)

  {
    CraterUI.UpdateColor(detailColor);
    //Camera.GetComponent<HDAdditionalCameraData>().clearColorMode = HDAdditionalCameraData.ClearColorMode.Color;
    Camera.GetComponent<HDAdditionalCameraData>().backgroundColorHDR = backgroundColor;
    //Camera.GetComponent<HDAdditionalCameraData>().volumeLayerMask = 0;
    Camera.GetComponent<CamGrid>().UpdateGridColor(gridColor);
  }

  // Update is called once per frame
  private void Update()
  {
    var _pingPongDown = 1f - Mathf.PingPong(Time.time * Multiplier, 1f);
    var _pingPongUp = Mathf.PingPong(Time.time * Multiplier, 1f);

    if (Reverse)
    {
      _globeMat.SetFloat("Opacity", _pingPongUp);
      CraterUI.Scanline = _pingPongDown;
    }
    else
    {
      _globeMat.SetFloat("Opacity", _pingPongDown);
      CraterUI.Scanline = _pingPongUp;
    }
  }
}