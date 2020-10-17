using UnityEngine;

public class MoonSceneController : MonoBehaviour
{
  public Globe Globe;

  [Range(0.01f, .5f)]
  public float Multiplier = .2f;

  private Material _globeMat;
  private float _pingPong;

  // Start is called before the first frame update
  private void Start()
  {
    _globeMat = Globe.GetComponent<MeshRenderer>().sharedMaterial;
  }

  // Update is called once per frame
  private void Update()
  {
    _globeMat.SetFloat("Opacity", Mathf.PingPong(Time.time * Multiplier, 1f));
  }
}