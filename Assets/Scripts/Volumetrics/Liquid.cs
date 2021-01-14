using System;
using UnityEngine;

namespace EduLabs.Volumetrics
{
  [Serializable]
  public class Liquid : Substance
  {

    public static readonly Liquid WATER = new Liquid(997, new Color(.9f, .9f, .9f, .15f));

    public Liquid(float density, Color color) : base(density, color, MatterState.LIQUID) { }
  }
}