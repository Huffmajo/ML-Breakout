using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;

public class PenguinAgent : Agent
{
	public GameObject heartPrefab;
	public GameObject regurgitatedFishPrefab;

	private PenguinArea penguinArea;
	private Animator animator;
	private RayPerception3D rayPerception;
	private GameObject baby;

	private bool isFull; // if true, penguin has a full stomach

	/// <summary>
	/// Read inputs from the keyboard and convert them to a list of actions.
	/// This is called only when the player wants to control the agent and has set
	/// Behavior Type to "Heuristic Only" in the Behavior Parameters inspector.
	/// </summary>
	/// <returns>A vectorAction array of floats that will be passed into <see cref="AgentAction(float[])"/></returns>
	public override float[] Heuristic()
	{
    	float forwardAction = 0f;
		float turnAction = 0f;

	    if (Input.GetKey(KeyCode.W))
    	{
	        // move forward
    	    forwardAction = 1f;
	    }
	    if (Input.GetKey(KeyCode.A))
	    {
	        // turn left
	        turnAction = 1f;
	    }
	    else if (Input.GetKey(KeyCode.D))
	    {
	        // turn right
	        turnAction = 2f;
	    }

	    // Put the actions into an array and return
	    return new float[] { forwardAction, turnAction };
	}

	public override void AgentAction(float[] vectorAction)
	{
		// Convert actions to axis values
		float forward = vectorAction[0];
		float leftOrRight = 0f;
		if (vectorAction[1] == 1f)
		{
			leftOrRight = -1f;
		}
		else if (vectorAction[1] == 2f)
		{
			leftOrRight = 1f;
		}

		// set animator parameters
		animator.SetFloat("Vertical", forward);
		animator.SetFloat("Horizontal", leftOrRight);

		// Tiny negative reward every step
		AddReward(-1f / agentParameters.maxStep);
	}

	public override void AgentReset()
	{
		isFull = false;
		penguinArea.ResetArea();
	}

	public override void CollectObservations()
	{
		// Has the penguin eaten
		AddVectorObs(isFull);

		// Distance to the baby
		AddVectorObs(Vector3.Distance(baby.transform.position, transform.position));

		// Direction to the baby
		AddVectorObs((baby.transform.position - transform.position).normalized);

		// Direction penguin is facing
		AddVectorObs(transform.forward);

		// RayPerception (sight)
		// ========================
		// rayDistance: how far to raycast
		// rayAngles: angles to raycast (0 is right, 90 is forward, 180 is left)
		// detectableObjects: list of tags whcih correspond to object types agent can see
		// startOffset: Starting height offset of ray from center of agent
		// end Offset: ending height offset of ray from center of agent
		float rayDistance = 20f;
		float[] rayAngles = {30f, 60f, 90f, 120f, 150f};
		string[] detectableObjects = {"baby", "fish", "wall"};
		AddVectorObs(rayPerception.Perceive(rayDistance, rayAngles, detectableObjects, 0f, 0f));
	}

	private void Start()
	{
		penguinArea = GetComponentInParent<PenguinArea>();
		baby = penguinArea.penguinBaby;
		animator = GetComponent<Animator>();
		rayPerception = GetComponent<RayPerception3D>();
	}

	private void FixedUpdate()
	{
		if (Vector3.Distance(transform.position, baby.transform.position) < penguinArea.feedRadius)
		{
			// close enough, try to feed the baby
			RegurgitateFish();
		}
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.transform.CompareTag("fish"))
		{
			EatFish(collision.gameObject);
		}
		else if (collision.transform.CompareTag("baby"))
		{
			// try to feed the baby
			RegurgitateFish();
		}
	}

	private void EatFish(GameObject fishObject)
	{
		if (isFull) return; // can't eat another fish while full
		isFull = true;

		penguinArea.RemoveSpecificFish(fishObject);

		// reward for eaing fish
		AddReward(1f);
	}

	private void RegurgitateFish()
	{
		if (!isFull) return; // nothing to regurgitate
		isFull = false;

		// spawn regurgitated fish
		GameObject regurgitatedFish = Instantiate<GameObject>(regurgitatedFishPrefab);
		regurgitatedFish.transform.parent = transform.parent;
		regurgitatedFish.transform.position = baby.transform.position;
		Destroy(regurgitatedFish, 4f);

		// spawn heart
		GameObject heart = Instantiate<GameObject>(heartPrefab);
		heart.transform.parent = transform.parent;
		heart.transform.position = baby.transform.position + Vector3.up;
		Destroy(heart, 4f);

		AddReward(1f);
	}
}
