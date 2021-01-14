using System;
using UnityEngine;

namespace EduLabs.Mechanics.Interaction
{
  public interface Interactuable
  {

    Transform GetTransform();

    Type GetInteractor();

    InteractionType GetSupportedInteractions();

  }
}
