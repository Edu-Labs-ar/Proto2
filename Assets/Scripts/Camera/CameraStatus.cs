using UnityEngine;

namespace EduLabs.Camera {

  public enum CameraMode { ATTACHED, UNATTACHED }

  public class CameraStatus {

    public static CameraMode mode = CameraMode.ATTACHED;

    public static Vector2 yawConstraint = new Vector2(-120f, 120f);
    public static Vector2 pitchConstraint = new Vector2(-88f, 88f);
  }
}