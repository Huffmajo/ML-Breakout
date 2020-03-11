//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;


public class Ball : MonoBehaviour
{
	// Game Objects
	public GameObject paddle;
	public PlayerGM gm;
	public AIGM aiGM;
	private Rigidbody rb;
    private ParticleSystem ps;

	// Variables
	public bool firstLaunch;
	public bool AIBall;
	public bool heldByPaddle;
	public bool training = false;
	public float ballSpeed = 20f;
	public float totalVelocity;
	public float velocityAngle;
	public float launchAngle;

	// Vector3s
	public Vector3 ballImpactVector;
	public Vector3 ballVelocity;


    void Start()
    {
    	// get ball rigidbody and particle system
    	rb = GetComponent<Rigidbody>();

        if (!training)
        {
            ps = gameObject.transform.Find("Particle System").GetComponent<ParticleSystem>();
            ps.Stop();
        }

		// firstLaunch is set to true to prevent game from starting before player launches the ball
		if (training){
			firstLaunch = false;
		} else {
			firstLaunch = true;
		}
    	ResetBall();
    }

    void Update()
    {
    	// get input for ball release
    	bool releaseButton = Input.GetButtonDown("Jump");

    	if (heldByPaddle)
    	{
            // trail while held by paddle
            if (!training)
            {
                GetComponent<TrailRenderer>().Clear();    
            }
            
			//move ball with the paddle
    		transform.position = new Vector3(paddle.transform.position.x, paddle.transform.position.y + 2, paddle.transform.position.z);

    		// release ball from user input
    		if (releaseButton)
    		{
    			heldByPaddle = false;
    	    	LaunchBall(ballSpeed, launchAngle);
    		}
    	}
    	else //ball is in play
    	{
			// track velocity for interactions with bricks and paddle
			ballVelocity = rb.velocity;
			totalVelocity = Mathf.Abs(rb.velocity.x) + Mathf.Abs(rb.velocity.y);

			// get angle of ball's vector, 0-359°
			velocityAngle = Mathf.Atan2(ballVelocity.y, ballVelocity.x) * Mathf.Rad2Deg;
			if (velocityAngle < 0)
			{
				velocityAngle += 360;
			}

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
    		if (!training)
    		{
    			FindObjectOfType<AudioManager>().Play("Bounce");
    		}
            else
            {
                paddle.GetComponent<PaddleAgent>().AddReward(1f);
            }

    		// get impact vector, convert to launch angle, launch
    		ballImpactVector = paddle.transform.InverseTransformPoint(col.contacts[0].point); 
	    	launchAngle = CalculateLaunchAngle(ballImpactVector.x);
    		LaunchBall(ballSpeed, launchAngle);
    	}
    	else if (col.gameObject.tag == "brick")
    	{
	    	// play bounce sound
	    	if (!training)
    		{
    			FindObjectOfType<AudioManager>().Play("Pop");

                // emit particles when hitting brick
                var emitParams = new ParticleSystem.EmitParams();
                emitParams.startColor = col.gameObject.GetComponent<Renderer>().material.color;
                emitParams.startSize = 0.25f;
                emitParams.velocity = col.gameObject.transform.position - transform.position;
                gameObject.transform.Find("Particle System").GetComponent<ParticleSystem>().Emit(emitParams, 10);
    		} 	

		}
    	else if (col.gameObject.tag != "ground")
    	{
    		if (!training)
    		{
    			FindObjectOfType<AudioManager>().Play("Bounce");
    		}
    	}
        else if (col.gameObject.tag == "ground")
        {
            if (!training)
            {
                // emit particles on death
                var emitParams = new ParticleSystem.EmitParams();
                emitParams.startColor = gameObject.GetComponent<Renderer>().material.color;
                emitParams.startSize = 0.5f;
                gameObject.transform.Find("Particle System").GetComponent<ParticleSystem>().Emit(emitParams, 30);

                if (AIBall)
                {
                    aiGM.LoseLife();
                } 
                else
                {
                    gm.LoseLife();
                }

                ResetBall();
            }
            else
            {
                paddle.GetComponent<PaddleAgent>().Done();
            }
        }
    }


    // resets local variables and location of the ball
    public void ResetBall()
    {
    	launchAngle = 45f;

        // ball is docked to paddle
        heldByPaddle = true;
        transform.position = new Vector3(paddle.transform.position.x, paddle.transform.position.y + 2, paddle.transform.position.z);

        if (!training)
        {
            // remove trail
            GetComponent<TrailRenderer>().Clear();
        }
    }


    // returns launch angle based on where the paddle is impacted
    float CalculateLaunchAngle(float impactPos)
    {
    	return 90 - (12 * impactPos);
    }


    // launches ball at specified speed and angle
    public void LaunchBall(float speed, float angle)
    {
		if (firstLaunch && !AIBall)
		{
			Time.timeScale = 1;
			firstLaunch = false;
			gm.masterGM.aiGM.aiBall.firstLaunch = false;
			gm.masterGM.gameStarted = true;
		}

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
