# AI-Project

Meow is a game developed as part of a course on AI programming for games. It was developed in Unity.

- [Gameplay](#gameplay)
- [AI Techniques](#ai-techniques)
- [Link to source files](https://github.com/rabbort/AI-Project/tree/master/Assets/Scripts)

## Gameplay

The game takes place in a dungeon, where the player must fight off orcs (ranged) and lizard people (melee). Upon killing all the enemies
within a room, the player can move on and enter a new room with new enemies. After clearing several rooms, the player will level up and cause 
the enemy population to evolve, forcing the player to face harder enemies as time goes on. 

To ensure the player is clearing rooms quickly, a bar fills up slowly over time. Each time the player kills an enemy, the bar empties partially.
If the bar ever reaches its full capacity, the cats overwatching the player will slay the player.

##AI Techniques

- Neural Networks

  Each enemy has their own neural network. These networks were trained until the enemy learned to attack the player upon finding them.
  The networks took in several inputs, consisting of distance from the player, angle to the player, and distance from walls. The data
  for these inputs is obtained via Unity's raycast and overlapsphere systems.
  
  As each input is fed into the network, it is multiplied by a weight (obtained through training) and the result is fed into the next layer 
  of neurons. A bias input is also included so that the enemy will still wander if no input is obtained. This is repeated until the network 
  reaches the output layer. 
  
  The output layer consists of three possible actions for the enemy: move, turn, and attack. The enemies learned to move toward the player and
  then attack when within range.
  
- Neuroevolution

  The enemies in the game were pre-trained to know to attack the player. Their training continues as the player progresses through the game using
  neuroevolution. Every time the player levels up, the population pool of enemies reproduces, adding twenty new individuals to the population. 
  Individuals are selected for reproduction based on their fitness, which is based solely on damage done to the player.
  
  Using this technique, enemies that do well against the player will create new enemies that may do better. Also, enemies can only create
  enemies of their same type (ranged or melee). The idea is that the game will get harder because better individuals appear over time, and
  these individuals will consist mostly of whichever enemy type the player is doing worse against.
  
- Finite State Machines
  
  Certain systems within the game made use of simple finite state machines. The doors make use of this technique. While enemies in the room
  are still alive, the doors remain locked. Once all enemies are dead, the doors unlock, allowing the player to move on by entering the next
  room. Once this occurs, the doors enter the locked state once again.
  
  The cats watching the player also have two states they can be in. The default state is to simply watch the player. If the player is killing 
  enemies too slowly they will enter their second state and kill the player.
