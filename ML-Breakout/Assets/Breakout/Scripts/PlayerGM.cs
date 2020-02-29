using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGM : MonoBehaviour
{
	public Paddle playerPaddle;
	public Ball ball;

	public int lives;
	public int bricksLeft;

	public GameObject[] bricks;
	public List<GameObject> brickList;
	public Transform brickListContainer;

	private Vector3 paddleStartingPos;

    void Start()
    {
    	// set player lives
    	lives = 3;

    	// get paddle starting position for resets
    	paddleStartingPos = playerPaddle.transform.position;

    	// get container of brick prefabs
        brickListContainer = null;
        Transform[] parentObjects = gameObject.GetComponentsInChildren<Transform>();

        foreach (Transform child in parentObjects)
        {
        	if (child.tag == "BrickList")
        	{
        		brickListContainer = child;
        	}
        }

        // find each brick in this 
        if (brickListContainer != null)
        {
        	foreach (Transform child in brickListContainer)
        	{
        		brickList.Add(child.gameObject);
        	}
        }

        // set number of bricks remaining
        bricksLeft = brickList.Count;
    }

    // called when player ball collides with a brick
    public void DecrementBrick()
    {
    	bricksLeft--;
    }

    // decrements lives and resets ball
	public void LoseLife()
	{
		// big oof
        FindObjectOfType<AudioManager>().Play("Death");

		lives--;

		ball.ResetBall();
	}
}
