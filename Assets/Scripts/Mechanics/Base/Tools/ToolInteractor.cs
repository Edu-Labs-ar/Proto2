using UnityEngine;

namespace EduLabs.Mechanics.Tools
{
  using Player;
  using Interaction;
  public class ToolInteractor : AbstractInteractor
  {
    private Inventory inventory;
    private Tool actionable;
    private Settings.PlayerSettings settings;
    private InteractionType interactionType;

    private bool active = false;


    public ToolInteractor(
        Transform transform, Settings.PlayerSettings settings,
        Interactuable interactuable, InteractionType type)
    {
      actionable = (Tool)interactuable;
      inventory = transform.GetComponent<Inventory>();
      this.settings = settings;
      this.interactionType = type;
    }

    public override bool OnClickStart()
    {
      switch (interactionType)
      {
        case InteractionType.Click:
          if (inventory.HasSpace())
          {
            actionable.Select();
            actionable.SetMode(InteractionType.Click);
            return false;
          }
          return true;
        case InteractionType.Action:
          actionable.Select();
          return false;
        default:
          return true;
      }
    }

    public override bool OnClickEnd()
    {
      actionable.Unselect();
      return interactionType != InteractionType.Action;
    }

    public override bool ActionStart(Hand hand)
    {
      switch (interactionType)
      {
        case InteractionType.Click:
          if (inventory.HasSpace(hand))
          {
            actionable.Grab();
            inventory.Grab(actionable, hand, false);
            return true;
          }
          return false;
        case InteractionType.Action:
          if (!active)
          {
            active = true;
            actionable.SetMode(InteractionType.Action);
            actionable.Activate();
            return false;
          }
          else
          {
            actionable.Deactivate();
            return true;
          }
      }
      return false;
    }

    public override bool ActionEnd(Hand hand)
    {
      return false;
    }

    public override void DoPan(Vector2 delta)
    {
      if (interactionType == InteractionType.Action) {
        actionable.Pan(delta);
      }
    }
  }
}