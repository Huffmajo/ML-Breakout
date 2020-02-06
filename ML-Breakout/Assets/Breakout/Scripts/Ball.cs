using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{

	public float ballSpeed = 20f;
	public float startingSpeed = 20f;
	public Vector3 ballDirection;
	public Vector3 ballVelocity;
	public GameObject paddle;
	public float totalVelocity;
	public bool heldByPaddle;
	public float velocityAngle;

	private float horiDirection = 1f;
	private float vertDirection = 1f;
	private Rigidbody rb;
	//private bool heldByPaddle;
	private Vector3 heldBallPosition;
	private float minBallVerticalVelocity = 5f;

    // Start is called before the first frame update
    void Start()
    {
    	// get ball rigidbody
    	rb = GetComponent<Rigidbody>();

    	// ball speed is starting speed
    	ballSpeed = startingSpeed;

    	// set starting velocity directions
        horiDirection = 1;
        vertDirection = 1;
        ballDirection = new Vector3(horiDirection, vertDirection, 0f);

        // ball starts docked to paddle
        heldByPaddle = true;
        heldBallPosition = new Vector3(paddle.transform.position.x, paddle.transform.position.y + 2, paddle.transform.position.z);
        transform.position = heldBallPosition;
    }

    // Update is called once per frame
    void Update()
    {
    	// get input for ball release
    	bool releaseButton = Input.GetButtonDown("Jump");

    	// ball is held in front of paddle until released
    	if (heldByPaddle)
    	{
    		heldBallPosition = new Vector3(paddle.transform.position.x, paddle.transform.position.y + 2, paddle.transform.position.z);
    		transform.position = heldBallPosition;

    		// remove velocity while ball is held
    		rb.velocity = Vector3.zero;

    		// release ball from user input
    		if (releaseButton)
    		{
    			heldByPaddle = false;

	    		// start ball moving
    	    	rb.velocity = ballDirection * ballSpeed;
    		}
    	}
    	// otherwise ball is released and play is active
    	else
    	{
    		// if ball has little to no y velocity, give it some
    		float absVelocityY = Mathf.Abs(rb.velocity.y);

    		if (absVelocityY < minBallVerticalVelocity)
    		{
    			ballDirection = new Vector3(rb.velocity.x, startingSpeed, rb.velocity.z);
    			rb.velocity = ballDirection;
    			Debug.Log("Y velocity too low, added some vertical oof!");
    		}
    	}

        // track velocity for interactions with bricks and paddle
        ballVelocity = rb.velocity;
        totalVelocity = Mathf.Abs(rb.velocity.x) + Mathf.Abs(rb.velocity.y);

        velocityAngle = Mathf.Atan2(ballVelocity.y, ballVelocity.x) * Mathf.Rad2Deg;
    }

    // ball bounces toward side of paddle hit, bounces as usual in center of paddle
    void OnCollisionEnter(Collision col)
    {
    	// play bounce sound
    	FindObjectOfType<AudioManager>().Play("Bounce");

    	// leftside hit makes ball bounce left
    	if (col.gameObject.tag == "paddle_left")
    	{
    		Debug.Log("Leftside paddle hit");
    		
    		/*
    		// if ball is moving right, reverse x velocity
    		if (rb.velocity.x > 0)
    		{
				ballDirection = new Vector3((rb.velocity.x * -1), rb.velocity.y, rb.velocity.z);
				rb.velocity = ballDirection;
    		}
    		*/

    		float angle = 135f;
    		float velx = ballSpeed * Mathf.Cos(angle * Mathf.Deg2Rad);
    		float vely = ballSpeed * Mathf.Sin(angle * Mathf.Deg2Rad);
    		rb.velocity = new Vector3(velx, vely, 0f);
    	}
    	// rightside hit makes ball bounce right
    	else if (col.gameObject.tag == "paddle_right")
    	{
    		Debug.Log("Rightside paddle hit");
    		/*
    		// if ball is moving right, reverse x velocity
    		if (rb.velocity.x < 0)
    		{
				ballDirection = new Vector3((rb.velocity.x * -1), rb.velocity.y, rb.velocity.z);
				rb.velocity = ballDirection;
    		}	
    		*/
    		float angle = 45f;
    		float velx = ballSpeed * Mathf.Cos(angle * Mathf.Deg2Rad);
    		float vely = ballSpeed * Mathf.Sin(angle * Mathf.Deg2Rad);
    		rb.velocity = new Vector3(velx, vely, 0f);
    	}
    	else if (col.gameObject.tag == "paddle_mid")
    	{
    		float angle = 0f;
    		float velx = ballSpeed * Mathf.Cos(angle * Mathf.Deg2Rad);
    		float vely = ballSpeed * Mathf.Sin(angle * Mathf.Deg2Rad);
    		rb.velocity = new Vector3(velx, vely, 0f);
    	}
    }
}
