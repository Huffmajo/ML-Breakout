using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{

	public float ballSpeed = 2f;
	public Vector3 ballDirection;
	public Vector3 ballVelocity;

	private float horiDirection = 1f;
	private float vertDirection = 1f;
	private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
    	// get ball rigidbody
    	rb = GetComponent<Rigidbody>();

    	// set starting velocity directions
        horiDirection = 1;
        vertDirection = 1;
        ballDirection = new Vector3(horiDirection, vertDirection, 0f);

        // start ball moving, bouncy physics material will keep it moving
        rb.velocity = ballDirection * ballSpeed;
    }

    // Update is called once per frame
    void Update()
    {
    	//ballMovement = new Vector3(hori, vert, 0f);
    	//transform.position += ballMovement;
        //rb.AddForce(new Vector3(ballSpeed, ballSpeed, 0f));

        // track velocity for interactions with bricks and paddle
        ballVelocity = rb.velocity;
    }

    void OnCollisionEnter(Collision col)
    {
    	// bounce off paddle is dependent on proximity to paddle center
    	if (col.gameObject.tag == "Paddle")
    	{
    		Debug.Log("Ball hit Paddle");
    		// insert bounce equations here once paddle prefab is available
    	}
    	// bounce off any other surface as usual
    	else
    	{
	    	horiDirection *= -1;
    		vertDirection *= -1;
    	}
    }
}
