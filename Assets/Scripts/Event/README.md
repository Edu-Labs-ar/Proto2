# Event

In our implementation, a message bean is an `EventType` with optional extra data (which should be serializable).

A component can suscribe to an event by implementing the `EventListener` interface and registering itself at the `EventsManager`.

A component can invoke an event by calling `EventsManager.TriggerEvent`.

## Files
### EventListener
Interface to listen to an event.
The `ListenEvent` function will be called for any event, you must make sure to discard the events you don't care about.

### EventsController
This class is responsible for triggering events when it's unclear who should trigger them.

### EventsManager
This is a common channel accessible by all components, meant for different components to interact with each other.

### EventType
An enum which lists all the events in the system.
Feel free to add as many events as you need.
