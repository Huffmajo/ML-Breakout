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

        // small punishment for holding ball
        if (ball.heldByPaddle)
        {
            AddReward(-1f / agentParameters.maxStep);
        }
        // small reward for staying still
        else if (leftOrRight == 0)
        {
            AddReward(5f / agentParameters.maxStep);
        }
	}

    public override void AgentReset()
    {
        breakoutArea.ResetArea();
    }

    public override void CollectObservations()
    {
        // paddle position
        AddVectorObs(transform.position.x);

        //get ball position
        AddVectorObs(ball.transform.position.x);
        AddVectorObs(ball.transform.position.y);

        //AddVectorObs(ball.velocityAngle/360.0f);
        AddVectorObs(ball.rb.velocity.normalized);

        // if ball is held
        AddVectorObs(ball.heldByPaddle);
		
        // number of bricks left
        //AddVectorObs(breakoutArea.bricks.Length);
		
    }
}