using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace EduLabs.Mechanics.Micropipettes
{
  using Animation;
  using Volumetrics;
  using Tools;
  using Interaction;
  using Recipients;
  public class Micropipette : MonoBehaviour, Tool
  {
    [Serializable]
    public struct TriggerStep
    {
      [HideInInspector]
      public int index;

      [Range(0, 1)]
      public float position;
      [Range(-1, 2)]
      public float value;
      [Range(.05f, .8f)]
      public float sensitivity;
    }

    [Header("References")]
    public MeshRenderer targetMesh;

    [Header("Animation")]
    [Range(0f, .1f)]
    public float baseRimWidth = .005f;

    [Range(0f, .1f)]
    public float tinkleRimWidth = .012f;

    [Header("Pipette")]
    [Rename("Upper Limit (mL)")]
    public float upperLimit = 20;
    [Rename("Lower Limit (mL)")]
    public float lowerLimit = .5f;
    [Rename("Desired Volume (mL)")]
    public float desiredVolume = 20;
    [Rename("Internal Capacity (mL)")]
    public float internalCapacity = 25;
    [Rename("Extra Push (mL)")]
    public float extraPush = 1;
    public Tip tip;
    public TriggerStep[] trigger;


    private bool ready { get { return settings != null && tip != null; } }

    private ResizableContainer container;

    private OutlineAnimator outlineAnimator;
    private Settings.PlayerSettings settings;

    private InteractionType mode;
    private Recipient target;
    private bool panEnabled;

    private float _internalCapacity;
    private float _extraPush;
    private float triggerPosition = 0f;
    private float triggerValue = 0f;


    void OnValidate()
    {
      if (outlineAnimator != null)
      {
        outlineAnimator.baseWidth = baseRimWidth;
        outlineAnimator.tinkleWidth = tinkleRimWidth;
      }

      for (int i = 0; i < trigger.Length; ++i)
        trigger[i].index = i;
    }

    void OnEnable()
    {
      Addressables
          .LoadAssetAsync<Settings.PlayerSettings>(Settings.PlayerSettings.PATH)
          .Completed += handle => settings = handle.Result;

      _internalCapacity = internalCapacity / 1000f; // mL to L
      _extraPush = extraPush / 1000f; // mL to L
      container = new ResizableContainer(_internalCapacity);

      outlineAnimator = new OutlineAnimator(targetMesh);
      outlineAnimator.baseWidth = baseRimWidth;
      outlineAnimator.tinkleWidth = tinkleRimWidth;
    }

    public void SetMode(InteractionType mode) { this.mode = mode; }

    public void Select()
    {
      switch (mode)
      {
        case InteractionType.Click:
          outlineAnimator.Select();
          break;
        case InteractionType.Action:
          panEnabled = true;
          break;
      }
    }

    public void Unselect()
    {
      switch (mode)
      {
        case InteractionType.Click:
          outlineAnimator.Unselect();
          break;
        case InteractionType.Action:
          panEnabled = false;
          break;
      }
    }

    public void Grab() { outlineAnimator.Grab(); }
    public bool Activate()
    {
      if (!ready) return true;
      target = Interactor.FindNearestToCamera<Recipient>(settings);
      outlineAnimator.Select();
      return target == null;
    }
    public void Deactivate() { outlineAnimator.Unselect(); }

    // Only called on Action mode
    public void Pan(Vector2 delta)
    {
      if (ready && panEnabled && trigger.Length > 0)
      {
        // Nothing to update
        if (delta.x == 0) return;

        TriggerStep currStep;
        if (delta.x > 0)
        { // Moving Up
          triggerPosition += -delta.x; // No resistance going up
        }
        else
        { // Moving Down
          currStep = GetTriggerStep(triggerPosition);
          triggerPosition += -delta.x * currStep.sensitivity; // TODO: Needs scaling
        }

        currStep = GetTriggerStep(triggerPosition);
        float oldValue = triggerValue;

        if (trigger.Length == currStep.index + 1)
          triggerValue = currStep.value;
        else
        {
          TriggerStep nextStep = trigger[currStep.index + 1];
          float weight = (triggerPosition - currStep.position) / (nextStep.position - currStep.position);
          triggerValue = (1 - weight) * currStep.value + weight * nextStep.value; // value interpolation
        }

        // in Liters
        float depression = (Mathf.Clamp(triggerValue, 0, 1) - Mathf.Clamp(oldValue, 0, 1)) * desiredVolume;
        depression += (Mathf.Max(1f, triggerValue) - Mathf.Max(1f, oldValue)) * _extraPush;

        if (depression > 0)
        { // Expell
          List<Volume> internalFlow = container.Reduce(depression, FlowDirection.OUT);
          List<Volume> expelled = new List<Volume>();
          internalFlow.ForEach(volume => expelled.AddRange(tip.Transfer(volume, FlowDirection.OUT)));
          List<Volume> spills = new List<Volume>();
          expelled.ForEach(volume => spills.AddRange(target.Transfer(volume, FlowDirection.IN)));
          // TODO: manage spills (prolly with a particle system and a shader)
        }
        else if (depression < 0)
        { // Withdraw
          List<Volume> withdrawal = target.Transfer(new Volume(-depression, Gas.AIR), FlowDirection.OUT);
          List<Volume> internalFlow = new List<Volume>();
          withdrawal.ForEach(volume => internalFlow.AddRange(tip.Transfer(volume, FlowDirection.IN)));
          internalFlow.ForEach(volume => container.Expand(volume, FlowDirection.IN));
        }
      }
    }

    private TriggerStep GetTriggerStep(float position)
    {
      TriggerStep currStep = default(TriggerStep);
      foreach (TriggerStep step in trigger)
        if (step.position > position)
          break;
        else
          currStep = step;

      return currStep;
    }


    // Interactor setup
    public Type GetInteractor() { return typeof(ToolInteractor); }
    public Transform GetTransform() { return transform; }
    public InteractionType GetSupportedInteractions() { return InteractionType.Click | InteractionType.Action; }
  }
}