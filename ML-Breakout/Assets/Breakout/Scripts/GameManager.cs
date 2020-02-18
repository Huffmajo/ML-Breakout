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
	public bool levelComplete = false;
	public int bricksTotal;
	public int bricksLeft;
	public GameObject nextLevelButton;
	public GameObject endGameUI;
	public GameObject pauseGameUI;
	public List<GameObject> bricks;
	public float currentTime;

	public TextMeshProUGUI brickText;
	public TextMeshProUGUI titleText;
	public TextMeshProUGUI livesText;
	public TextMeshProUGUI timerText;

	// Start is called before the first frame update
	void Start()
	{

		currentTime = 0f;
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
		
		bricks.AddRange(GameObject.FindGameObjectsWithTag("brick"));

		// setup brick count UI variables
		bricksTotal = bricks.Count;
		bricksLeft = bricks.Count;
		brickText.text = "BRICKS LEFT\n" + bricksLeft + "/" + bricksTotal;
		livesText.text = "LIVES: " + _lives;

		endGameUI = GameObject.FindWithTag("EndGameUI");
		pauseGameUI = GameObject.FindWithTag("PauseGameUI");

		endGameUI.SetActive(false);
		pauseGameUI.SetActive(false);


		timerText.text = "TIME: " + currentTime;
	}

	void Update()
	{
		//UpdateUI();
		livesText.text = "LIVES: " + _lives;

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

		if (_lives <= 0 && gameOver == false)
		{
			endGameUI.SetActive(true);
			gameOver = true;
			currentTime = 0;
		}

		//Timer
		if (gameOver == false && levelComplete == false)
		{
			currentTime += 1 * Time.deltaTime;
			timerText.text = "TIME: " + currentTime.ToString("0");
		}

	}

	// update brick UI
	public void UpdateUI()
	{
		//bricksLeft--;
		//bricks = GameObject.FindGameObjectsWithTag("brick");
		//bricks = new List<GameObject>();		
		//bricks.AddRange(GameObject.FindGameObjectsWithTag("brick"));
		/*foreach (GameObject brick in bricks) {
			if (brick == null) 
			{
				bricks.Remove(brick);
			}
		}*/


		//removes null values in the list
		bricks.RemoveAll(item => item == null);
		bricksLeft = bricks.Count;
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
		// big oof
        FindObjectOfType<AudioManager>().Play("Death");

		DecrementLives();
		RespawnBall();

		if (_lives <= 0)
		{
			GameOver();
		}
	}

	public void DecrementLives()
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

	public void GameOver()
	{
		// set UI to be active
		endGameUI.SetActive(true);

		// play sad trombone
		//FindObjectOfType<AudioManager>().Stop("BGM");
        FindObjectOfType<AudioManager>().Play("GameOver");

		// update game over message
		titleText.text = "GAME OVER";

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
		levelComplete = true;

		// set UI to be active
		endGameUI.SetActive(true);

		// play victory sound
		//FindObjectOfType<AudioManager>().Stop("BGM");
        FindObjectOfType<AudioManager>().Play("Victory");

		// update game over message
		titleText.text = "LEVEL COMPLETE";

		//make paddle and ball disappear
		ball.SetActive(false);
		paddle.SetActive(false);

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
