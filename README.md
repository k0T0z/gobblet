# Gobblet

https://github.com/k0T0z/gobblet/assets/74428638/20445c0f-7bb9-440d-92bf-a178c99a3e20

## State

- Board with pieces' positions.
- Whose turn it is.

## Number of actions at each state (worst case)

- 3 * 16 = 48 actions: When picking a piece from side boards.
- 8 * 8 = 64 actions: When moving a piece on the board (biggest piece).

# Building the project

## Requirements

### Ubuntu Linux 64-bit

- [Godot_v4.2-stable](https://github.com/godotengine/godot/releases/download/4.2-stable/Godot_v4.2-stable_mono_linux_x86_64.zip).
- Installing .NET on Ubuntu has some problems but this answer here [unable-to-locate-package-dotnet-sdk-8-0](https://stackoverflow.com/questions/77498786/unable-to-locate-package-dotnet-sdk-8-0) helped me a lot.
- See also for other Linux distributions [dotnet/core/install/linux](https://learn.microsoft.com/en-gb/dotnet/core/install/linux).

### Windows

- [Godot_v4.2-stable](https://github.com/godotengine/godot/releases/download/4.2-stable/Godot_v4.2-stable_mono_win64.zip).
- [.NET 8.0 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/thank-you/sdk-8.0.100-windows-x64-installer).

## Building



### Windows

1. Download and install the requirements.
2. Open the project in Godot.
3. Run the project.

# Rules



# Screenshots



# References

1. [Game Rules](https://www.boardspace.net/gobblet/english/gobblet_rules.pdf).
2. [Gather Together Games Gobblet Presentation](https://www.youtube.com/watch?v=aSaAjQY8_b0).
3. [These fresh Godot tutorials by Alphredo M were very helpful to me](https://www.youtube.com/watch?v=fW7_0uBHsBw&list=PLrx2VhSBm-FcO9TeLv2fUg6VxO43jkM4Y).
