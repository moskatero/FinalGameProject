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


	//void OnTriggerEnter(Collider other)
	//{
	//	if(other.CompareTag("Agent"))
	//	{
	//		//targets.Add(other.gameObject);
	//		Debug.Log ("HELP");
	//		//Vector3 target = new Vector3(transform.position.x, transform.position.y,transform.position.z);
	//		//Actor.instance.MoveOrder(target);
	//	}
	//}

	//void OnTriggerStay(Collider other)
	//{
	//	if(isNTrigger)
	//	{
	//		if(other.CompareTag("Agent"))
	//		{
	//			//targets.Add(other.gameObject);
	//			targets = GameObject.FindGameObjectsWithTag("Agent");
	//			foreach(GameObject target in targets)
	//			{
	//				Debug.Log ("HELP" + target);
	//				pos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
	//				Actor.instance.MoveOrder(pos);
	//			}
	//
	//			//poss[k] = new Vector3(transform.position.x, transform.position.y,transform.position.z);
	//			//Debug.Log ("HELP");
	//			//Vector3 target = new Vector3(transform.position.x, transform.position.y, transform.position.z);
	//			//pos = target;
	//			//Actor.instance.MoveOrder(target);
	//			++k;
	//			if(k == 1)
	//			{
	//				isNTrigger = false;
	//
	//			}
	//		}
	//	}
	//} 

	public void Setfalse()
	{
		isNTrigger = true;
	}

	public Vector3 GetEnd()
	{
		return pos;
	}
}
