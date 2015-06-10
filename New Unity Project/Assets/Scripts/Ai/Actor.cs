using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Actor : MonoBehaviour 
{
	static public Actor instance;
	
	enum State
	{
		kIdle,
		kMoving,
	}
	
	float speed;
	float speedMulti = 5;
	public bool DebugMode;

	Vector3 start;
	bool isEnd = false;

	bool onNode = true;
	Vector3 target = new Vector3(0, 0, 0);
	Vector3 currNode;
	int nodeIndex;
	List<Vector3> path = new List<Vector3>();
	NodeControl control;
	State state = State.kIdle;
	float OldTime = 0;
	float checkTime = 0;
	float elapsedTime = 0;
	
	void Awake()
	{
		instance = this;
		start = transform.position;
		//GameObject cam = GameObject.FindGameObjectWithTag("MainCamera");
		//control = (NodeControl)cam.GetComponent(typeof(NodeControl));
		GameObject cam = GameObject.FindGameObjectWithTag("pointB");
		control = (NodeControl)cam.GetComponent(typeof(NodeControl));
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
							currNode = path[nodeIndex];
					} else
						MoveToward();
				}
			}
				break;
			}
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
		
		if ((Xdistance < 0.1 && Ydistance < 0.1) && target == currNode) //Reached target
		{
			if(isEnd)
			{
				MoveOrder(start);
				isEnd = false;
			}
			else
			{
				Vector3 endpos = Senser.instance.GetEnd();
				MoveOrder(endpos);
				isEnd = true;
			}
			//ChangeState(State.kIdle);
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
	}
	
	private void SetTarget()
	{
		path = control.Path(transform.position, target);//problem
		nodeIndex = 0;
		onNode = true;
	}
	
	public void MoveOrder(Vector3 pos)
	{
		target = pos;
		SetTarget();
		ChangeState(State.kMoving);
	}
	
	private void ChangeState(State newState)
	{
		state = newState;
	}

}
