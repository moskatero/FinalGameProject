using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Actor : MonoBehaviour 
{
	//public Actor instance;

	enum State
	{
		kIdle,
		kMoving,
		kDead,
	}
	State state = State.kIdle;

	public bool DebugMode;
	GameObject[] senser;
	GameObject target;
	GameObject targetorg;
	//GameObject[] agent;
	bool isEnd = false;
	bool isAttack = false;
	bool onNode = true;
	
	float speed;
	float speedMulti = 5;

	int ran;
	int nodeIndex;

	Vector3 start;
	Vector3 startpos;
	Vector3 targetpos = new Vector3(0, 0, 0);
	Vector3 currNode;
	Vector3 end;
	Vector3 endpos;

	//NodeControl control;

	List<Vector3> path = new List<Vector3>();

	float OldTime = 0;
	float checkTime = 0;
	float elapsedTime = 0;
	
	void Awake()
	{
		senser = GameObject.FindGameObjectsWithTag("pointB");
		ran = Random.Range (0, senser.Length);

	}

	void Start()
	{
		Debug.Log("ran " +ran);
		end = senser[ran].transform.position;
		endpos = end;
		start = gameObject.transform.position;
		startpos = start;
		targetorg = senser [ran].gameObject; 
		target = targetorg;
		MoveOrder (endpos);
	}
	
	void Update () 
	{
		speed = Time.deltaTime * speedMulti;
		elapsedTime += Time.deltaTime;
		
		if (elapsedTime > OldTime)
		{
			switch (state)
			{
				case State.kIdle:
				{
					animation.CrossFade ("Idle");
				}
				break;
					
				case State.kMoving:
				{	
					OldTime = elapsedTime + 0.01f;
					
					if (elapsedTime > checkTime)
					{
						checkTime = elapsedTime + 1;
						SetTarget();
					}
					
					if (path != null)
					{
						if (onNode)
						{
							onNode = false;
							if (nodeIndex < path.Count)
							{
								currNode = path[nodeIndex];
							}
						} else
							MoveToward();
					}
				}
				break;
			case State.kDead:
			{
				animation.CrossFade ("Die");
				OldTime = elapsedTime + 0.05f;
				
				if (elapsedTime > checkTime)
				{
					DestroyObject(gameObject);
				}

			}
			break;
			}
		}
	}

	//void OnCollisionEnter(Collision collision)
	//{
	//	if(collision.collider.CompareTag("Player"))
	//	{
	//		animation.CrossFade ("Attack");
	//		startpos = new Vector3(collision.transform.position.x, transform.position.y, collision.transform.position.z);
	//		endpos = startpos;
	//	}
	//}

	void OnTriggerEnter(Collider other)
	{
		if(other.CompareTag("Player"))
		{
			animation.CrossFade ("Run");
			target = other.gameObject;
			targetpos = target.transform.position;
			startpos = new Vector3(other.transform.position.x, transform.position.y, other.transform.position.z);
			endpos = startpos;
			MoveOrder(startpos);
		}
	}

	void OnTriggerStay(Collider other)
	{
		if(other.CompareTag("Player"))
		{
			animation.CrossFade ("Run");
			target = other.gameObject;
			targetpos = target.transform.position;
			startpos = new Vector3(other.transform.position.x, transform.position.y, other.transform.position.z);
			endpos = startpos;
		}
	}

	void OnTriggerExit(Collider other)
	{
		if(other.CompareTag("Player"))
		{
			animation.CrossFade ("Walk");
			ChangeState(State.kMoving);
			target = targetorg;
			targetpos = target.transform.position;
			startpos = start;
			endpos = end;
			MoveOrder(startpos);
		}
	}

	void MoveToward()
	{
		if (DebugMode)
		{
			for (int i=0; i < path.Count-1; ++i)
			{
				Debug.DrawLine((Vector3)path[i], (Vector3)path[i+1], Color.white, 0.01f);
			}
		}
		
		Vector3 newPos = transform.position;
		
		float Xdistance = newPos.x - currNode.x;
		if (Xdistance < 0) Xdistance -= Xdistance*2;
		float Ydistance = newPos.z - currNode.z;
		if (Ydistance < 0) Ydistance -= Ydistance*2;
	

		if ((Xdistance < 0.1 && Ydistance < 0.1) && targetpos == currNode) //Reached target
		{

			if(isEnd)
			{
				MoveOrder(startpos);
				isEnd = false;
				Debug.Log("screw AI");
			}
			else 
			{
				Debug.Log("gonna move");
				MoveOrder(endpos);
				isEnd = true;
			}
		}

		else if (Xdistance < 0.1 && Ydistance < 0.1)
		{
			nodeIndex++;
			onNode = true;
		}
		
		/***Move toward waypoint***/
		Vector3 motion = currNode - newPos;
		motion.Normalize();
		newPos += motion * speed;
		
		transform.position = newPos;

		Vector3 relativePos = target.transform.position - transform.position;
		Quaternion rotation = Quaternion.LookRotation(relativePos);
		transform.rotation = rotation;
	}
	
	void SetTarget()
	{
		path = NodeControl.instance.Path(transform.position, targetpos);
 		//path = control.Path(transform.position, target);
		nodeIndex = 0;
		onNode = true;
	}
	
	public void MoveOrder(Vector3 pos)
	{
		targetpos = pos;
		SetTarget();
		ChangeState(State.kMoving);
	}
	
	void ChangeState(State newState)
	{
		state = newState;
	}

}
