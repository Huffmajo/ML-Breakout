# ML-Breakout
A Breakout clone that let's you play against a neural network trained to play the game. Can you beat our trained AI?

Joel Huffman, Jeffrey Dickinson, Tera Schaller

### Background
ML-Breakout was created in the Unity3D development engine. The machine learning aspects were implemented using Unity's [ML-Agents toolkit](https://github.com/Unity-Technologies/ml-agents). This includes the environment setup and training of the neural network.

### Try the Game
A compiled executable of the game can be downloaded at [itch.io](https://dickinsj.itch.io/machine-learning-breakout). Currently the game is only available for windows systems.

### How to Play
Opening the executable will launch the game and bring you to the main menu. From there you can use the below controls to navigate the menus and play the game.

#### Controls
The menus are navigated with the mouse while the paddle can be controlled by keyboard or gamepad with the controls seen below.
##### Keyboard
- **Paddle movement:** A/D or Left arrow/Right arrow
- **Start game/Launch ball:** Spacebar
##### Gamepad
- **Paddle movement:** Left analog stick
- **Start game/Launch ball:** Y/Triangle 

### The Process
#### ML Agents Workflow
The workflow for creating intelligent game agents in Unity is a three step process. Initially, the Unity game engine is used to create an environment and define the actions the agent can take within the environment. There are numerous ways to provide the agent with observations including using game data, ray casts, or camera data. To increase the likelihood of success in the few weeks available to work on the project, our group sought to simplify all aspects of this step as much as possible. Our agent has only a few actions and uses in game data from game object methods as observations. The rewards were straightforward and the overall goal - to break bricks without dropping the ball - was as simple as possible.

Once the agent and environment were built in Unity, it was possible to begin training the agent to achieve the goal. Training of the neural net was performed by Tensorflow in a separate Python process and communication with the C# Unity scripts that comprised the training environment and agent occurred via sockets. Tensorboard is a data visualization tool that can be opened in a browser. It can be used to observe training in real time or to view saved training runs. Important metrics such as cumulative reward and entropy can be used to gauge the effectiveness of current training runs.

We selected the deep reinforcement learning algorithm called Proximal Policy Optimization (PPO) to train our agent. This provides the agent with rewards and penalties for desirable and undesirable behaviors, and the agent gradually adjusts it’s behavior (the policy) to maximize it’s cumulative reward.

Finally, once training is complete, the resulting behavior model is stored as a neural net. This is a .nn file that can be dragged and dropped back into a suitable Agent in the Unity editor. The Agent class in Unity MLAgents takes in observations and uses that information to decide on a course of action. The neural net is attached to the Agent, and the neural net is what allows the
Agent to make decisions.

### Training
#### Actions
Actions are the ways an ML-agent can interact with the environment. The paddle agent's actions including movement (left, right or stay still) and choosing to launch the ball (launch or hold onto ball).

#### Observations
Observations are the information the paddle agent receives about it's environment. The agent uses this information in conjunction with the reward system to decide what actions to take.

We initially provided the paddle with positional data of all the game objects in scene. While it seemed like a good idea to give the agent as much information as possible about it's surroundings, it yielded very slow training as the neural net had too much information to pore over to make an effective choice. Keeping only the most essential observations decreased required training time from 2 hours to 15 minutes. The final list of observations included:
- The paddle's position (x and y position)
- The ball's position (x and y position)
- The ball's velocity vector
- If the paddle has released the ball

#### Rewards
Rewards are used to show agents how well they are doing at the assigned task. Positive rewards are given to encourage wanted behavior and punishments (negative rewards) are given to discourage unwanted behavior.

We only needed a few reward conditions to train the AI effectively. The rewards included:
- A big reward for the paddle reflecting the ball
- A small reward for the ball colliding with and destroying a brick
- A small punishment for not launching the ball
