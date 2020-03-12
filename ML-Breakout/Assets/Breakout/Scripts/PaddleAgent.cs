//using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;
//using System;

public class PaddleAgent : Agent
{
    // Game Objects
    public BreakoutArea breakoutArea;
    public List<Vector3> brickPositions;
	public List<GameObject> bricks;
    public Ball ball;
    
    // Variables
    public bool training;
    public float paddleXScale;
    public float paddleSpeed = 20f;
    public float xPosLimit;
    public float xNegLimit;
    public float horizontalInput;
    public bool demo;
	public int bricksLeft;
	float ballYPos;
	float paddleYPos;


    // Rewards
    public float ballCollisionReward = 10f;
	public float brickBreakReward = 5f;
	public float ballDropPenalty = -5f;
	public float ballHeldPenalty = -0.1f;


	void Start()
    {
        if (training)
        {   //get positions of all bricks from breakoutArea
            brickPositions = breakoutArea.brickPositions;
            bricksLeft = breakoutArea.brickList.Count;
        }

		//get initial y values of paddle and ball
		paddleYPos = transform.position.y;
		ballYPos = ball.transform.position.y;

        if (demo)
        {
            xPosLimit = 5.5f;
            xNegLimit = -52.5f;
        }
 	}

    /// <summary>
    /// Read inputs from the keyboard and convert them to a list of actions.
    /// This is called only when the player wants to control the agent and has set
    /// Behavior Type to "Heuristic Only" in the Behavior Parameters inspector.
    /// </summary>
    /// <returns>A vectorAction array of floats that will be passed into <see cref="AgentAction(float[])"/></returns>
    public override float[] Heuristic()
    {
        float[] actions = new float[2]; 
        float releaseAction = 0f;
        float movementAction = 0f;

        if (Input.GetButtonDown("Jump"))
        {
            // release ball
            releaseAction = 1f;
        }
        if (Input.GetAxis("Horizontal") > 0)
        {
            // move left
            movementAction = 1f;
        }
        else if (Input.GetAxis("Horizontal") < 0)
        {
            // move right
            movementAction = -1f;
        }

        actions[0] = releaseAction;
        actions[1] = movementAction;

        return actions;
    }


    public override void AgentAction(float[] vectorAction)
    {
        // Set controls for agent
        int release = (int)vectorAction[0];
/*
        int leftOrRight;
        if ((int)vectorAction[1] == 0)
        {
            leftOrRight = 0;
        } else if ((int)vectorAction[1] == 1) {
            leftOrRight = 1;
        } else {
            leftOrRight = -1;
        }
    
        transform.position += new Vector3(leftOrRight * Time.deltaTime * paddleSpeed, 0f, 0f);
*/

        int leftOrRight = (int)vectorAction[1];
        if (leftOrRight == 0) {
            transform.position += new Vector3(1 * Time.deltaTime * paddleSpeed, 0f, 0f);
        } else if (leftOrRight == 1) {
            transform.position += new Vector3(-1 * Time.deltaTime * paddleSpeed, 0f, 0f);
        } else {
            transform.position += new Vector3(0 * Time.deltaTime * paddleSpeed, 0f, 0f);
        }

        // restrict paddle movement to positive and negative limits
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, xNegLimit, xPosLimit), transform.position.y, transform.position.z);

        // release ball from paddle
        if ((release == 1f && ball.heldByPaddle == true) || (demo && ball.heldByPaddle))
        {
            ball.heldByPaddle = false;
            ball.LaunchBall(ball.ballSpeed, ball.launchAngle);
        }

        // small reward loss over time
        //AddReward(-1f / agentParameters.maxStep);

        // penalty for holding ball
        if (ball.heldByPaddle)
        {
            AddReward(ballHeldPenalty);
        }
        if (training)
        {
            bricksLeft = breakoutArea.activeBricks;        
        }
/*
        // small reward for holding still
        if (leftOrRight > 1 && !ball.heldByPaddle)
        {
            AddReward(0.001f);
        }
*/
    }

    public override void AgentReset()
    {
        if (training)
        {
            breakoutArea.ResetArea();    
        }
    }

    public override void CollectObservations()
    {
        // paddle position
        AddVectorObs(transform.position.x);
        AddVectorObs(transform.position.y);

        //get ball position
        AddVectorObs(ball.transform.position.x);
        AddVectorObs(ball.transform.position.y);

        // distance to ball
        //AddVectorObs(Vector3.Distance(ball.transform.position, transform.position));

        // direction to ball
        //AddVectorObs((ball.transform.position - transform.position).normalized);

        // ball movement direction
        // may want to normalize with:
        /*
        Quaternion rotation = transform.rotation;
        Vector3 normalized = rotation.eulerAngles / 180.0f - Vector3.one;  // [-1,1]
        Vector3 normalized = rotation.eulerAngles / 360.0f;  // [0,1]
        */
        AddVectorObs(ball.velocityAngle/360f);

        // if ball is held
        AddVectorObs(ball.heldByPaddle);

		//number of bricks
		//AddVectorObs(bricksLeft);
	
    }

    // BUG: not adding reward
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "ball")
        {
            AddReward(ballCollisionReward);
        }
    }
}