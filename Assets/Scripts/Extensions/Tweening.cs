using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

namespace EduLabs.Tweening
{

  public static class TweeningExtensions
  {

    public static TweenerCore<float, float, FloatOptions> DOFloat(this MaterialPropertyBlock target, float endValue, string property, float duration)
    {
      return DG.Tweening.DOTween.To(() => target.GetFloat(property), value => target.SetFloat(property, value), endValue, duration);
    }

    public static TweenerCore<Color, Color, ColorOptions> DOColor(this MaterialPropertyBlock target, Color endValue, string property, float duration)
    {
      return DG.Tweening.DOTween.To(() => target.GetColor(property), value => target.SetColor(property, value), endValue, duration);
    }
  }
}