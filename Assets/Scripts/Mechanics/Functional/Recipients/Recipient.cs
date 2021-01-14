using System.Collections.Generic;

namespace EduLabs.Mechanics.Recipients
{
  using Volumetrics;
  using Drag;
  public interface Recipient : Draggable
  {

    List<Volume> Transfer(Volume volume, FlowDirection flow);

  }
}