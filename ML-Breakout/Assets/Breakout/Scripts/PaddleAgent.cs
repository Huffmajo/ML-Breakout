using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;
using System;

public class PaddleAgent : Agent
{

    private BreakoutArea breakoutArea;

    public Ball ball;
    public float paddleSpeed = 20f;
    public float xPosLimit = 11f;
    public float xNegLimit = -9f;
    public float horizontalInput;

	// Start is called before the first frame update
	void Start()
    {
        breakoutArea = GetComponentInParent<BreakoutArea>();		
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
            // move right
            movementAction = 1f;
        }
        else if (Input.GetAxis("Horizontal") < 0)
        {
            // move left
            movementAction = 2f;
        }

        actions[0] = releaseAction;
        actions[1] = movementAction;

        return actions;
    }


    public override void AgentAction(float[] vectorAction)
    {
		// convert actions to axis values
		float release = vectorAction[0];
		float leftOrRight = 0f;
        
        if (vectorAction[1] == 1f)
        {
            leftOrRight = 1f;
        }
        else if (vectorAction[1] == 2f)
        {
            leftOrRight = -1f;
        }
        
        //vectorAction[1];
		//int release = (int)vectorAction[0];
		//int leftOrRight = (int)vectorAction[1];
		//Debug.Log("left or right: " + leftOrRight);
		//Debug.Log("release: " + release);

		// convert axis values to movement
		// move paddle based on input and paddleSpeed
		transform.position += new Vector3(leftOrRight * Time.deltaTime * paddleSpeed, 0f, 0f);

        // restrict paddle movement to positive and negative limits
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, xNegLimit, xPosLimit), transform.position.y, transform.position.z);

        // release ball from paddle
        if (release == 1f && ball.heldByPaddle == true)
        {
            ball.heldByPaddle = false;

            // start ball moving
            ball.LaunchBall(ball.ballSpeed, ball.launchAngle);
        }

        // small reward loss over time
        AddReward(-1f / agentParameters.maxStep);
	}

    public override void AgentReset()
    {
        breakoutArea.ResetArea();
    }

    public override void CollectObservations()
    {
        // paddle position
        AddVectorObs(transform.position.normalized);

        //get ball position
        AddVectorObs(ball.transform.position.normalized);

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
        AddVectorObs(ball.velocityAngle/360.0f);

        // if ball is held
        AddVectorObs(ball.heldByPaddle);
		
        // number of bricks left
        AddVectorObs(breakoutArea.bricks.Length);

		//get brick positions
        /*
        float rayDistance = 20f;
		float[] rayAngles = {30f, 60f, 90f, 120f, 150f};
		string[] detectableObjects = {"ball", "brick", "wall"};
		AddVectorObs(rayPerception.Perceive(rayDistance, rayAngles, detectableObjects, 0f, 0f));
        */
    }
}