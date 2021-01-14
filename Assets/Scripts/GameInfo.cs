using UnityEngine;

namespace EduLabs
{
  public class GameInfo
  {
    // ADDRESSABLES
    public const string ADDRESSABLE_OUTLINE = "EduLabs/Outline";
    public const string ADDRESSABLE_SILHOUETTE = "EduLabs/Silhouette";

    public static readonly int environmentLayer = LayerMask.NameToLayer("Environment");

    public static bool isRunning = true;
  }
}