using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collision)
    {
    	if (collision.gameObject.tag == "ball")
    	{
    		Debug.Log("Ball touched brick");
    		Destroy(this.gameObject);
    	}
    }
}
