using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : MonoBehaviour
{
	public int maxCollisions = 0;
	public bool training;
	public PaddleAgent paddleAgent;
	private int collisionCount = 0;

	private int counter = 0;
	public List<Color> colors;	//colors defined in inspector window for prefabs



	//check for collisions and destroy brick
	void OnCollisionEnter(Collision other)
	{
		collisionCount++;
		
		if (collisionCount > maxCollisions)
		{
			Destroy(gameObject);
		}
		else if (maxCollisions > 0 && counter < colors.Count-1)
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
