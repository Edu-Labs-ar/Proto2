using DG.Tweening;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace EduLabs.Animation
{
  public class OutlineAnimator
  {

    private float? _baseWidth = null;
    public float baseWidth
    {
      get { return (float)_baseWidth; }
      set
      {
        this._baseWidth = value;
        UpdateWeights();
      }
    }

    private float? _tinkleWidth = null;
    public float tinkleWidth
    {
      get { return (float)_tinkleWidth; }
      set
      {
        this._tinkleWidth = value;
        UpdateWeights();
      }
    }

    private bool ready { get { return settings != null && _baseWidth != null && _tinkleWidth != null; } }

    private Settings.GameSettings settings;

    private Renderer renderer;
    private MaterialPropertyBlock propBlock;

    private Material[] originalMaterials, outlineMaterials;

    private Sequence currSeq;

    private float inDuration, outDuration;

    public OutlineAnimator(Renderer renderer, Material[] outlineMaterials = null)
    {
      Addressables
          .LoadAssetAsync<Settings.GameSettings>(Settings.GameSettings.PATH)
          .Completed += handle =>
          {
            settings = handle.Result;
            UpdateWeights();
          };

      Addressables
          .LoadAssetAsync<Material>(GameInfo.ADDRESSABLE_OUTLINE)
          .Completed += handle =>
            outlineMaterials[outlineMaterials.Length - 1] = handle.Result;

      this.renderer = renderer;
      originalMaterials = renderer.materials;

      if (outlineMaterials == null)
      {
        outlineMaterials = new Material[originalMaterials.Length + 1];
        originalMaterials.CopyTo(outlineMaterials, 0);
      }
      else
        this.outlineMaterials = outlineMaterials;

      propBlock = new MaterialPropertyBlock();
      renderer.GetPropertyBlock(propBlock);
    }


    public void Select(TweenCallback callback = null)
    {
      if (!ready) return;

      AddOutline();
      propBlock.SetFloat("_OutlineWidth", 0f);
      propBlock.SetColor("_OutlineColor", settings.baseColor);
      if (currSeq != null) currSeq.Kill();
      currSeq = propBlock.AnimateOutline(renderer, settings.selectColor, settings.focusedColor,
          tinkleWidth, baseWidth, inDuration, outDuration, callback);
    }

    public void Unselect(TweenCallback callback = null)
    {
      if (!ready) return;
      if (currSeq != null) currSeq.Kill();
      currSeq = propBlock.AnimateOutline(renderer, settings.unselectColor, settings.baseColor,
          tinkleWidth, 0f, outDuration, inDuration, () =>
          {
            if (callback != null) callback.Invoke();
            RemoveOutline();
          });
    }

    public void Grab(TweenCallback callback = null)
    {
      if (!ready) return;
      if (currSeq != null) currSeq.Kill();
      currSeq = propBlock.AnimateOutline(renderer, settings.grabColor, settings.baseColor,
          tinkleWidth, 0f, outDuration, inDuration, () =>
          {
            if (callback != null) callback.Invoke();
            RemoveOutline();
          });
    }

    private void UpdateWeights()
    {
      if (!ready) return;
      float inWeight = tinkleWidth / (2 * tinkleWidth - baseWidth);
      float outWeight = 1 - inWeight;

      inDuration = inWeight * settings.tinkleDuration;
      outDuration = outWeight * settings.tinkleDuration;
    }

    private void AddOutline()
    {
      renderer.materials = outlineMaterials;
    }

    private void RemoveOutline()
    {
      renderer.materials = originalMaterials;
    }
  }
}