using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class MasterGM : MonoBehaviour
{
    public int playerLives;
    public int aiLives;
	public GameObject ball;
	public GameObject aiBall;
	public Ball ballScript;
	public AIBall aiBallScript;
	public GameObject paddle;
	public GameObject aiPaddle;
	public bool gameOver;
	public bool levelComplete;
	public int bricksTotal;
	public int playerBricksLeft;
	public int aiBricksLeft;
	public GameObject nextLevelButton;
	public GameObject endGameUI;
	public GameObject pauseGameUI;
	public float currentTime;
	public float finalTime;

	public PlayerGM playerGM;
	public AIGM aiGM;

	public TextMeshProUGUI playerBricksLeftText;
	public TextMeshProUGUI aiBricksLeftText;
	public TextMeshProUGUI titleText;
	public TextMeshProUGUI playerLivesText;
	public TextMeshProUGUI aiLivesText;
	public TextMeshProUGUI timerText;

	// Start is called before the first frame update
	void Start()
	{
		currentTime = 0f;
		gameOver = false;
		levelComplete = false;
/*
		ball = GameObject.FindWithTag("ball");
		aiBall = GameObject.FindWithTag("AIBall");
		paddle = GameObject.FindWithTag("paddle");
		aiPaddle = GameObject.FindWithTag("AIPaddle");
*/
		ballScript = ball.GetComponent<Ball>();
		aiBallScript = aiBall.GetComponent<AIBall>();
/*
		ball.SetActive(true);
		aiBall.SetActive(true);
		paddle.SetActive(true);
		aiPaddle.SetActive(true);
*/
		// setup UI variables
		playerLives = 3;
		aiLives = 3;
		bricksTotal = GameObject.FindGameObjectsWithTag("brick").Length / 2;
		playerBricksLeft = bricksTotal;
		aiBricksLeft = bricksTotal;
/*
		Debug.Log("pLives: " + playerLives);
		Debug.Log("aiLives: " + aiLives);
		Debug.Log("pBricks: " + playerBricksLeft);
		Debug.Log("aiBricks: " + aiBricksLeft);
		Debug.Log("totalBricks: " + bricksTotal);
*/

		// setup initial UI values
		playerBricksLeftText.text = "BRICKS LEFT: " + playerBricksLeft + "/" + bricksTotal;
		playerLivesText.text = "LIVES: " + playerLives;
		aiBricksLeftText.text = "BRICKS LEFT: " + aiBricksLeft + "/" + bricksTotal;
		aiLivesText.text = "LIVES: " + aiLives;
		timerText.text = "TIME: " + currentTime;

		nextLevelButton.SetActive(false);
	}

	void Update()
	{
		// check for end game conditions
		if (IsGameOver() && gameOver == false)
		{
			finalTime = currentTime;
			gameOver = true;

			// find winner
			int winner = VictorIs();
			if (winner == 1)
			{
				// player won
				PlayerWins();
			}
			else if (winner == 2)
			{
				// ai won

				PlayerLoses();
			}
			else
			{
				// draw
				PlayerWins();
			}
		}
		else if (!gameOver)
		{
			// update UI information
			GetUpdatesFromSubGMs();
			UpdateUI();
		}

		// halt game with pause key
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			if (pauseGameUI.activeSelf == true)
			{
				Time.timeScale = 1;
				pauseGameUI.SetActive(false);
			}
			else
			{
				Time.timeScale = 0;
				pauseGameUI.SetActive(true);
			}
		}
	}

	void GetUpdatesFromSubGMs()
	{
		playerLives = playerGM.lives;
		aiLives = aiGM.lives;
		playerBricksLeft = playerGM.bricksLeft;
		aiBricksLeft = aiGM.bricksLeft;
		currentTime += 1 * Time.deltaTime;
	}

	// update brick UI
	public void UpdateUI()
	{
		playerBricksLeftText.text = "BRICKS LEFT: " + playerBricksLeft + "/" + bricksTotal;
		playerLivesText.text = "LIVES: " + playerLives;
		aiBricksLeftText.text = "BRICKS LEFT: " + aiBricksLeft + "/" + bricksTotal;
		aiLivesText.text = "LIVES: " + aiLives;
//		timerText.text = "TIME\n" + currentTime.ToString("0");
		var formattedTime = TimeSpan.FromSeconds(currentTime);
		timerText.text = string.Format("TIME\n{0:0}:{1:00}", formattedTime.Minutes, formattedTime.Seconds);
	}

	// returns true if endgame conditions are met
	// does not return victor of the game
	bool IsGameOver()
	{
		bool isOver = false;

		if (playerBricksLeft <= 0 ||
			aiBricksLeft <= 0 ||
			playerLives <= 0 ||
			aiLives <= 0)
		{
			isOver = true;
		}

		return isOver;
	}

	// returns 1 if player won, 2 if AI won and 0 if draw
	int VictorIs()
	{
		int victor = 0;

		// if both players lose last life at same frame, victor is side with fewer remaining bricks
		if (playerLives <= 0 && aiLives <= 0)
		{
			if (playerBricksLeft == aiBricksLeft)
			{
				// rare draw scenario
				victor = 0;
			}
			else if (playerBricksLeft < aiBricksLeft)
			{
				victor = 1;
			}
			else
			{
				victor = 2;
			}
		}
		// otherwise victor is who ever didn't lose last life
		else if (aiLives <= 0)
		{
			victor = 1;
		}
		else if (playerLives <= 0)
		{
			victor = 2;
		}
		// otherwise victor is whom ever removed all their bricks
		else if (playerBricksLeft == 0)
		{
			victor = 1;
		}
		else if (aiBricksLeft == 0)
		{
			victor = 2;
		}
		return victor;
	}

	public void PlayerLoses()
	{
		// disable paddles and balls
		ball.SetActive(false);
		paddle.SetActive(false);
		aiBall.SetActive(false);
		aiPaddle.SetActive(false);

		// set UI to be active
		endGameUI.SetActive(true);

		// play sad trombone
        FindObjectOfType<AudioManager>().Play("GameOver");

        // update game over message
		titleText.text = "AI WINS";
	}

	void PlayerWins()
	{
		// disable paddles and balls
		ball.SetActive(false);
		paddle.SetActive(false);
		aiBall.SetActive(false);
		aiPaddle.SetActive(false);

		// set UI to be active
		endGameUI.SetActive(true);

		// play victory sound
		//FindObjectOfType<AudioManager>().Stop("BGM");
        FindObjectOfType<AudioManager>().Play("Victory");

		// update game over message
		titleText.text = "PLAYER WINS";

		// disable next level button if no more levels
		if (IsLastLevel())
		{
			nextLevelButton.SetActive(false);
		}
		else
		{
			nextLevelButton.SetActive(true);
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
		int totalNumLevels = SceneManager.sceneCountInBuildSettings - 1;
		bool isLast = true;

		//Debug.Log("currentLevel: " + currentLevelIndex + " totalLevels: " + totalNumLevels);

		if (currentLevelIndex < totalNumLevels)
		{
			isLast = false;
		}

		return isLast;
	}

	// loads next level in build list
	public void GoToNextLevel()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
	}
}