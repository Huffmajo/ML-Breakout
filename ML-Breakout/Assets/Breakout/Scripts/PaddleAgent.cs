using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;
using System;

public class PaddleAgent : Agent
{

    private BreakoutArea breakoutArea;
    public bool training;
    public List<Vector3> brickPositions;
	public List<GameObject> bricks;
    public AIBall ball;
    public float paddleXScale;

    public float ballCollisionReward = 10f;
	public float brickBreakReward = 5f;
	public float ballDropPenalty = -5f;
	public float ballHeldPenalty = -0.1f;


    public float paddleSpeed = 20f;
    public float xPosLimit;
    public float xNegLimit;
    public float horizontalInput;

	//brick observation/rewards

	public int bricksPrev;
	public int bricksNext;

	//dropping ball obs + rewards
	float ballYPos;
	float paddleYPos;

	// Start is called before the first frame update
	void Start()
    {
        if (training)
        {
            breakoutArea = GetComponentInParent<BreakoutArea>();
            // take note of all the bricks in the scene
            brickPositions = breakoutArea.brickPositions;
            bricks = breakoutArea.brickList;
            bricksPrev = breakoutArea.brickList.Count;
        }

		//get initial y values of paddle and ball
		paddleYPos = transform.position.y;
		ballYPos = ball.transform.position.y;

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
        if (ball.canLaunch)
            {
            // convert actions to axis values
        /*
            float release = vectorAction[0];
            float leftOrRight = 0f;
            
            if (vectorAction[1] == 1f)
            {
                leftOrRight = -1f;
            }
            else if (vectorAction[1] == 2f)
            {
                leftOrRight = 1f;
            }

        */
            int release = (int)vectorAction[0];

            int leftOrRight = (int)vectorAction[1];
            // convert axis values to movement
    //		transform.position += new Vector3(leftOrRight * Time.deltaTime * paddleSpeed, 0f, 0f);

            if (leftOrRight == 0)
            {
                transform.position += new Vector3(1 * Time.deltaTime * paddleSpeed, 0f, 0f);
            }
            else if (leftOrRight == 1)
            {
                transform.position += new Vector3(-1 * Time.deltaTime * paddleSpeed, 0f, 0f);
            }
            else
            {
                transform.position += new Vector3(0 * Time.deltaTime * paddleSpeed, 0f, 0f);
            }
        

            // restrict paddle movement to positive and negative limits
            transform.position = new Vector3(Mathf.Clamp(transform.position.x, xNegLimit, xPosLimit), transform.position.y, transform.position.z);

            // release ball from paddle
            if (release == 1f && ball.heldByPaddle == true)
            //if (release == 1 && ball.heldByPaddle)
            {
                ball.heldByPaddle = false;

                // start ball moving
                ball.LaunchBall(ball.ballSpeed, ball.launchAngle);
            
            }

            // small reward loss over time
            AddReward(-1f / agentParameters.maxStep);

            //large reward for breaking a brick
            //get number of bricks
            //compare to previous number of bricks
            //if number of bricks decreased add large reward
            
            if (training)
            {
                bricks.Clear();
                bricks = breakoutArea.brickList;
                bricksNext = bricks.Count;
                if (bricksNext < bricksPrev)
                {
                    if (bricksPrev == 84 && bricksNext == 0)
                    {
                        Debug.Log("All Bricks Broken!");            
                        //AddReward(completionAward);
                    }
                    else
                    {                
                        Debug.Log("brick break order error");
                        Debug.Log("prev: " + bricksPrev);
                        Debug.Log("next: " + bricksNext);
                    }
                }
                bricksPrev = bricksNext;
            }
            
            //penalty for dropping ball
            //get y position of ball
            //get y position of paddle
            //if ball is below paddle, small penalty
            //mark as done
            
            paddleYPos = transform.position.y;
            ballYPos = ball.transform.position.y;
            if (ballYPos < paddleYPos)
            {
                //AddReward(ballDropPenalty);

                //mark agent as done and reset
                Done();
            }
            

            // penalty for holding ball
            if (ball.heldByPaddle)
            {
                AddReward(ballHeldPenalty);
            }
        }
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
        AddVectorObs(transform.position);

        //get ball position
        AddVectorObs(ball.transform.position);

        // distance to ball
        AddVectorObs(Vector3.Distance(ball.transform.position, transform.position));

        // direction to ball
        AddVectorObs((ball.transform.position - transform.position).normalized);

        // ball movement direction
        // may want to normalize with:
        /*
        Quaternion rotation = transform.rotation;
        Vector3 normalized = rotation.eulerAngles / 180.0f - Vector3.one;  // [-1,1]
        Vector3 normalized = rotation.eulerAngles / 360.0f;  // [0,1]
        */
        AddVectorObs(ball.velocityAngle);

        // if ball is held
        AddVectorObs(ball.heldByPaddle);

		//number of bricks
		AddVectorObs(bricksPrev);
	
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