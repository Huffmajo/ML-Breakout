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
	public Vector3 ballImpactVector;
	public float launchAngle;

	private Rigidbody rb;
	private Vector3 heldBallPosition;
	private float minBallVerticalVelocity = 5f;

    // Start is called before the first frame update
    void Start()
    {
    	// get ball rigidbody
    	rb = GetComponent<Rigidbody>();

    	// ball speed is starting speed
    	ballSpeed = startingSpeed;

    	ResetBall();
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

    		

    		// release ball from user input
    		if (releaseButton)
    		{
    			heldByPaddle = false;

	    		// start ball moving
    	    	LaunchBall(ballSpeed, launchAngle);
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

        // get combined velocity
        totalVelocity = Mathf.Abs(rb.velocity.x) + Mathf.Abs(rb.velocity.y);

        // get angle of ball's vector DELETE AFTER TESTING
        velocityAngle = Mathf.Atan2(ballVelocity.y, ballVelocity.x) * Mathf.Rad2Deg;
    }

    // ball bounces toward side of paddle hit, bounces as usual in center of paddle
    void OnCollisionEnter(Collision col)
    {
    	// play bounce sound
    	FindObjectOfType<AudioManager>().Play("Bounce");

    	if (col.gameObject.tag == "paddle")
    	{
    		// get where ball hits paddle
    		ContactPoint contact = col.contacts[0];
    		Vector3 pos = contact.point;
    		ballImpactVector = paddle.transform.InverseTransformPoint(pos); 

	    	launchAngle = CalculateLaunchAngle(ballImpactVector.x);

	    	// launch ball at angle
    		LaunchBall(ballSpeed, launchAngle);
    	}
    }

    // resets local variables and location of the ball
    public void ResetBall()
    {
    	launchAngle = 45f;

        // ball is docked to paddle
        heldByPaddle = true;
        heldBallPosition = new Vector3(paddle.transform.position.x, paddle.transform.position.y + 2, paddle.transform.position.z);
        transform.position = heldBallPosition;

        // ball has no velocity
    	rb.velocity = Vector3.zero;
    }

    // returns launch angle based on where the paddle is impacted
    float CalculateLaunchAngle(float impactPos)
    {
    	return 90 - (12 * impactPos);
    }

    // launches ball at specified speed and angle
    void LaunchBall(float speed, float angle)
    {
    	float xVelocity = speed * Mathf.Cos(angle * Mathf.Deg2Rad);
    	float yVelocity = speed * Mathf.Sin(angle * Mathf.Deg2Rad);
    	rb.velocity = new Vector3(xVelocity, yVelocity, 0f);
    }
}
