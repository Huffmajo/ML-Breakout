using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;
using TMPro;

public class BreakoutArea : Area
{

    public PaddleAgent paddleAgent;
    public Ball Ball;
    public TextMeshPro currentReward;
    public PaddleAcademy paddleAcademy;
    public GameObject brickPrefab;

    [HideInInspector]
    public float paddleXScale = 1f;
    [HideInInspector]
    public float ballSpeed = 20f;

    public GameObject[] bricks;
    public List<Vector3> brickPositions;
    private List<GameObject> brickList;
    private Vector3 paddleStartingPos;

    void Start()
    {
        // get all bricks 
        bricks = GameObject.FindGameObjectsWithTag("brick");

        // get starting position of paddle agent
        paddleStartingPos = paddleAgent.transform.position;

        // populate list with brick starting locations
        foreach (GameObject brick in bricks)
        {
            brickPositions.Add(brick.transform.position);
        }
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
        Ball.ResetBall();
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
        // maybe change paddleXscale here
    }

    // clear all bricks in area
    private void RemoveAllBricks()
    {
        foreach (GameObject brick in bricks)
        {
            Destroy(brick);
        }
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

            // may need to add brick row color here
        }

        // get all bricks 
        bricks = GameObject.FindGameObjectsWithTag("brick");
    }
}
