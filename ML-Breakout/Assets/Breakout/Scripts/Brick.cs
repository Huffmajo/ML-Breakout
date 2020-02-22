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
	private int counter = 0;
	public List<Color> colors;	//colors defined in inspector window for prefabs

	void Start()
	{
		
	}



	//check for collisions and destroy brick
	void OnCollisionEnter(Collision other)
	{
		collisionCount++;
		
		if (collisionCount > maxCollisions)
		{
			if (!training)
			{
				gm.UpdateUI();
			}
			Destroy(gameObject);
		}
		else if (maxCollisions > 0 && counter < colors.Count-1)	//for tough bricks
		{
			counter++;
			
			var brickRenderer = gameObject.GetComponent<Renderer>();
			brickRenderer.material.SetColor("_Color", colors[counter]);
		}

		if (training)
		{
			paddleAgent.AddReward(1f);
		}
	}



}
