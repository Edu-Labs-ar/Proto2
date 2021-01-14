# Volumetrics
> Note: This feature is still incomplete and it's implementation is subject to several changes.

Naive approximation to flow dynamics.
Eventhough a proper flow simulation would add a lot to game's realism, the computational cost of classic approaches is too expensive for our needs.

So we're using instead an original approach which doesn't take physics into account.

## Principles
- Containers cannot create nor destroy volumes.
- Containers are always full of some volume. "Empty" containers are full of `AIR`.
- Volumes are imaginary divisions. Merging or splitting volumes within a container has no impact as long as internal distributions are mantained (eventhough this isn't true yet as the implementation is incomplete, heterogeneous layers MUST be different volumes for the moment. Heterogeneous mixes are not currently supported).
- Denser substances tend to go below less dense ones (Not yet implemented).

## Files
### MatterState
State of a substance, acceptable values are `LIQUID` or `GAS`.
Solids are NOT supported by this model.
Currently states have no implications, might impact in volume's shading in a future.

### Substance
Labels for particular sets of properties. All substances have (at least) a `MatterState`, density and color.

### Volume
A volume is a substance which occupies space. It can be splitted or merged with another volumes.

### Liquid
Substance in liquid state, has no special behaviour and is separated just to keep things cleaner.

### Gas
Substance in gas state, has no special behaviour and is separated just to keep things cleaner.

### Container
Stores layers of volumes. It's total contents occupy always the same space (equal to the container's capacity). A container's capacity is fixed and not modifiable.

Volumes can enter or leave a container using `Transfers`. All transfers must specify a volume to transfer with, and the direction of the transference.
The input volume will enter the container, which will force previous volumes to leave it. Transfers return all the excedent volumes, which occupy the same space as the volume given as input.

### FlowDirection
Transferences have two endpoints, each specified by it's FlowDirection (`IN` and `OUT`).
An `IN`bound transfer adds the input `Volume` to the transfer point and spills from the top of the container. While an `OUT`bound transfer adds the input Volume to the top of the container and spills from the transfer point.
Usually, an `OUT` transference will take as input some volume from it's environment (i.e `AIR`).

> At the current implementation, transfer points are set fixed to the bottom of the container.

### ResizableContainer
Similar to `Container`s, but it's capacity can be modified with `Reductions` and `Expansions`.

An `Expansion` is a special kind of `Transfer` where no volume is spilled.
A `Reduction` is a special kind of `Transfer` where there's no input `Volume`.