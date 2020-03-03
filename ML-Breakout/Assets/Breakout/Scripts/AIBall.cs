using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBall : MonoBehaviour
{
	public float ballSpeed = 20f;
	public Vector3 ballDirection;
	public Vector3 ballVelocity;
	public GameObject paddle;
	public float totalVelocity;
	public bool heldByPaddle;
	public float velocityAngle;
	public Vector3 ballImpactVector;
	public float launchAngle;
    public AIGM gm;
	private Rigidbody rb;
	private Vector3 heldBallPosition;

	public bool canLaunch;


    // Start is called before the first frame update
    void Start()
    {
    	// get ball rigidbody
    	rb = GetComponent<Rigidbody>();

		if (paddle.gameObject.GetComponent<PaddleAgent>().training)
		{
			canLaunch = true;
		}
		else
		{
			canLaunch = false;
		}

    	ResetBall();

    }

    // Update is called once per frame
    void Update()
    {
    	// get input for ball release
    	bool releaseButton = Input.GetButtonDown("Jump");

    	// track velocity for interactions with bricks and paddle
        ballVelocity = rb.velocity;
		totalVelocity = Mathf.Abs(rb.velocity.x) + Mathf.Abs(rb.velocity.y);

        // get angle of ball's vector, 0-359°
        velocityAngle = Mathf.Atan2(ballVelocity.y, ballVelocity.x) * Mathf.Rad2Deg;
        if (velocityAngle < 0)
        {
        	velocityAngle += 360;
        }

    	// ball is held in front of paddle until released
    	if (heldByPaddle)
    	{
    		heldBallPosition = new Vector3(paddle.transform.position.x, paddle.transform.position.y + 2, paddle.transform.position.z);
    		transform.position = heldBallPosition;


    		// release ball from user input
    		if (releaseButton)
    		{
				if (canLaunch)
				{
    				heldByPaddle = false;

	    			// start ball moving
    	    		LaunchBall(ballSpeed, launchAngle);
				}
			}
    	}
    	// otherwise ball is released and play is active
    	else
    	{
    		// correct ball launch angle if too horizontal
    		if (BallAngleNeedFixing(velocityAngle))
    		{
    			launchAngle = FixBallAngle(velocityAngle);
    			LaunchBall(ballSpeed, launchAngle);
    		}
    	}
    }

    // ball bounces toward side of paddle hit, bounces as usual in center of paddle
    void OnCollisionEnter(Collision col)
    {
    	if (col.gameObject.tag == "paddle")
    	{

    		FindObjectOfType<AudioManager>().Play("Bounce");

    		// get where ball hits paddle
    		ContactPoint contact = col.contacts[0];
    		Vector3 pos = contact.point;
    		ballImpactVector = paddle.transform.InverseTransformPoint(pos); 

	    	launchAngle = CalculateLaunchAngle(ballImpactVector.x);

	    	// launch ball at angle
    		LaunchBall(ballSpeed, launchAngle);
    	}
    	else if (col.gameObject.tag == "brick")
    	{
	    	// play bounce sound
    		FindObjectOfType<AudioManager>().Play("Pop");

            // tell gm to decrease brick count
            //gm.DecrementBrick(col.gameObject.GetComponent<Brick>().points);
    	}
    	else if (col.gameObject.tag != "ground")
    	{
    		FindObjectOfType<AudioManager>().Play("Bounce");
    	}
        else if (col.gameObject.tag == "ground")
        {
            gm.LoseLife();
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
    	//rb.velocity = Vector3.zero;
    }

    // returns launch angle based on where the paddle is impacted
    float CalculateLaunchAngle(float impactPos)
    {
    	return 90 - (12 * impactPos);
    }

    // launches ball at specified speed and angle
    public void LaunchBall(float speed, float angle)
    {
    	float xVelocity = speed * Mathf.Cos(angle * Mathf.Deg2Rad);
    	float yVelocity = speed * Mathf.Sin(angle * Mathf.Deg2Rad);
    	rb.velocity = new Vector3(xVelocity, yVelocity, 0f);
    }

    // check if ball's velocity angle is too horizontal 
    bool BallAngleNeedFixing(float angle)
    {
    	bool needFix = false;

    	if  ((angle < 30) ||
    		(angle > 150 && angle <= 180) ||
    		(angle > 180 && angle <= 210) ||
    		(angle > 330))
    	{
    		needFix = true;
    	}
    	
    	return needFix;
    }

    // return fixed ball velocity if angle is not vertical enough
    float FixBallAngle(float angle)
    {
    	float fixedAngle = 0;

    	if (angle < 30)
    	{
    		fixedAngle = 30;
    	}
    	else if (angle > 150 && angle <= 180)
    	{
    		fixedAngle = 150;
    	}
    	else if (angle > 180 && angle <= 210)
    	{
   			fixedAngle = 210;
   		}
   		else if (angle > 330)
   		{    		
   			fixedAngle = 330;
    	}

    	return fixedAngle;
    }
}
