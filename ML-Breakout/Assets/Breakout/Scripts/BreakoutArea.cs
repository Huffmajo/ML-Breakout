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
//    public GameObject brickPrefab;
    public Transform brickListContainer;
    
    [HideInInspector]
    public float paddleXScale = 1f;
    [HideInInspector]
    public float ballSpeed = 20f;

    private Vector3 scaleChange;
    public List<Vector3> brickPositions;
    public List<GameObject> brickList;
    public int totalBricks;
    public int activeBricks;
    private List<Color> brickColors;
    private Vector3 paddleStartingPos;

    void Start()
    {
        // get all bricks 
        //Transform brickListContainer = null;
        
        //brickList = new List<GameObject>();
        brickColors = new List<Color>();
        brickPositions = new List<Vector3>();
                
        //bricks = GameObject.FindGameObjectsWithTag("brick");
        // Transform[] parentObjects = gameObject.GetComponentsInChildren<Transform>();

        // foreach (Transform child in parentObjects)
        // {
        //     if (child.tag == "BrickList")
        //     {
        //         brickListContainer = child;
        //     }
        // }



        // populate list with brick starting locations
        if (brickList.Count == 0 && brickListContainer != null)
        {
            foreach (Transform child in brickListContainer)
            {
                brickList.Add(child.gameObject);
                brickPositions.Add(child.position);
                brickColors.Add(child.GetComponent<Renderer>().material.GetColor("_Color"));
            }
        }
        else if (brickListContainer != null)
        {
            foreach (GameObject brick in brickList)
            {
                brickPositions.Add(brick.transform.position);
                brickColors.Add(brick.GetComponent<Renderer>().material.GetColor("_Color"));
            }
        }
        else
        {
            Debug.Log("brickListContainer is null");
        }



        // get starting position of paddle agent
        paddleStartingPos = paddleAgent.transform.position;
        ball.ballSpeed = paddleAcademy.ballSpeed;

        scaleChange = new Vector3(paddleAcademy.paddleXScale, 1, 1);
        paddleAgent.transform.localScale = scaleChange;

        totalBricks = brickList.Count;
        activeBricks = totalBricks;

    }

        // Update is called once per frame
    private void Update()
    {


        currentReward.text = paddleAgent.GetCumulativeReward().ToString("00.00");
    
        // if (brickList.Count == 0 && brickListContainer != null)
        // {
        //     foreach (Transform child in brickListContainer)
        //     {
        //         brickList.Add(child.gameObject);
        //         //brickPositions.Add(child.position);
        //         //brickColors.Add(child.GetComponent<Renderer>().material.GetColor("_Color"));
        //     }
        // }
    
    }

    //create reset area
    public override void ResetArea()
    {
        // place paddle and ball, make sure to include paddle size and ball speed 
        ball.ResetBall();
        ResetPaddle();

        resetAllBricks();
    }

    public void decrementBrick()
    {
        activeBricks--;
        if (activeBricks <= 0) 
        {
            //paddle big reward for finishing level
            ResetArea();
        }



    }

    // remove all bricks
            // RemoveAllBricks();

            // // place bricks
            // GenerateBricks();


    // move paddle back to starting middle position
    private void ResetPaddle()
    {
        paddleAgent.transform.position = paddleStartingPos;
    }

    private void resetAllBricks()
    {
        foreach (GameObject brick in brickList)
        {
            brick.SetActive(true);
           
            Brick brickScript = brick.GetComponent<Brick>();
            if (brickScript.maxCollisions > 0)
            {
                brickScript.counter = 0;
                var brickRenderer = brick.GetComponent<Renderer>();
                brickRenderer.material.SetColor("_Color", brickScript.colors[brickScript.counter]);
            }
        }
        activeBricks = totalBricks;
    }
}



//     // clear all bricks in area
//     private void RemoveAllBricks()
//     {
//         foreach (GameObject brick in brickList)
//         {
            
//             Destroy(brick);
//         }
// //        brickList.Clear();
// //        brickList = new List<GameObject>();
//     }



//     private void GenerateBricks()
//     {
//  //       brickList = new List<GameObject>();

//         for (int i = 0; i < brickPositions.Count; i++)
//         {
//             // create brick and set position and original color
//             GameObject brickObject = Instantiate<GameObject>(brickPrefab.gameObject);
//             brickObject.transform.position = brickPositions[i];
//             brickObject.GetComponent<Renderer>().material.color = brickColors[i];
            
//             // set brick training variables
//             Brick brickScript = brickObject.GetComponent<Brick>();
//             brickScript.training = true;
//             brickScript.paddleAgent = paddleAgent;
            
//             //adds back into brickList for reference
//             brickList.Add(brickObject.gameObject);

//             //puts new brick under the brickListContainer GameObject
//             brickObject.transform.parent = brickListContainer.transform;
//         }
//     }
// }
