//using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGM : MonoBehaviour
{
	//Game Objects
	public Paddle playerPaddle;
	public Ball ball;
	public List<GameObject> brickList;
	public Transform brickListContainer;
	public MasterGM masterGM;

	//Variables
	public int lives;
	public int bricksLeft;
	public int playerScore;	
	private Vector3 paddleStartingPos;



    void Start()
    {
		// get game objects
		if (brickListContainer == null){        
			getBrickListContainer();
		}
	
		// find each brick in the brickListContainer 
		if (brickListContainer != null) {
			foreach (Transform child in brickListContainer) {
				brickList.Add(child.gameObject);
			}
		}
	
    	// set variables
    	lives = 3;
    	paddleStartingPos = playerPaddle.transform.position;
        bricksLeft = brickList.Count;
		playerScore = 0;
    }


	void getBrickListContainer() {
		//get this gameObject's transform for foreach traversal
		Transform[] parentObjects = gameObject.GetComponentsInChildren<Transform>();

		//find brickListContainer gameObject (empty gameObject that contains all bricks)
		foreach (Transform child in parentObjects) {
			if (child.tag == "BrickList") {
				brickListContainer = child;
			}
		}
	}


    // when player ball breaks brick
    public void DecrementBrick(int brickPoints)
    {	//dec bricksLeft and increase player score by passed in points from brick
    	bricksLeft--;
		playerScore += brickPoints;
    }

    // when player misses ball
	public void LoseLife()
	{	
        FindObjectOfType<AudioManager>().Play("Death");
		lives--;
		ball.ResetBall();
	}
}


