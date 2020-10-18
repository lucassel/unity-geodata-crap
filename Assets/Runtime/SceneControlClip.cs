using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[Serializable]
public class SceneControlClip : PlayableAsset, ITimelineClipAsset
{
  [SerializeField]
  private SceneControlBehaviour template = new SceneControlBehaviour();

  public ClipCaps clipCaps { get { return ClipCaps.None; } }

  public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
  {
    return ScriptPlayable<SceneControlBehaviour>.Create(graph, template);
  }
}