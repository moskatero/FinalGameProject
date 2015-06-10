using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Senser : MonoBehaviour
{
	static public Senser instance;
	public GameObject actor;
	public GameObject[] agent;


	Actor actorScript;
	bool isNTrigger = true;
	Vector3 pos;
	int k = 0;

	void Awake()
	{
		instance = this;
	}

	void Start()
	{
		if (actor != null)
		{
			actorScript = (Actor)actor.GetComponent(typeof(Actor));
		}
	}

	void OnTriggerEnter(Collider other)
	{
		if(other.CompareTag("Agent"))
		{
			//agent[k] = other.gameObject;
			Debug.Log ("Help");				
			Vector3 target = new Vector3(transform.position.x, transform.position.y,transform.position.z);
			Actor.instance.MoveOrder(target);
		}
	}

	void OnTriggerStay(Collider other)
	{
		if(isNTrigger)
		{
			if(other.CompareTag("Agent"))
			{
				Vector3 target = new Vector3(transform.position.x, transform.position.y, transform.position.z);
				pos = target;
				actorScript.MoveOrder(target); 
				isNTrigger = false;
			}
		}
	} 

	public void Setfalse()
	{
		isNTrigger = true;
	}

	public Vector3 GetEnd()
	{
		return pos;
	}
}
