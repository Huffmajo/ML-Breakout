# ML-Breakout
A Breakout clone that let's you play against a neural network trained to play the game. Can you beat our trained AI?

###Background
ML-Breakout was created in the Unity3D development engine. The machine learning aspects were implemented using Unity's [ML-Agents toolkit](https://github.com/Unity-Technologies/ml-agents). This includes the environment setup and training of the neural network.

###How to Play
A compiled version of the ML-Breakout game called Machine Learning Breakout.exe is available as the quickest way to play the game. This executable is compatible with Windows x86 systems and was tested on PCs running Windows 10. Opening the executable will launch the game and bring you to the main menu. From there you can use the below controls to navigate the menus and play the game.

####Controls
The menus are navigated with the mouse while the paddle can be controlled by keyboard or gamepad with the controls seen below.
#####Keyboard
*Paddle movement* - A/D or Left arrow/Right arrow
*Start game/Launch ball* - Spacebar
#####Gamepad
*Paddle movement* - Left analog stick
*Start game/Launch ball* - Y/Triangle

###The Process
####ML Agents Workflow
The workflow for creating intelligent game agents in Unity is a three step process. Initially, the Unity game engine is used to create an environment and define the actions the agent can take within the environment. There are numerous ways to provide the agent with observations including using game data, ray casts, or camera data. To increase the likelihood of success in the few weeks available to work on the project, our group sought to simplify all aspects of this step as much as possible. Our agent has only a few actions and uses in game data from game object methods as observations. The rewards were straightforward and the overall goal - to break bricks without dropping the ball - was as simple as possible.
Once the agent and environment were built in Unity, it was possible to begin training the agent to achieve the goal. Training of the neural net was performed by Tensorflow in a separate Python process and communication with the C# Unity scripts that comprised the training environment and agent occurred via sockets. Tensorboard is a data visualization tool that can be opened in a browser. It can be used to observe training in real time or to view saved training runs. Important metrics such as cumulative reward and entropy can be used to gauge the effectiveness of current training runs.
We selected the deep reinforcement learning algorithm called Proximal Policy Optimization (PPO) to train our agent. This provides the agent with rewards and penalties for desirable and undesirable behaviors, and the agent gradually adjusts it’s behavior (the policy) to maximize it’s cumulative reward.
Finally, once training is complete, the resulting behavior model is stored as a neural net. This is a .nn file that can be dragged and dropped back into a suitable Agent in the Unity editor. The Agent class in Unity MLAgents takes in observations and uses that information to decide on a course of action. The neural net is attached to the Agent, and the neural net is what allows the
Agent to make decisions.
