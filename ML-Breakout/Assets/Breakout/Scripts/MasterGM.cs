//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class MasterGM : MonoBehaviour
{
	//Game Objects
	private GameObject ball;
	private GameObject aiBall;
	private Ball ballScript;
	private Ball aiBallScript;
	private GameObject paddle;
	private GameObject aiPaddle;
	public PlayerGM playerGM;
	public AIGM aiGM;
	
	//UI objects
    public GameObject nextLevelButton;
	public GameObject endGameUI;
	public GameObject pauseGameUI;
	public GameObject startGameUI;

	//Variables	
	public bool gameStarted;
	private bool gameOver;
	private int playerLives;
    private int aiLives;
	private int bricksTotal;
	private int playerBricksLeft;
	private int aiBricksLeft;
	public float currentTime;
	public float finalTime;

	//Text Objects
	public TextMeshProUGUI titleText;
	public TextMeshProUGUI playerStats;
	public TextMeshProUGUI aiStats;
	public TextMeshProUGUI livesText;
	public TextMeshProUGUI timerText;
	public TextMeshProUGUI scoreText;

	//image var
	public Image crown;



	// Start is called before the first frame update
	void Start()
	{
		//initialize all vars
		currentTime = 0f;
		gameOver = false;
		getAllObjects();
		initializeUI();
		Time.timeScale = 0;
		
		gameStarted = false;//changed to true by player ball on first launch
	}

	void getAllObjects() 
	{
		//player side
		ball = playerGM.ball.gameObject;
		ballScript = ball.GetComponent<Ball>();
		paddle = playerGM.playerPaddle.gameObject;
		playerLives = playerGM.lives;
		playerBricksLeft = playerGM.bricksLeft;
		
		//ai side
		aiBall = aiGM.aiBall.gameObject;
		aiBallScript = aiBall.GetComponent<Ball>();
		aiPaddle = aiGM.aiPaddle.gameObject;
		aiLives = aiGM.lives;
		aiBricksLeft = aiGM.bricksLeft;

		//both sides
		bricksTotal = playerBricksLeft;
	}


	void initializeUI() {
		// setup initial UI values
		scoreText.text = "Score" + '\n' + playerGM.playerScore + "	" + aiGM.aiScore;
		livesText.text = "Lives\n" + playerGM.lives + "	" + aiGM.lives;
		timerText.text = "TIME: " + currentTime;

		nextLevelButton.SetActive(false);
	}


	void Update()
	{
		// remove gameplay instructions once game begins
		if (gameStarted && startGameUI.activeSelf)
		{
			startGameUI.SetActive(false);
		}

		// check for end game conditions
		if (IsGameOver() && !gameOver)
		{
			gameOver = true;
			//sets final time, disables ball/paddles, enables endgame UI
			finalTime = currentTime;
			Time.timeScale = 0;
			endGameCleanup();
			setPlayerStats();
			setAiStats();

			// find winner
			int winner = VictorIs();
			if (winner == 1){	
				PlayerWins();
			} else if (winner == 2) {
				PlayerLoses();
			} else {
				PlayerWins();
			}
		}
		else if (gameStarted)	//if player has done initial launch
		{
			// update UI information
			GetUpdatesFromSubGMs();
			UpdateUI();

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
	}

    void GetUpdatesFromSubGMs()
	{	//grabs vars from each GM
		playerLives = playerGM.lives;
		aiLives = aiGM.lives;
		playerBricksLeft = playerGM.bricksLeft;
		aiBricksLeft = aiGM.bricksLeft;

	}
	// update brick UI
	public void UpdateUI()
	{	//updates time, score, lives and associated texts
		currentTime += 1 * Time.deltaTime;
		var formattedTime = TimeSpan.FromSeconds(currentTime);
		timerText.text = string.Format("TIME\n{0:0}:{1:00}", formattedTime.Minutes, formattedTime.Seconds);
		scoreText.text = "Score" + '\n' + playerGM.playerScore + "	" + aiGM.aiScore;
		livesText.text = "Lives\n" + playerGM.lives + "	" + aiGM.lives;
	}
	
	//Sets the final stats for the player and AI
    void setPlayerStats() {
        playerStats.text = "Lives:	" + playerLives + '\n' + "Bricks:	" + playerBricksLeft + "/" + bricksTotal;
    }
	void setAiStats(){
		aiStats.text = "Lives:	" + aiLives + '\n' + "Bricks:	" + aiBricksLeft + "/" + bricksTotal;
	}

	// returns true if endgame conditions are met
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
		int victor = -1;
		// if both players lose last life at same frame, victor is side with fewer remaining bricks
		if (playerLives <= 0 && aiLives <= 0)
		{
			if (playerBricksLeft == aiBricksLeft)
			{	// rare draw scenario
				victor = 0;
			} else if (playerBricksLeft < aiBricksLeft) {
				victor = 1;
			} else {
				victor = 2;
			}
		}// otherwise victor is who ever didn't lose last life
		else if (aiLives <= 0) {
			victor = 1;
		} else if (playerLives <= 0) {
			victor = 2;
		} // otherwise victor is whom ever removed all their bricks
		else if (playerBricksLeft == 0) {
			victor = 1;
		} else if (aiBricksLeft == 0) {
			victor = 2;
		}
		return victor;
	}

	//removes game pieces, and enables endGameUI 
	void endGameCleanup() {
		
		// disable paddles and balls
		ball.SetActive(false);
		paddle.SetActive(false);
		aiBall.SetActive(false);
		aiPaddle.SetActive(false);

		// set UI to be active
		endGameUI.SetActive(true);
	}


	public void PlayerLoses()
	{
		// play sad trombone, update text, set crown image above AI
        FindObjectOfType<AudioManager>().Play("GameOver");
		titleText.text = "AI WINS";
		crown.transform.localPosition = new Vector3(370.0f, 90.0f, 0f);
	}

	void PlayerWins()
	{
		// play victory sound, update text, set crown above player
        FindObjectOfType<AudioManager>().Play("Victory");
		titleText.text = "PLAYER WINS";
		crown.transform.localPosition = new Vector3(-360.0f, 90.0f, 0f);

		// disable next level button if no more levels
		if (IsLastLevel()) {
			nextLevelButton.SetActive(false);
		} else {
			nextLevelButton.SetActive(true);
		}
	}

	public void RestartLevel() {
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	public void GoToMainMenu() {
		SceneManager.LoadScene("Main Menu");
        Time.timeScale = 1;
	}

	public void GoToNextLevel() {
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
	}

	public bool IsLastLevel()
	{	//checks if this level is the last in loaded list
		int currentLevelIndex = SceneManager.GetActiveScene().buildIndex;
		int totalNumLevels = SceneManager.sceneCountInBuildSettings - 1;
		bool isLast = true;

		if (currentLevelIndex < totalNumLevels)
		{
			isLast = false;
		}

		return isLast;
	}
}