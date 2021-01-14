using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

namespace EduLabs.Mechanics.Interaction
{
  using Player;
  public class Interactor : MonoBehaviour
  {

    public static T FindNearestToCamera<T>(Settings.PlayerSettings settings) where T : Interactuable
    {
      T[] targets = FindObjectsOfType(typeof(T)) as T[];
      T nearest = default(T);
      float distance = 1f;

      Vector2 center = new Vector2(.5f, .5f);
      foreach (T target in targets)
      {
        Vector2 viewPos = UnityEngine.Camera.main.WorldToViewportPoint(target.GetTransform().position);
        viewPos -= center;

        float currDistance = viewPos.sqrMagnitude;
        if (currDistance <= settings.selectThreshold && currDistance < distance)
        {
          nearest = target;
          distance = currDistance;
        }
      }
      return nearest;
    }


    private Inventory inventory;
    private AbstractInteractor interactor;
    private Settings.PlayerSettings settings;

    private bool ready { get { return settings != null; } }


    void OnEnable()
    {
      Addressables
          .LoadAssetAsync<Settings.PlayerSettings>(Settings.PlayerSettings.PATH)
          .Completed += handle => settings = handle.Result;
      inventory = GetComponent<Inventory>();
    }

    public void DoClick(CallbackContext ctx)
    {
      if (!GameInfo.isRunning || !ready) return;
      switch (ctx.phase)
      {
        case InputActionPhase.Started:
          ClickStart();
          break;
        case InputActionPhase.Canceled:
          ClickEnd();
          break;
      }
    }

    public void DoPan(CallbackContext ctx)
    {
      if (!GameInfo.isRunning || !ready || ctx.phase != InputActionPhase.Performed) return;

      if (interactor != null)
        interactor.DoPan(ctx.ReadValue<Vector2>());
    }

    public void ActionLeft(CallbackContext ctx)
    {
      if (!GameInfo.isRunning || !ready) return;
      switch (ctx.phase)
      {
        case InputActionPhase.Started:
          ActionStart(Player.Hand.Left);
          break;
        case InputActionPhase.Canceled:
          ActionEnd(Player.Hand.Left);
          break;
      }
    }

    public void ActionRight(CallbackContext ctx)
    {
      if (!GameInfo.isRunning || !ready) return;
      switch (ctx.phase)
      {
        case InputActionPhase.Started:
          ActionStart(Player.Hand.Right);
          break;
        case InputActionPhase.Canceled:
          ActionEnd(Player.Hand.Right);
          break;
      }
    }


    private void ClickStart()
    {
      if (settings == null) return;
      Interactuable nearest = FindNearestToCamera<Interactuable>(settings);

      // TODO: Play some animation when no target found
      if (nearest != null && (nearest.GetSupportedInteractions() & InteractionType.Click) != 0)
      {
        StartSelection(nearest, InteractionType.Click);

        if (interactor.OnClickStart())
          EndSelection();
      }
    }

    private void ClickEnd()
    {
      if (interactor != null && interactor.OnClickEnd())
        EndSelection();
    }

    private void ActionStart(Player.Hand hand)
    {
      if (interactor != null)
      {
        if (interactor.ActionStart(hand)) EndSelection();
      }
      else if (!inventory.HasSpace(hand) && (inventory.GetItem(hand).GetSupportedInteractions() & InteractionType.Action) != 0)
        StartSelection(inventory.GetItem(hand), InteractionType.Action);
      else
      {
        // TODO: Play some animation when no target found
      }
    }

    private void ActionEnd(Player.Hand hand)
    {
      if (interactor != null && interactor.ActionEnd(hand))
        EndSelection();
    }


    private void StartSelection(Interactuable target, InteractionType type)
    {
      Event.EventsManager.TriggerEvent(Event.EventType.SELECTION_START);
      interactor = (AbstractInteractor)Activator.CreateInstance(
          target.GetInteractor(), new object[] { transform, settings, target, type });
    }

    private void EndSelection()
    {
      Event.EventsManager.TriggerEvent(Event.EventType.SELECTION_END);
      interactor = null;
    }
  }
}