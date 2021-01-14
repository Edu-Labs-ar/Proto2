using UnityEngine;

namespace EduLabs.Mechanics.Tools
{
  using Interaction;
  public interface Tool : Interactuable
  {

    void Select();

    void Unselect();

    void Grab();

    bool Activate();

    void Deactivate();

    void Pan(Vector2 delta);

    void SetMode(InteractionType mode);
  }
}