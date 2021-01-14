# Scripts
We will document the general structure of the project and purpose of individual scripts using this README format.

Each folder inside `scripts` will have a `README` file, which briefly describes the folder itself and each of it's files.

Most modern IDEs already have a markdown reader. If yours doesn't, google how to read these files (I'm sure there's an extension for it).

## Design Patterns
Generally speaking, one might encounter problems when trying to use variables or call methods in another component. I'll call the former a `data sharing` issue and the later an `event calling` issue.

### Data Sharing
The problem with accessing other component's variables is getting it's instance. Eventhough it's not an impossible task, usually requires adding extra code and configuration to get that instance, which is far from ideal.

Our solution will be making the data owner responsable of making it accessible.
If the value of the variable is meant to be shared across all component instances, or there will only ever be one instance of the component, then make the variable static. As these variables are going to be shared, we will also group them in a separate file. This file will be a `SettingsFile` when the data is not meant to change at runtime, and a `SharedDataFile` otherwise (both concepts are made-up). `SettingsFiles` are a `ScriptableObject`, while `SharedDataFiles` are just a class with static fields and a default value. Ideally, there will be only one component which modifies the value of a given variable in a `SharedDataFile`. We will call that component the `owner` of the data.

If the variable to be shared is meant to be independient for each instance of the component, then you must send the data inside an event.

### Event Calling
We will use message beans from the Microservices pattern, where each component is a service on it's own.

A component might never invoke a method from another one directly (Break this pattern if needed, these are just guidelines which help with complex scenarios but run short on simpler ones). Instead, the caller component should send a message to a channel common to all components, and the target component should listen to the message.

You can check our implementation details on the [events documentation](Event/README.md).

This pattern simplifies a lot some scenarios and further networking on the game. Despite this, you are supposed to break it on trivial scenarios.

### Mechanics
See [mechanics documentation](Mechanics/README.md).

<br>

## Files
### GameInfo
SharedDataFile which contains information about the current state of the game.
