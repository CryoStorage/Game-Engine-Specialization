# Sim Game

## Description
A simple game where you awaken on an island where the inhabitants are tiny and you use your increased stature to help them, in return they reward you with clothing.
The player can chose and customize a character and then walk around a small town and talk to NPCs, pickup wood and sell it in town.

## Controls
- WASD to move / also supports controller
- E to interact/ a on gamepad
- Click interactions on Ui

## Systems
- Event based architecture: The game is built around scriptable objects that can be used to raise events, and a component that listens for these events which then evokes a response through a Unity event. This makes it easy to modify existing behaviours and create new ones through the Unity editor with minimal code.
- GameVariables: A scriptable object that holds a variable of a specified type. This can be used to store variables that need to be accessed by multiple scripts while keeping them decoupled.
- Character customization: The player can choose between different characters and chose between three outfits. Originally i wanted to make the outfits buyable but i didn't have time to implement that.
- Interactable: This abstract class defines an interactable object that can be interacted with by the player. It defines an abstract Interact method abd automatically adds a collider to the object which the PlayerInteraction script can detect to enable and disable interaction through OnTriggerEnter and OnTriggerExit callbacks.
- NPC interaction: The player can talk to NPCs and they will respond. These inherit from the Interactable class and have a dialogue that is displayed when the player interacts with them.
- Pickups: The player can pick up items that are placed in the scene. These inherit from the Interactable class and have a pickup method that is called when the player interacts with them.