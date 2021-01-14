using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

namespace EduLabs.Player
{
  using Settings;
  using Event;
  using static Event.EventType;

  [RequireComponent(typeof(Rigidbody))]
  public class PlayerMotor : MonoBehaviour, EventListener
  {

    private PlayerSettings settings;

    private Rigidbody rb;

    private Vector3 currVelocity;

    private bool running = false;

    private Camera.CameraController cameraController;


    private float currSpeed
    {
      get { return running ? settings.runSpeed : settings.walkSpeed; }
    }

    private Vector3 effectiveVelocity
    {
      get
      {
        return currVelocity.z >= 0 ? currVelocity
            : new Vector3(currVelocity.x, currVelocity.y, currVelocity.z * settings.backPenalty);
      }
    }


    void OnEnable()
    {
      Addressables
          .LoadAssetAsync<PlayerSettings>(PlayerSettings.PATH)
          .Completed += handle => settings = handle.Result;

      rb = GetComponent<Rigidbody>();
      cameraController = GetComponentInChildren<Camera.CameraController>();

      EventsManager.RegisterListener(this);
    }

    public void ListenEvent(Event.EventType type, object message)
    {
      switch (type)
      {
        case PAUSE_GAME:
        case SELECTION_START:
          Player.PlayerStatus.playerFreeze = true;
          cameraController.ShowCursor();
          break;
        case RESUME_GAME:
        case SELECTION_END:
          Player.PlayerStatus.playerFreeze = false;
          cameraController.HideCursor();
          break;
      }
    }


    void FixedUpdate()
    {
      if (settings == null || PlayerStatus.playerFreeze) return;

      Vector3 movement = effectiveVelocity * currSpeed;
      rb.velocity = transform.right * movement.x + transform.up * movement.y + transform.forward * movement.z;
    }


    public void DoMove(CallbackContext ctx)
    {
      switch (ctx.phase)
      {
        case InputActionPhase.Performed:
          {
            Vector2 delta = ctx.ReadValue<Vector2>();
            currVelocity = new Vector3(delta.x, 0, delta.y);
          }
          break;
        case InputActionPhase.Canceled:
          currVelocity = Vector3.zero;
          break;
      }
    }

    public void DoRun(CallbackContext ctx)
    {
      switch (ctx.phase)
      {
        case InputActionPhase.Performed:
        case InputActionPhase.Canceled:
          running = ctx.ReadValueAsButton();
          break;
      }
    }

    public void TurnAround(float yaw)
    {
      transform.eulerAngles += new Vector3(0, yaw);
    }
  }
}