using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : MonoBehaviour
{
	public int maxCollisions = 0;
	public bool training;
	public PaddleAgent paddleAgent;
	private int collisionCount = 0;

	//check for collisions and destroy brick
	void OnCollisionEnter(Collision other)
	{
		collisionCount++;
		
		if (collisionCount > maxCollisions)
		{
			Destroy(gameObject);
		}

		if (training)
		{
			paddleAgent.AddReward(1f);
		}
	}
}
