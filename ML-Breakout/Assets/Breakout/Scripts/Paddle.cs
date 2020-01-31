using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paddle : MonoBehaviour
{

    public float paddleSpeed = 20f;
    public float xPosLimit = 9.5f;
    public float xNegLimit = -9.5f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //if outside higher x axis range, reset back to range limit
        if (transform.position.x > xPosLimit)
            transform.position = new Vector3(xPosLimit, transform.position.y, transform.position.z);

        //if outside lower x axis range, reset back to range limit
        if (transform.position.x < xNegLimit)
            transform.position = new Vector3(xNegLimit, transform.position.y, transform.position.z);

        transform.Translate( 0f, Input.GetAxis("Horizontal") * Time.deltaTime * paddleSpeed, 0f);
    }
}
