//using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;
using TMPro;

public class BreakoutArea : Area
{
    // Game Objects
    public PaddleAgent paddleAgent;
    public Ball ball;
    public TextMeshPro currentReward;
    public PaddleAcademy paddleAcademy;
    public Transform brickListContainer;
    public List<GameObject> brickList;    
    public List<Vector3> brickPositions;
    private List<Color> brickColors;
    
    // Variables
    public int totalBricks;
    public int activeBricks;
    private Vector3 paddleStartingPos; 
    // [HideInInspector]
    // public float paddleXScale = 1f;
    // [HideInInspector]
    // public float ballSpeed = 20f;



    void Start()
    {
        brickColors = new List<Color>();
        brickPositions = new List<Vector3>();

		// get game objects
		if (brickListContainer == null){        
			getBrickListContainer();
		}
	
		// find each brick in the brickListContainer 
		if (brickListContainer != null) {
			foreach (Transform child in brickListContainer) {
				brickList.Add(child.gameObject);                
                brickPositions.Add(child.position);
                brickColors.Add(child.GetComponent<Renderer>().material.GetColor("_Color"));
			}
		}
  
        
        // set variables
        paddleStartingPos = paddleAgent.transform.position;
        ball.ballSpeed = paddleAcademy.ballSpeed;
        totalBricks = brickList.Count;
        activeBricks = totalBricks;

        //gets x scale size for paddle from academy
        paddleAgent.transform.localScale = new Vector3(paddleAcademy.paddleXScale, 1, 1);
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

    private void Update()
    {
        currentReward.text = paddleAgent.GetCumulativeReward().ToString("00.00");
    }

    public void decrementBrick()
    {
        activeBricks--;
        if (activeBricks <= 0) 
        {
            //completed level
            ResetArea();
        }
    }
    
    //create reset area
    public override void ResetArea()
    {
        // reset paddle and ball
        ball.ResetBall();       
        paddleAgent.transform.position = paddleStartingPos;
        
        resetAllBricks();
    }

    private void resetAllBricks()
    {
        foreach (GameObject brick in brickList)
        {   // re-activate brick
            brick.SetActive(true);
           
            // get script and reset collision Count
            Brick brickScript = brick.GetComponent<Brick>();
            brickScript.collisionCount = 0;

            //if a tough brick, reset counter and color
            if (brickScript.maxCollisions > 0)
            {   
                brickScript.colorIndex = 0;
                var brickRenderer = brick.GetComponent<Renderer>();
                brickRenderer.material.SetColor("_Color", brickScript.colors[brickScript.colorIndex]);
            }
        }
        activeBricks = totalBricks;
    }
}

