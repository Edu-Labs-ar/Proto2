# Player
Set of functions which are bound to the player and would have no sense without him.

## Files
### Hand
Meant to distinguish between `Left` and `Right` player's hand.

### Inventory
Holds player's items. Currently the only slots for items are both hands, might be further expanded to wear lab coats, glasses, etc.

### PlayerMotor
Controls all player related movement (rotation + translation). Camera movement (`CameraController`) and player interaction (`Interactor`) are managed separatedly.

### PlayerStatus
SharedDataFile which contains information about the current state of the player.
