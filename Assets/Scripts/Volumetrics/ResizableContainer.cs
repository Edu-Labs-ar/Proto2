using System.Collections.Generic;

namespace EduLabs.Volumetrics {
  public class ResizableContainer : Container {

    public ResizableContainer(float capacity) : base(capacity) { }

    public List<Volume> Reduce(float shrinkAmount, FlowDirection flow)
    {
      capacity -= shrinkAmount;
      return Spill(shrinkAmount, flow);
    }

    public void Expand(Volume volume, FlowDirection flow) {
      if (flow == FlowDirection.IN)
        contents.AddToBack(volume);
      else
        contents.AddToFront(volume);
      capacity += volume.amount;
    }
  }
}