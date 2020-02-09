using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour
{
	public GameManager gameManager;

	void OnCollisionEnter(Collision collision)
	{
		//decrement life from game manager
		if (collision.gameObject.tag == "ball")
		{
			gameManager.LoseLife();
		}
	}
}

