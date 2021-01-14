using System.Collections.Generic;
using EduLabs.Volumetrics;
using UnityEngine;

namespace EduLabs.Mechanics.Recipients
{
  using Drag;

  public class GenericRecipient : GenericDraggable, Recipient
  {

    [Header("Containment")]
    [Rename("Capacity (L)")]
    public float capacity;

    private Container container;

    void OnEnable()
    {
      container = new Container(capacity);
    }

    public List<Volume> Transfer(Volume volume, FlowDirection flow)
    {
      return container.Transfer(volume, flow);
    }
  }
}