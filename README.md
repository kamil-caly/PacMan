# Pac-Man Game

## Game Description

A classic Pac-Man game where the player moves through a maze, collecting small and large dots while avoiding ghosts. The goal is to earn as many points as possible by collecting dots and eating ghosts while avoiding losing all lives.

## Controls

- Pac-Man moves in four directions: up, down, left, and right.

## Game Rules

### Small Dots
- Pac-Man earns points by eating small dots scattered throughout the maze.
- **Point value**: 10 points per small dot.

### Large Dots
- After eating a large dot, Pac-Man gains the temporary ability to eat ghosts.
- Ghosts turn blue and start fleeing from Pac-Man.
- **Point value of a large dot**: 50 points.
- **Point value for eating a ghost**: 200 points per ghost.

### Ghosts
There are four ghosts in the maze, each with a unique movement strategy:

- **Blinky (red ghost)**:
  - Directly chases Pac-Man (within a distance of 12 tiles or less), aiming for his current position.

- **Pinky (pink ghost)**:
  - Tries to predict Pac-Man's movements and attempts to position itself four tiles ahead of Pac-Man in the direction he's moving (within a distance of 12 tiles or less).

- **Inky (blue ghost)**:
  - Inky's target is determined by both Blinky's and Pac-Man's positions. Inky's movements are more unpredictable because his target is a reflection of the distance between Pac-Man and Blinky (within a distance of 
  12 tiles or less).

- **Clyde (orange ghost)**:
  - When far from Pac-Man, Clyde moves toward him, but when within 6 tiles, he changes direction and moves randomly.

#### Ghost Movement Rules:
- Ghosts can only move in four directions: up, down, left, right.
- Ghosts cannot reverse direction and go back to the tile they just came from (except in their starting area).
- At intersections, ghosts choose their direction based on their movement algorithm, or if no algorithm applies, they move randomly.

### Lives
- The player starts with three lives.
- A life is lost when Pac-Man touches a ghost.

### Maze
- The maze includes special tunnels that allow Pac-Man to travel from one side of the maze to the other.

## Victory and Defeat Conditions

- The player wins the level by collecting all dots in the maze.
- The level is lost if the player loses all lives before collecting all the dots.

## Graphics

The graphics used in the game are from another repository, available at this link: [https://github.com/adrianeyre/pacman](https://github.com/adrianeyre/pacman).

## How to Run

To run the game locally, follow these steps:

1. Clone the repository:
   ```bash
   git clone https://github.com/your-username/your-repository.git
2. Open the project in Visual Studio 2022 by selecting the .sln file from the cloned repository.
3. Run the game in debug mode -> ensures smooth gameplay, as running without debugging may cause performance issues and slow down the game significantly.

## Game View
![image](https://github.com/user-attachments/assets/e0a9ef71-d110-4c2e-9b91-053c1108e796)
![image](https://github.com/user-attachments/assets/4340603f-b6d1-46fa-8233-6dc1b926b05f)
