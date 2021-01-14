# Animation
Some special effects require a lot of interaction between shaders, DOTween, unity animations and custom code.
As these effects are generally an addon rather to being related to an object's functions, they're separated into this folder.

## Files
### AnimationUtils
Configures each animation's sequence (interacting with DOTween).

### OutlineAnimator
Controller which handles the `OutlineShader` to display outline animations.
Also reads the effect's properties from `GameSettings`.
