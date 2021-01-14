using UnityEngine;

namespace EduLabs.Settings {
  [CreateAssetMenu(fileName = "GameSettings", menuName = "EduLabs/GameSettings", order = 1)]
  public class GameSettings : ScriptableObject {

    public const string PATH = "EduLabs/GameSettings";

    [Header("Tinkling")]
    [Range(0f, 1.5f)]
    public float tinkleDuration = .2f;

    [ColorUsage(false, true)]
    public Color baseColor = Color.white;

    [ColorUsage(false, true)]
    public Color focusedColor = Color.white;

    [ColorUsage(false, true)]
    public Color selectColor = Color.cyan;

    [ColorUsage(false, true)]
    public Color grabColor = Color.green;

    [ColorUsage(false, true)]
    public Color unselectColor = Color.red;

  }
}