using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : MonoBehaviour
{
	public int maxCollisions = 0;
	public bool training;
	public PaddleAgent paddleAgent;
	public int collisionCount = 0;

	public GameManager gm;
	public PlayerGM playerGM;
	public AIGM aiGM;
	public int counter = 0;
	public int points;
	public List<Color> colors;	//colors defined in inspector window for prefabs

	public BreakoutArea breakoutArea;

	void Start()
	{
		//Jeff: I attempted to base the points on colors, I gave up
		//and just assigned it in the inspector

		// var brickRenderer = gameObject.GetComponent<Renderer>();
		// thisColor = brickRenderer.material.GetColor("_Color");

		// Color red = new Color(248, 18, 18, 255);
		// Color orange = new Color(255, 132, 0, 255);
		// Color yellow = new Color(255, 251, 0, 255);
		// Color green = new Color(0, 147, 31, 255);
		// Color blue = new Color(7, 0, 255, 255);
		// Color indigo = new Color(136, 0, 255, 255);
		// Color violet = new Color(252, 0, 255, 255);

		// if (thisColor == red || thisColor == orange)
		// {
		// 	points = 1;
		// } 
		// else if (thisColor == yellow || thisColor == green) 
		// {
		// 	points = 4;
		// }
		// else if (thisColor == indigo || thisColor == violet)
		// {
		// 	points = 7;
		// } 
	}

	//check for collisions and destroy brick
	void OnCollisionEnter(Collision other)
	{
		if (training)
		{
			paddleAgent.AddReward(1f);
		}			
		
		collisionCount++;
		
		if (collisionCount > maxCollisions)
		{
			if (!training)
			{
				if (other.gameObject.tag == "ball")
				{
					// not working for some reason
					playerGM.DecrementBrick(points);
				}
				else if (other.gameObject.tag == "AIBall")
				{
					// not working for some reason
					aiGM.DecrementBrick(points);
				}
				else
				{
					gm.UpdateUI();
				}
			}
			else
			{
				breakoutArea.decrementBrick();
			}

			gameObject.SetActive(false);
//			Destroy(gameObject);
		}
		else if (maxCollisions > 0 && counter < colors.Count-1)	//for tough bricks
		{
			counter++;
			
			var brickRenderer = gameObject.GetComponent<Renderer>();
			brickRenderer.material.SetColor("_Color", colors[counter]);
		}
		else
		{
			Debug.Log("MaxCollision > 0 but counter !< colors.Count-1");
		}


	}
}
