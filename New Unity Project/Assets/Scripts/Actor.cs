using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Actor : MonoBehaviour {
	
	enum State
	{
		kIdle,
		kMoving,
	}
	
	float m_speed;
	float m_speed_multi = 5;
	public bool DebugMode;

	Vector3 startpos;
	Vector3 pointBpos;
	
	bool onNode = true;
	Vector3 m_target = new Vector3(0, 0, 0);
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
		startpos = gameObject.transform.position;
		GameObject pointB = GameObject.FindGameObjectWithTag("pointB");
		control = (NodeControl)pointB.GetComponent(typeof(NodeControl));
	}
	
	void Update () 
	{
		m_speed = Time.deltaTime * m_speed_multi;
		elapsedTime += Time.deltaTime;
		
		if (elapsedTime > OldTime)
		{
			switch (state)
			{
			case State.kIdle:
				break;
				
			case State.kMoving:
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
				break;
			}
		}
	}
	
	void MoveToward()
	{
		if (DebugMode)
		{
			for (int i=0; i<path.Count-1; ++i)
			{
				Debug.DrawLine((Vector3)path[i], (Vector3)path[i+1], Color.white, 0.01f);
			}
		}
		
		Vector3 newPos = transform.position;

		float Xdistance = newPos.x - currNode.x;
		if (Xdistance < 0) Xdistance -= Xdistance*2;
		float Ydistance = newPos.z - currNode.z;
		if (Ydistance < 0) Ydistance -= Ydistance*2;
	
		if ((Xdistance < 0.1 && Ydistance < 0.1) && m_target == currNode) //Reached target
		{
			ChangeState(State.kIdle);
		}
		else if (Xdistance < 0.1 && Ydistance < 0.1)
		{
			nodeIndex++;
			onNode = true;
		}
		
		/***Move toward waypoint***/
		Vector3 motion = currNode - newPos;
		motion.Normalize();
		newPos += motion * m_speed;
		
		transform.position = newPos;
	}
	
	private void SetTarget()
	{
		path = control.Path(transform.position, m_target);
		nodeIndex = 0;
		onNode = true;
	}
	
	public void MoveOrder(Vector3 pos)
	{
		m_target = pos;
		SetTarget();
		ChangeState(State.kMoving);
	}
	
	private void ChangeState(State newState)
	{
		state = newState;
	}

	void OnTriggerEnter(Collider other)
	{
		if(other.CompareTag("pointB"))
		{
			pointBpos = other.transform.position;
			MoveOrder(pointBpos);
		}
	}

}
