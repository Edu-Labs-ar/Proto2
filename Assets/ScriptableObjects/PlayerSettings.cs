using UnityEngine;

// TODO: Mover a GameSettings
namespace EduLabs.Settings {
  [CreateAssetMenu(fileName = "Settings", menuName = "EduLabs/PlayerSettings", order = 1)]
  public class PlayerSettings : ScriptableObject {

    public const string PATH = "EduLabs/PlayerSettings";

    [Header("Movement")]
    [Range(0.5f, 5f)]
    public float walkSpeed = 1.3f;

    [Range(0.5f, 5f)]
    public float runSpeed = 2f;

    [Range(0.5f, 1f)]
    public float backPenalty = .9f;

    [Range(0.25f, 2f)]
    public float sensitivity = .6f;

    [Header("Gameplay")]
    [Range(0f, 0.5f)]
    public float selectThreshold = .2f;
    [Range(0f, 0.5f)]
    public float micropipetteSensitivity = .3f;
  }
}