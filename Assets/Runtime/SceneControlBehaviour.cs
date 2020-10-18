using System;
using UnityEngine;
using UnityEngine.Playables;

[Serializable]
public class SceneControlBehaviour : PlayableBehaviour
{
  [SerializeField]
  private Color detailColor = Color.red;

  [SerializeField]
  [ColorUsage(true, true)]
  private Color backgroundColor = Color.black;

  [SerializeField]
  private Color gridColor = Color.white;

  private bool firstFrameHappened;

  private Color originalDetailColor;
  private Color originalBackgroundColor;
  private Color originalGridColor;

  private MoonSceneController scene;

  public override void ProcessFrame(Playable playable, FrameData info, object playerData)
  {
    //Debug.Log(playerData.ToString());
    scene = playerData as MoonSceneController;
    //Debug.Log(scene);
    if (scene == null)
      return;

    if (!firstFrameHappened)
    {
      originalDetailColor = scene.DetailColor;
      originalBackgroundColor = scene.BackgroundColor;
      originalGridColor = scene.GridColor;

      firstFrameHappened = true;
    }

    scene.UpdateScene(detailColor, backgroundColor, gridColor);
    base.ProcessFrame(playable, info, playerData);
  }

  public override void OnBehaviourPause(Playable playable, FrameData info)
  {
    firstFrameHappened = false;

    base.OnBehaviourPause(playable, info);
  }
}