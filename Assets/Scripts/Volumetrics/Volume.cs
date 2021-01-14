using System;

namespace EduLabs.Volumetrics
{

  [Serializable]
  public class Volume
  {

    [Rename("Amount (L)")]
    public float amount;

    public Substance content;

    public Volume() { }

    public Volume(float amount, Substance content)
    {
      this.amount = amount;
      this.content = content;
    }


    /// <summary>
    /// Reduces this volume by <paramref name="amount"/>, and returns a new volume with the withdrawn one
    /// </summary>
    /// <param name="amount">The volume to be taken from this volume.</param>
    public Volume Split(float amount)
    {
      this.amount -= amount;
      return new Volume { amount = amount, content = content };
    }


    /// <summary>
    /// Expands this volume by <paramref name="amount"/>
    /// </summary>
    /// <param name="amount">The volume to be added to this volume.</param>
    public void Merge(float amount) { this.amount += amount; }
  }
}