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
    	rb = GetComponent<Rigidbody>();
        horiDirection = 1;
        vertDirection = 1;
        ballDirection = new Vector3(horiDirection, vertDirection, 0f);

        rb.velocity = ballDirection * ballSpeed;
    }

    // Update is called once per frame
    void Update()
    {
    	//ballMovement = new Vector3(hori, vert, 0f);
    	//transform.position += ballMovement;
        //rb.AddForce(new Vector3(ballSpeed, ballSpeed, 0f));
        ballVelocity = rb.velocity;
    }

    void OnCollisionEnter(Collision col)
    {
    	horiDirection *= -1;
    	vertDirection *= -1;
    }
}
