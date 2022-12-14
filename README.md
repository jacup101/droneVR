# Offbeat VR
Hello, welcome to Offbeat VR, my senior comprehensive project, currently completed for the Fall 2022 Semester. Still plenty more to go for this game, but it is in a good prototype state right now :)

Offbeat is a rhythm game where, instead of hitting the notes, you avoid them. This creates a bullet-hell style game that is difficult, but fun and immersive for players. For more information on the game and its development process, check out the corresponding paper written about it [here](https://github.com/jacup101/comps).

![picture of offbeat](https://github.com/jacup101/droneVR/blob/main/Readme%20Images/game.png)

# Technical Features
Offbeat is made using Unity and developed for the Meta Quest 2 platform using the Oculus SDK integration. The game is split into several different systems, which work together to run the different components of the game. This separation keeps the codebase organization and also allows for customization of levels, since systems can be turned on or off at will. Such systems include:
- **Rhythm System**: handles the spawning of notes and visual events in time with the music
- **Corridor Handler**: responsible for spawning the corridor which surrounds the player, doing so in an efficient way
- **Collision Predictor**: predicts when the player is on a collision path, and notifies the player
- **Health System**: keeps track of the player's health, and notifies them via a warning sign in the HUD
- **Collision System**: detects when the player collides with a note, and works with health system to take appropriate action
- **Motion Controls**: tracks the position of the controllers in relation to the headset, and uses deltas to move the player around
- **Controller Controls**: also runs the barrel roll.

Although this not a full diagram documenting every process of the game, the general flow of how the different systems interact with each other can be found below:
![design diagram of offbeat](https://github.com/jacup101/droneVR/blob/main/Readme%20Images/design.jpg)


# Repro Instructions
This game was made using Unity, and built specfically for Oculus Quest 2. Reproduction instructions found below are meant for the Quest 2 platform, but can technically be used to build for PCVR. However, please note that there are known bugs with PCVR that currently are not planned to be fixed.

1. Install Unity version [2021.2.19f1](https://download.unity3d.com/download_unity/602ecdbb2fb0/UnityDownloadAssistant-2021.2.19f1.exe) (Note that newer Unity versions will probably work, but this version is the one used to develop and is guaranteed to work)
2. Set up Unity for developing with the Quest 2 platform, following the instructions [here](https://developer.oculus.com/documentation/unity/book-unity-gsg/)
3. Set up the Quest 2 headset for development, following the instructions [here](https://developer.oculus.com/documentation/unity/unity-enable-device/)
4. Create a new Unity project, clone this repo, and place the contents of this repo into the Assets folder of the new project. You may have to install a couple of plugins, including the Text Mesh Pro plugin (although Unity should already do this automatically), as well as the [Oculus Integration SDK](https://assetstore.unity.com/packages/tools/integration/oculus-integration-82022) (although this is already included in Assets, so also may not be necessary)
5. Follow the instructions [here](https://developer.oculus.com/documentation/unity/unity-conf-settings/) to set up a few final settings for the project
6. With this, you should be able to connect your Quest 2 to your computer through USB, build, and flash the game!

# Other Notes
- There is a lot of code in this repo not written by me, which comes from the Oculus Integration SDK. Code written by me can be found in the Scripts and Tools directories. There are also many directories containing work done by me which is not code.
- As mentioned above, there are known bugs running the game with PCVR
- Level design is possible, but not streamlined at the moment. Further work will be done on this soon.