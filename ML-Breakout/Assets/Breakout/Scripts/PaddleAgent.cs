using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;

public class PaddleAgent : Agent
{

    private BreakoutArea breakoutArea;

    public Ball ball;

    public float ballCollisionReward = 1f;
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
        float releaseAction = 0f;
        float leftOrRight = 0f;

        if (Input.GetButtonDown("Jump"))
        {
            // release ball
            releaseAction = 1f;
        }
        if (Input.GetAxis("Horizontal") > 0)
        {
            // move left
            leftOrRight = 1f;
        }
        else if (Input.GetAxis("Horizontal") < 0)
        {
            // move right
            leftOrRight = -1f;
        }

        return  new float[] { releaseAction, leftOrRight };
    }


    public override void AgentAction(float[] vectorAction)
    {
        // convert actions to axis values
        float release = vectorAction[0];
        float leftOrRight = vectorAction[1];

        // convert axis values to movement
        // move paddle based on input and paddleSpeed
        transform.position += new Vector3(leftOrRight * Time.deltaTime * paddleSpeed, 0f, 0f);

        // restrict paddle movement to positive and negative limits
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, xNegLimit, xPosLimit), transform.position.y, transform.position.z);

        // small reward loss over time
        //AddReward(-1f / agentParameters.maxStep);
    }

    public override void AgentReset()
    {
        breakoutArea.ResetArea();
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

        //get ball vectors
        AddVectorObs(ball.velocityAngle);

        //get brick positions

        /*
        float rayDistance = 20f;
		float[] rayAngles = {30f, 60f, 90f, 120f, 150f};
		string[] detectableObjects = {"ball", "brick", "wall"};
		AddVectorObs(rayPerception.Perceive(rayDistance, rayAngles, detectableObjects, 0f, 0f));
        */
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("ball"))
        {
            AddReward(ballCollisionReward);
        }
    }
}