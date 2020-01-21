using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{

	public float ballSpeed = 2f;
	public Vector3 ballMovement;

	private float hori = 0.25f;
	private float vert = 0.25f;
	private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
    	rb = GetComponent<Rigidbody>();
        hori = 1;
        vert = 1;
        ballMovement = new Vector3(hori, vert, 0f);
        rb.AddForce(600f, 600f, 0);
    }

    // Update is called once per frame
    void Update()
    {
    	//ballMovement = new Vector3(hori, vert, 0f);
    	//transform.position += ballMovement;
        //rb.AddForce(new Vector3(ballSpeed, ballSpeed, 0f));
    }

    void OnCollisionEnter(Collision col)
    {
    	hori *= -1;
    	vert *= -1;
    }
}
