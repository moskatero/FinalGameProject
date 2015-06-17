using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

public class Senser : MonoBehaviour
{
	static public Senser instance;


	GameObject[] targets;
	Vector3[] poss;

	bool isNTrigger = true;
	Actor actorScript;
	Vector3 pos;

	void Awake()
	{
		instance = this;
		
		targets = GameObject.FindGameObjectsWithTag("Agent");			
	}

	void Update()
	{
		if(isNTrigger)
		{
			for(int i = 0; i < targets.Length; ++i)
			{
				actorScript = (Actor)targets[i].gameObject.GetComponent(typeof(Actor));
				Debug.Log ("HELP" + targets[i]);
				pos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
				actorScript.MoveOrder(pos);
				//Actor.instance.MoveOrder(pos);
			}
			isNTrigger = false;
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
