# Disclaimer
This project was developed for educational purposes during my studies at AIV (Accademia Italiana Videogiochi).
It makes use of proprietary libraries provided by the academy "AIV Fast2D" and "AIV Fast3D" which are not publicly available and are the property of AIV.
Therefore, these libraries and their source code are not included in this repository.
Only the scripts and assets I created are shared here to demonstrate my understanding of how a game engine works.
# Project Content
## Fast2D
### Boids
A visual simulation of flocking behavior inspired by birds in nature.
Each boid follows three simple rules: alignment, cohesion, and separation; resulting in smooth, lifelike group movement.
When you run the project, you can left-click to spawn new boids. As more are added, they begin reacting to one another, adjusting their direction and spacing dynamically to stay together and avoid collisions.
### Car2D
A simple driving simulation focused on realistic vehicle movement in a 2D space.
The car responds to acceleration, friction, and steering, simulating front and rear wheel movement to create a believable turning behavior.
When you run the project, you can drive using WASD, accelerating, braking, and steering as the car smoothly rotates and slides according to its speed and wheelbase.
>The car can go off screen
### Heads
A local multiplayer game prototype where two players must escape from an intelligent enemy in a maze-like environment.
The enemy uses a Finite-State Machine combined with pathfinding logic to patrol, chase, and attack. When a player enters its field of view, it locks on and shoots projectiles; once the player escapes, the enemy returns to patrolling and resumes its path.
Players can collect power-ups scattered around the map for temporary advantages, with support for both keyboard and joypad controls (WASD for Player 1, arrow keys for Player 2).
### Space Shooters
A classic-style 2D space shooter where the player pilots a ship that must survive waves of enemies while dodging and firing back.
Enemies appear at random intervals from the right side of the screen, shooting bullets toward the player as they move left across the stage.
The player can move using WASD and aim or shoot using the mouse cursor. Power-ups occasionally drift in from the same direction: some restore health, while others enhance firepower, allowing the player to shoot three bullets at once.
### Tanks
A local multiplayer turn-based tank battle where two players take turns moving, aiming, and firing under time pressure.
Each player controls their tank using dedicated keys: 
- Player 1: move with A/D, aim with W/S, and shoot with Space;
- Player 2: move with the arrow keys, aim with up/down, and shoot with Right Ctrl.
Every turn is limited by a timer, encouraging quick thinking and precision before the opponent’s counterattack. The project also features post-processing effects.
## Fast3D
### Meshes
An introductory 3D example exploring how meshes work within the AIV Fast3D framework.
The project loads and displays a simple 3D object (a triangle mesh) and demonstrates how vertices, position, scale, and pivot points affect its rendering.
### Heightmap
A 3D project demonstrating how heightmaps can be converted into meshes to create terrain-like surfaces.
The program loads a grayscale image where pixel brightness determines vertex height, generating a textured or wireframe mesh.
You can move the camera freely with WASD/QE and look around with the mouse, exploring the 3D terrain from any angle. Press 1 for textured mode or 2 for wireframe mode to see the underlying geometry.
### Materials
A 3D demonstration of material properties, textures, and lighting on 3D meshes.
The project shows how diffuse, normal, specular, emissive, and reflection maps affect rendering using Phong shading.
You can move the camera freely with WASD/QE and look around with the mouse, observing how the materials respond to lighting and rotation. The scene features a textured wall and a rotating R2D2 model, allowing a hands-on exploration of material effects in 3D.
