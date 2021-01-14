using System;
using UnityEngine;

namespace EduLabs.Volumetrics
{

  [Serializable]
  public class Gas : Substance
  {
    public static readonly Gas AIR = new Gas(1.225f, new Color(0, 0, 0, 0));

    public Gas(float density, Color color) : base(density, color, MatterState.GAS) { }
  }
}