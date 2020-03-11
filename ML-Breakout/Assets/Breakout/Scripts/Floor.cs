using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour
{
	public GameManager gameManager;
	public PlayerGM playerGM;
	public AIGM aiGM;
	public PaddleAgent paddleAgent;
	public Ball ball;
	public bool training;

	void OnCollisionEnter(Collision collision)
	{
		//decrement life from game manager
		if (collision.gameObject.tag == "ball")
		{
			if (training)
			{
				paddleAgent.AddReward(-1f);
				paddleAgent.Done();
				//ball.ResetBall();
			}
			else
			{
				playerGM.LoseLife();
			}
		}
	}
}

