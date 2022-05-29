# Yet Another Engine
## Description
This is a simple learning-purpose game engine implementation.

It uses YaECS for cross-system communication, BulletSharp for physics,
Microsoft implementations of DI, Configuration and Logging,
and Silk.NET bindings for everything else
## Structure
### Bootstrapper
Main entry point is the Bootstrapper, it retrieves the window context,
world and scene managers from the DI, runs the window and proxies
window callbacks to the world manager
### WorldManager
The world manager has references to the default systems,
it creates ECS Worlds, initializes, updates and disposes them
### World
World is a ECS world that executes ECS Systems over it's entities
and their components.
There could be different instances of worlds for Rendering,
Model or Physics but they could share a context either
### SceneManager
Scene manager holds the list of available scene providers and commands
the world manager to change a scene
