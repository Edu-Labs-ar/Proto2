using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

namespace EduLabs.Camera
{
  using Settings;
  public class CameraController : MonoBehaviour
  {

    private PlayerSettings settings;

    private Player.PlayerMotor motor;


    void OnEnable()
    {
      Addressables
          .LoadAssetAsync<PlayerSettings>(PlayerSettings.PATH)
          .Completed += handle => settings = handle.Result;

      motor = GetComponentInParent<Player.PlayerMotor>();
      HideCursor();
    }

    public void HideCursor()
    {
      Cursor.lockState = CursorLockMode.Locked;
    }

    public void ShowCursor()
    {
      Cursor.lockState = CursorLockMode.None;
    }


    public void DoLook(CallbackContext ctx)
    {
      if (settings == null || ctx.phase != InputActionPhase.Performed || Player.PlayerStatus.playerFreeze) return;
      Vector2 delta = ctx.ReadValue<Vector2>() * settings.sensitivity;

      float pitch = CheckPitch(delta.y);
      float yaw = 0;

      switch (CameraStatus.mode)
      {
        case CameraMode.ATTACHED:
          motor.TurnAround(delta.x);
          break;
        case CameraMode.UNATTACHED:
          yaw = CheckYaw(delta.x);
          break;
      }

      transform.eulerAngles += new Vector3(-pitch, yaw);
    }

    private float CheckPitch(float delta)
    {
      float currRot = transform.rotation.eulerAngles.x;
      currRot = currRot > 180 ? 360 - currRot : -currRot;
      Vector2 minMax = CameraStatus.pitchConstraint;
      return Mathf.Clamp(delta, minMax.x - currRot, minMax.y - currRot);
    }

    private float CheckYaw(float delta)
    {
      float currRot = transform.rotation.eulerAngles.y;
      currRot = currRot > 180 ? currRot - 360 : currRot;
      Vector2 minMax = CameraStatus.yawConstraint;
      return Mathf.Clamp(delta, minMax.x - currRot, minMax.y - currRot);
    }

  }
}