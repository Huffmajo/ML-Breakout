using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
	//[SerializeField]
	//private int _lives = 3;
	public static int _lives = 3;
	public static GameObject ball;
	static Ball ballScript;
	public static GameObject paddle;
	//public Text livesText;
	//public Text gameOverText;

	// Start is called before the first frame update
	void Start()
	{
		_lives = 3;
		ball = GameObject.FindWithTag("ball");
		paddle = GameObject.FindWithTag("paddle");
		ballScript = ball.GetComponent<Ball>();
		ball.SetActive(true);
		paddle.SetActive(true);
		//gameOverText.gameObject.SetActive(false);

		if (ballScript == null)
		{
			Debug.Log("ballScript is NULL");
		}

		//livesText.text = "Lives: " + _lives.ToString();
	}

	//gets called by floor when ball hits it
	public void LoseLife()
	{
		DecrementLives();
		RespawnBall();

		if (_lives < 0)
		{
			GameOver();
		}
	}

	void DecrementLives()
	{
		Debug.Log("Decrement life, lives: " + _lives);
		_lives -= 1;
		//livesText.text = "Lives: " + _lives.ToString();
	}

	void RespawnBall()
	{
		//respawn ball above paddle
		Debug.Log("Respawn Ball");
		ballScript.ResetBall();
	}

	void GameOver()
	{
		//make paddle and ball disappear
		ball.SetActive(false);
		paddle.SetActive(false);
		//Game Over Message
		//gameOverText.gameObject.SetActive(true);
	}


}
