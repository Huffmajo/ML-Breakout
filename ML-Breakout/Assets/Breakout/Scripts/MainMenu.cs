using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

	// loads level select scene
	public void LevelSelect()
	{
		//SceneManager.LoadScene(<level select scene name>);
	}

	// exits game
	public void Quit()
	{
		Application.Quit();
	}
}
