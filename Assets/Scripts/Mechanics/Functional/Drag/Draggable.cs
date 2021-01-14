using UnityEngine;

namespace EduLabs.Mechanics.Drag
{
  using Interaction;
  public interface Draggable : Interactuable
  {
    void Select();

    void Unselect();

    void Drag(Vector2 delta);

    void Grab();
  }
}
