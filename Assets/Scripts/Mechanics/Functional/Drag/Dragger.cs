using UnityEngine;

namespace EduLabs.Mechanics.Drag
{
  using Player;
  using Interaction;
  public class Dragger : AbstractInteractor
  {

    private Inventory inventory;
    private Draggable selection;


    public Dragger(
        Transform transform, Settings.PlayerSettings settings,
        Interactuable interactuable, InteractionType type)
    {
      inventory = transform.GetComponent<Inventory>();
      selection = (Draggable)interactuable;
    }

    public override bool OnClickStart()
    {
      if (inventory.HasSpace())
      {
        selection.Select();
        return false;
      }
      return true;
    }

    public override bool OnClickEnd()
    {
      selection.Unselect();
      return true;
    }

    public override void DoPan(Vector2 delta)
    {
      selection.Drag(delta);
    }

    public override bool ActionStart(Hand hand)
    {
      if (inventory.HasSpace(hand))
      {
        selection.Grab();
        inventory.Grab(selection, hand, true);
        return true;
      }
      return false;
    }

    public override bool ActionEnd(Hand hand)
    {
      return false;
    }
  }
}