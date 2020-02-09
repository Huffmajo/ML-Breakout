using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
	public static int _lives = 3;
	public static GameObject ball;
	static Ball ballScript;
	public static GameObject paddle;
	public static bool gameOver = false;
	public int bricksTotal;
	public int bricksLeft;
	public GameObject nextLevelButton;
	public GameObject endGameUI;
	public GameObject[] bricks;

	public TextMeshProUGUI brickText;
	public TextMeshProUGUI resultsText;
	public TextMeshProUGUI titleText;
	public TextMeshProUGUI livesText;

	// Start is called before the first frame update
	void Start()
	{
		gameOver = false;
		_lives = 3;
		ball = GameObject.FindWithTag("ball");
		paddle = GameObject.FindWithTag("paddle");
		ballScript = ball.GetComponent<Ball>();
		ball.SetActive(true);
		paddle.SetActive(true);

		if (ballScript == null)
		{
			Debug.Log("ballScript is NULL");
		}

		// take note of all the bricks in the scene
		bricks = GameObject.FindGameObjectsWithTag("brick");

		// setup brick count UI variables
		bricksTotal = bricks.Length;
		bricksLeft = bricks.Length;
		Debug.Log("brickText: " + brickText.text);
		brickText.text = "BRICKS LEFT\n" + bricksLeft + "/" + bricksTotal;
		livesText.text = "LIVES: " + _lives;
		endGameUI.SetActive(false);
	}

	// update brick UI
	public void UpdateUI()
	{
		bricksLeft--;
		brickText.text = "BRICKS LEFT\n" + bricksLeft + "/" + bricksTotal;

		// bring up 
		if (bricksLeft <= 0)
		{
			LevelComplete();
		}
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
		livesText.text = "LIVES: " + _lives;
	}

	void RespawnBall()
	{
		//respawn ball above paddle
		ballScript.ResetBall();
	}

	void GameOver()
	{
		Debug.Log("GAMEOVER");

		// set UI to be active
		endGameUI.SetActive(true);

		// update game over message
		titleText.text = "GAME OVER";
		Debug.Log(titleText.text);
		// update results text
		resultsText.text = bricksLeft + "/" + bricksTotal + " BRICKS LEFT";

		//make paddle and ball disappear
		ball.SetActive(false);
		paddle.SetActive(false);

		// disable next level button if no more levels
		if (IsLastLevel())
		{
			nextLevelButton.SetActive(false);
		}
	}

	void LevelComplete()
	{
		// update game over message
		titleText.text = "LEVEL COMPLETE";

		// update results text
		resultsText.text = "ALL BRICKS DESTROYED";

		//make paddle and ball disappear
		ball.SetActive(false);
		paddle.SetActive(false);

		// set UI to be active
		endGameUI.SetActive(true);

		// disable next level button if no more levels
		if (IsLastLevel())
		{
			nextLevelButton.SetActive(false);
		}
	}

	// reload current level
	public void RestartLevel()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	// load main menu scene
	public void GoToMainMenu()
	{
		SceneManager.LoadScene("Main Menu");
	}

	// check if this is the last level
	public bool IsLastLevel()
	{
		int currentLevelIndex = SceneManager.GetActiveScene().buildIndex;
		int totalNumLevels = SceneManager.sceneCountInBuildSettings;
		bool isLast = true;

		if (currentLevelIndex < totalNumLevels)
		{
			isLast = false;;
		}

		return isLast;
	}

	// loads next level in build list
	public void GoToNextLevel()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
	}

}
