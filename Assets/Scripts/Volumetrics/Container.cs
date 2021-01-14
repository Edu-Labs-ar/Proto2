using System.Collections.Generic;
using Nito.Collections;

namespace EduLabs.Volumetrics
{
  // Assumes unvariable pressure
  // Does not account for internal currents due to differences in densities
  // Does not account for evaporation (Transfers due to envorinment's density)
  public class Container
  {

    protected float capacity;

    // First element = bottom,
    // Last element = top
    protected Deque<Volume> contents = new Deque<Volume>();

    public Container(float capacity)
    {
      this.capacity = capacity;
      contents.AddToBack(new Volume { amount = capacity, content = Gas.AIR });
    }

    // For now I'll start all transfers from the bottom of the container.
    // We might need to add the height at which the transfer occurs in a future.
    public List<Volume> Transfer(Volume volume, FlowDirection flow)
    {
      if (flow == FlowDirection.IN)
        contents.AddToBack(volume);
      else
        contents.AddToFront(volume);

      return Spill(volume.amount, flow);
    }


    // Inbound flow spills from top
    // (Imagine adding volume to the bottom of the container, it will spill from the top)
    //
    // Outbound flow spills from bottom
    // (Imagine removing volume from the bottom of the container, you're substracting (spilling) from the bottom)
    protected List<Volume> Spill(float amount, FlowDirection flow)
    {
      List<Volume> spills = new List<Volume>();
      Volume content;
      while (amount > 0 && (content = (flow == FlowDirection.IN ? contents.PeekFront() : contents.PeekBack())) != null)
      {
        if (amount >= content.amount)
        {
          if (flow == FlowDirection.IN)
            contents.RemoveFromFront();
          else contents.RemoveFromBack();
          spills.Add(content);
          amount -= content.amount;
        }
        else
        {
          spills.Add(content.Split(amount));
          amount = 0;
        }
      }

      return spills;
    }
  }
}