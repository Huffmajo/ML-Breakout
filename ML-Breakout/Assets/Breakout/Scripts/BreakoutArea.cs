using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;
using TMPro;

public class BreakoutArea : Area
{

    public PaddleAgent paddleAgent;
    public Ball ball;
    public TextMeshPro currentReward;
    public PaddleAcademy paddleAcademy;
    public GameObject brickPrefab;

    
    [HideInInspector]
    public float paddleXScale = 1f;
    [HideInInspector]
    public float ballSpeed = 20f;

    private Vector3 scaleChange;

    public GameObject[] bricks;
    public List<Vector3> brickPositions;
    private List<GameObject> brickList;
    private Vector3 paddleStartingPos;
    private Transform gobricks;

    void Start()
    {
        // get all bricks 
        //bricks = GameObject.FindGameObjectsWithTag("brick");
        var parentObjects = gameObject.GetComponentsInChildren<Transform>();
        

        foreach (Transform child in parentObjects)
        {
            if (child.name == "bricks")
            {
                Transform gobricks = child;
            }
        }

        // get starting position of paddle agent
        paddleStartingPos = paddleAgent.transform.position;

        // populate list with brick starting locations
        foreach (Transform child in gobricks)
        {

//ERROR HERE, NULL ref exception. object ref not set to an instance of an object



            if (child.tag == "brick")
            {
                brickPositions.Add(child.transform.position);
                brickList.Add(child.gameObject);
                //Debug.Log(child.name);
            }

        }



        ball.ballSpeed =paddleAcademy.ballSpeed;

        scaleChange = new Vector3(paddleAcademy.paddleXScale, 1, 1);
        paddleAgent.transform.localScale = scaleChange;


    }

        // Update is called once per frame
    private void Update()
    {
        currentReward.text = paddleAgent.GetCumulativeReward().ToString("00.00");
    }

    //create reset area
    public override void ResetArea()
    {
        // place paddle and ball, make sure to include paddle size and ball speed 
        ball.ResetBall();
        ResetPaddle();

        // remove all bricks
        RemoveAllBricks();

        // place bricks
        GenerateBricks();
    }

    // move paddle back to starting middle position
    private void ResetPaddle()
    {
        paddleAgent.transform.position = paddleStartingPos;
    }


    // clear all bricks in area
    private void RemoveAllBricks()
    {
        
//********************* Error here! Object ref not set

        foreach (GameObject brick in brickList)
        {
            Destroy(brick);
        }
        brickList.Clear();
    
    }


    // add bricks into area at original placement
    private void GenerateBricks()
    {


        for (int i = 0; i < brickPositions.Count; i++)
        {
            GameObject brickObject = Instantiate<GameObject>(brickPrefab.gameObject);
            brickObject.transform.position = brickPositions[i];

            // set brick training variables
            Brick brickScript = brickObject.GetComponent<Brick>();
            brickScript.training = true;
            brickScript.paddleAgent = paddleAgent;
            
            
            brickList.Add(brickObject.gameObject);
            // may need to add brick row color here
        }

        // get all bricks 
        //bricks = GameObject.FindGameObjectsWithTag("brick");
        //brickList repopulated in previous for loop
    }
}
