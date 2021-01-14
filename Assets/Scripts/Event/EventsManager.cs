using System.Collections.Generic;
namespace EduLabs.Event
{

  public class EventsManager
  {

    private static List<EventListener> listeners = new List<EventListener>();

    public static void TriggerEvent(EventType type)
    {
      TriggerEvent(type, null);
    }

    public static void TriggerEvent(EventType type, object message)
    {
      if (GameInfo.isRunning || type == EventType.RESUME_GAME)
        listeners.ForEach(listener => listener.ListenEvent(type, message));
    }

    public static void RegisterListener(EventListener listener)
    {
      listeners.Add(listener);
    }

    public static void UnregisterListener(EventListener listener)
    {
      listeners.Remove(listener);
    }
  }
}