using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

namespace EduLabs.Event
{
  public class EventsController : MonoBehaviour
  {

    public void DoPause(CallbackContext ctx)
    {
      switch (ctx.phase)
      {
        case InputActionPhase.Started:
          if (GameInfo.isRunning)
          {
            EventsManager.TriggerEvent(Event.EventType.PAUSE_GAME);
            GameInfo.isRunning = false;
          }
          else
          {
            EventsManager.TriggerEvent(Event.EventType.RESUME_GAME);
            GameInfo.isRunning = true;
          }
          break;
      }
    }

  }
}