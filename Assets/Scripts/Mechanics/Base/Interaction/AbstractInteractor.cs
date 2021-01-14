using UnityEngine;

namespace EduLabs.Mechanics.Interaction
{
  public abstract class AbstractInteractor
  {

    public abstract bool OnClickStart();
    public abstract bool OnClickEnd();
    public abstract bool ActionStart(Player.Hand hand);
    public abstract bool ActionEnd(Player.Hand hand);
    public abstract void DoPan(Vector2 delta);

  }
}