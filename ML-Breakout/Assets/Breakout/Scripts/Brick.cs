//using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : MonoBehaviour
{
	// Game Objects
	public PaddleAgent paddleAgent;
	public BreakoutArea breakoutArea;
	public GameManager gm;
	public PlayerGM playerGM;
	public AIGM aiGM;
	public List<Color> colors;	//colors defined in inspector window for prefabs

	// Variables
	public int maxCollisions = 0;
	public int collisionCount = 0;
	public int colorIndex = 0;
	public bool training;
	public int points;

	void Start()
	{}

	//check for collisions and destroy brick
	void OnCollisionEnter(Collision other)
	{
		if (training)
		{
			paddleAgent.AddReward(1f);
		}			
		
		collisionCount++;
		
		//if brick is destroyed
		if (collisionCount > maxCollisions)
		{
			if (!training)
			{
				if (other.gameObject.tag == "ball")
				{	//send brick points to playerGM
					playerGM.DecrementBrick(points);
				}
				else if (other.gameObject.tag == "AIBall")
				{	//send brick points to AIGM
					aiGM.DecrementBrick(points);
				}
				else
				{	//something went wrong
					Debug.Log("Not training and not ball or AIBall");
				}
			}
			else
			{
				breakoutArea.decrementBrick();
			}
			//deactivate the brick object
			gameObject.SetActive(false);
		}
		else if (maxCollisions > 0 && colorIndex < colors.Count-1)	//for tough bricks
		{
			colorIndex++;
			
			// change color to indicate brick is taking damage
			var brickRenderer = gameObject.GetComponent<Renderer>();
			brickRenderer.material.SetColor("_Color", colors[colorIndex]);
		}
		else
		{	//problem
			Debug.Log("MaxCollision > 0 but colorIndex !< colors.Count-1");
		}
	}
}
