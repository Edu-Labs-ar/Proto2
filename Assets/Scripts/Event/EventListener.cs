namespace EduLabs.Event {

  public interface EventListener {

    void ListenEvent(EventType type, object message);
  }
}