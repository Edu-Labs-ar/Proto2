using System;
using UnityEngine;

namespace EduLabs.Volumetrics
{
  [Serializable]
  public abstract class Substance
  {
    // On reality, density depends on temperature and pressure.
    // Let's assume it constant and see how it works.
    public float density;
    public Color color;
    public MatterState state;

    public Substance(float density, Color color, MatterState state)
    {
      this.density = density;
      this.color = color;
      this.state = state;
    }
  }
}