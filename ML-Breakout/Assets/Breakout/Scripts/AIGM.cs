//using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIGM : MonoBehaviour
{
	// Game Objects
	public PaddleAgent aiPaddle;
	public Ball aiBall;
	public List<GameObject> brickList;
	public Transform brickListContainer;

	// Variables
	public int lives;
	public int bricksLeft;
	public int aiScore;
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
		paddleStartingPos = aiPaddle.transform.position;
        bricksLeft = brickList.Count;
  		aiScore = 0;
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


    // when ai ball breaks brick
    public void DecrementBrick(int brickPoints)
    {	//dec bricksLeft and increase player score by passed in points from brick
    	bricksLeft--;
		aiScore += brickPoints;
    }

    // when ai misses ball
	public void LoseLife()
	{	
        FindObjectOfType<AudioManager>().Play("Death");
		lives--;
		aiBall.ResetBall();
	}
}
