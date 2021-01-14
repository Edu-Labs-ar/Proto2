# Mechanics
Holds most of the game's logic.

A mechanic is a module which provides certain features to the game.

Each mechanic's structure is splitten into Actors, Attributes and Functionalities. All mechanics must have atleast an Attribute and it's related functionality.
It's also possible for a mechanic to lack it's functionality. In that case, the mechanic will be useless by itself and would serve as a base abstraction for other mechanics to complete.

> Disclaimer: If you don't care much about pattern names, skip this part
> I don't know if this pattern has a name, it ressambles to an [MVVM](https://en.wikipedia.org/wiki/Model-view-viewmodel), where attributes would be the ViewModel, Functionalities the View and Actors the Model, but here the functionalities also act as a controler and Actors are not always necessary.

Also note that each module can have more classes which don't fit this pattern if needed. As an example, the `Interaction` module has two extra classes: `InteractionType` and `AbstractInteractor`.

### Naming
Mechanic modules should be named as nouns. In most cases it will be the noun form of it's attribute.
If the attribute and module's name are the same, the module must be in plural to prevent naming conflicts.

## Attributes
An attribute is an interface which defines the behaviours it's functionality must implement. They also serve as a markup interface to find if a given object has a certain functionality.

### Naming
They must be named after an object, or an attributive adjective. Regarding the later, it seems like there's no name for the exact type of adjective, as not all attributive adjectives apply. Think of it as if the mechanic was a property, and the attribute being the thing which embeds that property into your target object.
This can be more easily understood by example:
- As an object: Machine, Recipient, Flag, Ball
- As an attribute: Interactuable, Activable, Attackable, Indicative, Attributable
- Also accepted: Bouncy, Slippery


## Functionalities
Realizations of attributes. Functionalities implement the behaviour related to an object.
Note this isn't enough as most mechanics will need to communicate with other objects, this is what actors are for.
From the functionality's point of view, it should care only for the current object and trust another component to invoke it's functions when needed.

### Naming
Functionalities don't have any restriction regarding it's name. In most scenarios you would try to have certain similitude to the attribute you're using, but it isn't needed.

## Actors
Opposed to attributes functionalities, actors usually aren't in the target component.
Actors are the responsibles of coordinating functionalities from different objects.
Actors use attributes as markups to find all the implementations available. Most of times they are the input of the module.

### Naming
Actors must be named as a noun. All actors attribute a role to it's target. Think about the name of that role and spell it as a noun.
Examples: Dragger, Bouncer, Activator, Worker
