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
		kAttack,
	}
	State state = State.kIdle;

	public bool DebugMode;
	GameObject[] senser;
	//GameObject[] agent;
	bool isEnd = false;

	int k = 0;
	bool onNode = true;
	
	float speed;
	float speedMulti = 5;

	int ran;
	int nodeIndex;

	Vector3 start;
	Vector3 target = new Vector3(0, 0, 0);
	Vector3 currNode;
	Vector3 endpos;

	//NodeControl control;

	List<Vector3> path = new List<Vector3>();

	float OldTime = 0;
	float checkTime = 0;
	float elapsedTime = 0;
	
	void Awake()
	{
		//instance = this;
		start = transform.position;
		senser = GameObject.FindGameObjectsWithTag("pointB");
		ran = Random.Range (0, senser.Length);

		//Debug.Log("length " + senser.Length);
		//control = (NodeControl)senser.GetComponent(typeof(NodeControl));
		//agent = GameObject.FindGameObjectsWithTag("Agent");

		//GameObject cam = GameObject.FindGameObjectWithTag("MainCamera");
		//GameObject cam = GameObject.FindGameObjectWithTag("pointB");
		//control = (NodeControl)cam.GetComponent(typeof(NodeControl));
	}

	void Start()
	{
		//Random.seed = 42;
		//Random.seed = 42;
		Debug.Log("ran " +ran);
		//ran = Random.Range (0, senser.Length);
		//Debug.Log("length " + senser.Length);
		endpos = senser[ran].transform.position;
		//Debug.Log("position " + endpos);
		////ChangeState (State.kMoving);
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
			}
		}
	}

	void OnTriggerEnter(Collider other)
	{
		if(other.CompareTag("Player"))
		{
			animation.CrossFade ("Run");
			Debug.Log("HI, FUCK YOU NICKY");
			Vector3 pos1 = new Vector3(other.transform.position.x, transform.position.y, other.transform.position.z);
			MoveOrder(pos1);
		}
	}

	void OnTriggerExit(Collider other)
	{
		if(other.CompareTag("Player"))
		{
			animation.CrossFade ("Walk");
			ChangeState(State.kMoving);
			MoveOrder(endpos);
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

		//infanite
		if ((Xdistance < 0.1 && Ydistance < 0.1) && target == currNode) //Reached target
		{
			if(isEnd && k != 2)
			{
				MoveOrder(start);
				isEnd = false;
				//++k;
			}
			//if (isEnd == false && k != 2)
			else 
			{
				Debug.Log("gonna move");
				//Vector3 endpos = Senser.instance.GetEnd();

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
		//Debug.Log("hi");
	}
	
	void SetTarget()
	{
		path = NodeControl.instance.Path(transform.position, target);
 		//path = control.Path(transform.position, target);
		nodeIndex = 0;
		onNode = true;
	}
	
	public void MoveOrder(Vector3 pos)
	{
		target = pos;
		SetTarget();
		ChangeState(State.kMoving);
	}
	
	void ChangeState(State newState)
	{
		state = newState;
	}

}
