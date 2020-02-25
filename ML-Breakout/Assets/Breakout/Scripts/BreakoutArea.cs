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
    public GameObject brickListContainer;
    
    [HideInInspector]
    public float paddleXScale = 1f;
    [HideInInspector]
    public float ballSpeed = 20f;

    private Vector3 scaleChange;

    public GameObject[] bricks;
    public List<Vector3> brickPositions;
    private List<GameObject> brickList;
    private List<Color> brickColors;
    private Vector3 paddleStartingPos;


    void Start()
    {
        // get all bricks 
        Transform brickListContainer = null;
        
        brickList = new List<GameObject>();
        brickColors = new List<Color>();
        brickPositions = new List<Vector3>();
                
        //bricks = GameObject.FindGameObjectsWithTag("brick");
        Transform[] parentObjects = gameObject.GetComponentsInChildren<Transform>();

        foreach (Transform child in parentObjects)
        {
            if (child.tag == "BrickList")
            {
                brickListContainer = child;
            }
        }

        // populate list with brick starting locations
        if (brickListContainer != null)
        {
            foreach (Transform child in brickListContainer)
            {
                brickPositions.Add(child.position);
                brickList.Add(child.gameObject);
                brickColors.Add(child.GetComponent<Renderer>().material.GetColor("_Color"));
            }
        }

        
        // get starting position of paddle agent
        paddleStartingPos = paddleAgent.transform.position;
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
            brickObject.GetComponent<Renderer>().material.color = brickColors[i];
            // set brick training variables
            Brick brickScript = brickObject.GetComponent<Brick>();
            brickScript.training = true;
            brickScript.paddleAgent = paddleAgent;
            
            
            brickList.Add(brickObject.gameObject);

            
            brickObject.transform.parent = brickListContainer.transform;



            // may need to add brick row color here
        }

        // get all bricks 
        //bricks = GameObject.FindGameObjectsWithTag("brick");
        //brickList repopulated in previous for loop
    }
}
