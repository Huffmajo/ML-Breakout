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
		// loads first level
		// TODO: change this to load the level select screen once implemented
		SceneManager.LoadScene("Breakout Level 01");
	}

	// exits game
	public void Quit()
	{
		// close application when run from executable
		Application.Quit();

		// close down editor when running from editor
		UnityEditor.EditorApplication.isPlaying = false;
	}
}
