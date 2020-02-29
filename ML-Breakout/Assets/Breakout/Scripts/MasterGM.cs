using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MasterGM : MonoBehaviour
{
    public static int playerLives;
    public static int aiLives;
	public static GameObject ball;
	public static GameObject aiBall;
	static Ball ballScript;
	static AIBall aiBallScript;
	public static GameObject paddle;
	public static GameObject aiPaddle;
	public bool gameOver = false;
	public bool levelComplete = false;
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
		ball = GameObject.FindWithTag("ball");
		aiBall = GameObject.FindWithTag("AIBall");
		paddle = GameObject.FindWithTag("paddle");
		aiPaddle = GameObject.FindWithTag("AIPaddle");
		ballScript = ball.GetComponent<Ball>();
		aiBallScript = aiBall.GetComponent<AIBall>();
		ball.SetActive(true);
		aiBall.SetActive(true);
		paddle.SetActive(true);
		aiPaddle.SetActive(true);

		// setup UI variables
		playerLives = playerGM.lives;
		aiLives = aiGM.lives;
		bricksTotal = playerGM.bricksLeft;
		playerBricksLeft = playerGM.bricksLeft;
		aiBricksLeft = aiGM.bricksLeft;
		
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
		// game is over
		if (IsGameOver() && gameOver == false)
		{
			finalTime = currentTime;
			gameOver = true;

			// disable paddles and balls
			ball.SetActive(false);
			paddle.SetActive(false);
			aiBall.SetActive(false);
			aiPaddle.SetActive(false);

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
				Debug.Log("Rare draw condition");
				PlayerWins();
			}
		}
		else
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
		bricksTotal = playerGM.bricksLeft;
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
		timerText.text = "TIME\n" + currentTime;
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

		if (playerBricksLeft < aiBricksLeft || aiLives <= 0)
		{
			victor = 1;
		}
		else if (playerBricksLeft > aiBricksLeft || playerLives <= 0)
		{
			victor = 2;
		}

		return victor;
	}

	public void PlayerLoses()
	{
		// set UI to be active
		endGameUI.SetActive(true);

		// play sad trombone
        FindObjectOfType<AudioManager>().Play("GameOver");

        // update game over message
		titleText.text = "AI WINS";
	}

	void PlayerWins()
	{
		levelComplete = true;

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