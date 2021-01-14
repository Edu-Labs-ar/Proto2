using DG.Tweening;
using UnityEngine;

namespace EduLabs.Animation {
  using Tweening;
  public static class AnimationUtils {

    public static Sequence AnimateOutline(
        this MaterialPropertyBlock propBlock, Renderer renderer,
        Color colorRim, Color colorEnd, float tinkleWidth, float targetWidth,
        float growDuration, float shrinkDuration, TweenCallback callback = null)
    {
      Sequence seq = DOTween.Sequence()
        .Append(propBlock.DOFloat(tinkleWidth, "_OutlineWidth", growDuration))
        .Join(propBlock.DOColor(colorRim, "_OutlineColor", growDuration))
        .Append(propBlock.DOFloat(targetWidth, "_OutlineWidth", shrinkDuration))
        .Join(propBlock.DOColor(colorEnd, "_OutlineColor", shrinkDuration))
        .OnUpdate(() => renderer.SetPropertyBlock(propBlock));

      if (callback != null)
        seq.AppendCallback(callback);
      seq.Play();

      return seq;
    }
  }
}