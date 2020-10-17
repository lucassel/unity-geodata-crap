using UnityEngine;

public class MoonSceneController : MonoBehaviour
{
  public Globe Globe;
  public CraterUI CraterUI;

  [Range(0.01f, .5f)]
  public float Multiplier = .2f;

  private Material _globeMat;
  public bool Reverse;

  // Start is called before the first frame update
  private void Start()
  {
    _globeMat = Globe.GetComponent<MeshRenderer>().sharedMaterial;
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