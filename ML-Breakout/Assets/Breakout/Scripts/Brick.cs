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

	//check for collisions and destroy brick
	void OnCollisionEnter(Collision other)
	{
		Destroy(gameObject);
	}
}
