using System.Collections.Generic;
using UnityEngine;

namespace EduLabs.Mechanics.Micropipettes
{
  using Volumetrics;
  using Drag;
  public class GenericTip : GenericDraggable, Tip
  {

    [Header("Settings")]
    [Rename("Capacity (mL)")]
    public float capacity;

    private float internalCapacity;

    private Container container;


    void OnEnable()
    {
      internalCapacity = capacity / 1000f; // mL to L

      container = new Container(internalCapacity);
    }


    public List<Volume> Transfer(Volume volume, FlowDirection flow)
    {
      return container.Transfer(volume, flow);
    }
  }
}