using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;
using TMPro;

public class BreakoutArea : Area
{

    public Paddle Paddle;

    public GameObject Ball;

    public TextMeshPro currentReward;

    public PaddleAcademy paddleAcademy;

    private List<GameObject> brickList;



    //create reset area

    public override void ResetArea()
    {
        // to do

        //place paddle and ball, make sure to include paddle size and ball speed 


        //place bricks
    }

/*
    public void DeleteBrick(GameObject brick)
    {
        brickList.Remove(brick);
        Destroy(brick);
    }
*/

    private void RemoveAllBricks()
    {
        if (brickList != null)
        {
            for (int i = 0; i < brickList.Count; i++)
			{
				if (brickList[i] != null)
				{
					Destroy(brickList[i]);
				}
			}
        }
    }



    private void GenerateBricks(int count)
    {
        // to do, 
    }






    // Update is called once per frame
    private void Update()
    {
        currentReward.text = paddleAgent.GetCurrentReward().ToString("0.00");
    }
}
