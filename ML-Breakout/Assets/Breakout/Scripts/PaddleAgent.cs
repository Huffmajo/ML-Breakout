using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;

public class Paddle : Agent
{

    private BreakoutArea breakoutArea;

    private GameObject Ball; //stores position and velocity


    public float ballCollisionReward = 1f;
    public float paddleSpeed = 20f;
    public float xPosLimit = 9f;
    public float xNegLimit = -9f;


    public override float Heuristic()
    {
        float leftOrRight = 0f;

        if (Input.GetAxis("Horizontal") > 0)
        {
            leftOrRight = 1f;
        }
        else if (Input.GetAxis("Horizontal") < 0)
        {
            leftOrRight = -1f;
        }

        return  leftOrRight;
    }





    public override void AgentAction(float vectorAction)
    {;

        transform.Translate( 0f, vectorAction * Time.deltaTime * paddleSpeed, 0f);
    
        //AddReward(-1f / agentParameters.maxSteps);

    }


    public override void AgentReset()
    {
        breakoutArea.ResetArea();
    }

    public override void CollectObservations()
    {

        //possible options, get exact positions for everything

        //get ball position
        //get ball vectors
        //get brick positions
        //get current position

        //do something with them

        //or use raycasting to determine where objects positions are...

/*

        float rayDistance = 20f;
		float[] rayAngles = {30f, 60f, 90f, 120f, 150f};
		string[] detectableObjects = {"ball", "brick", "wall"};
		AddVectorObs(rayPerception.Perceive(rayDistance, rayAngles, detectableObjects, 0f, 0f));
*/

    }

    // Start is called before the first frame update
    void Start()
    {
        breakoutArea = GetComponentsInParent<BreakoutArea>();
        Ball = breakoutArea.Ball;
        
        //rayPerception = GetComponent<RayPerception3D>();
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("ball"))
        {
            AddReward(ballCollisionReward);
        }
    }


/*
    // Update is called once per frame
    void Update()
    {
        //if outside higher x axis range, reset back to range limit
        if (transform.position.x > xPosLimit)
            transform.position = new Vector3(xPosLimit, transform.position.y, transform.position.z);

        //if outside lower x axis range, reset back to range limit
        if (transform.position.x < xNegLimit)
            transform.position = new Vector3(xNegLimit, transform.position.y, transform.position.z);

        transform.Translate( 0f, Input.GetAxis("Horizontal") * Time.deltaTime * paddleSpeed, 0f);
    }
}

*/
