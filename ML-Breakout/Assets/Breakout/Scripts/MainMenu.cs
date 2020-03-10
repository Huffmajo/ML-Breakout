using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
	private GameObject aiBall;
	private Ball aiBallScript;

	// loads level select scene
	public void level01()
	{
		SceneManager.LoadScene("Versus01");
	}

	public void level02()
	{
		SceneManager.LoadScene("Versus02");
	}

	public void level03()
	{
		SceneManager.LoadScene("Versus03");
	}

	//function called when main menu reloads 
	public void OnEnable()
	{
		Debug.Log("enable main menu script");
		//what happens if I reload entire scene??
		//NO!!! don't do that --> infinite loop
		//BAD IDEA
		//SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

		//find the current AI ball in the scene
		aiBall = GameObject.FindWithTag("AIBall");
		//aiBall = GameObject.Find("/AIArea/AIBall");
		if (aiBall == null)
		{
			Debug.Log("AIBall not found");
		}
		else
		{
			//use ball functions to launch or reset the ball
			aiBallScript = aiBall.GetComponent<Ball>();
			//aiBallScript.ResetBall();
			aiBallScript.LaunchBall(20f, 45f);
		}
		
		
	}

	public void OnDisable()
	{
		Debug.Log("script disabled");
	}



	// exits game
	public void Quit()
	{
		// close application when run from executable
		Application.Quit();

		// close down editor when running from editor
		//UnityEditor.EditorApplication.isPlaying = false;
	}
}
