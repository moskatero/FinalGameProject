using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NodeControl : MonoBehaviour 
{
	static public NodeControl instance;
	public string nodeTag;

	void Awake()
	{
		instance = this;
	}
	enum states
	{
		ku,
		ko,
		ke,
		ks,
		kc,
	};

	class Point
	{
		Vector3 mPos;
		int mState = (int)states.ku;
		float mScore = 0;
		Point mPrevPoint;
		
		List<Point> mConnectedPoints = new List<Point>();
		List<Point> mPotentialPrevPoints = new List<Point>();
		
		public Point(Vector3 pos, int state = (int)states.ku)
		{
			mPos = pos;
			mState = state;
		}
		
		public int GetState()
		{
			return mState;
		}
		
		public Vector3 GetPos()
		{
			return mPos;
		}
		
		public List<Point> GetConnectedPoints()
		{
			return mConnectedPoints;
		}
		
		public Point GetPrevPoint()
		{
			return mPrevPoint;
		}
		
		public float GetScore()
		{
			return mScore;
		}
		
		public List<Point> GetPotentialPrevPoints()
		{
			return mPotentialPrevPoints;
		}
		
		public void AddConnectedPoint(Point point)
		{
			mConnectedPoints.Add(point);
		}
		
		public void AddPotentialPrevPoint(Point point)
		{
			mPotentialPrevPoints.Add(point);
		}
		
		public void SetPrevPoint(Point point)
		{
			mPrevPoint = point;
		}
		
		public void SetState(int newState)
		{
			mState = newState;
		}
		
		public void SetScore(float newScore)
		{
			mScore = newScore;
		}
	}
	
	public List<Vector3> Path(Vector3 startPos, Vector3 targetPos)
	{
		//Can I see the exit
		float exitDistance = Vector3.Distance(startPos, targetPos);

		if (exitDistance > .7f)
		{
			exitDistance -= .7f;
		}

		if (!Physics.Raycast(startPos, targetPos - startPos, exitDistance))
		{
			List<Vector3> path = new List<Vector3>();
			path.Add(startPos);
			path.Add(targetPos);
			return path;
		}
		
		GameObject[] nodes = GameObject.FindGameObjectsWithTag(nodeTag);
		List<Point> points = new List<Point>();

		for (int i = 0; i < nodes.Length; ++i)
		{
			Point currNode = new Point(nodes[i].transform.position);
			points.Add(currNode);
		}
		
		Point endPoint = new Point(targetPos, (int)states.ke);
		
		/***Connect them together***/
		foreach(Point point in points) //Could be optimized to not go through each connection twice
		{
			foreach (Point point2 in points)
			{
				float distance = Vector3.Distance(point.GetPos(), point2.GetPos());
				if (!Physics.Raycast(point.GetPos(), point2.GetPos() - point.GetPos(), distance))
				{
					//Debug.DrawRay(point.GetPos(), point2.GetPos() - point.GetPos(), Color.white, 1);
					point.AddConnectedPoint(point2);
				}
			}
			float distance2 = Vector3.Distance(targetPos, point.GetPos());
			if (!Physics.Raycast(targetPos, point.GetPos() - targetPos, distance2))
			{
				//Debug.DrawRay(targetPos, point.GetPos() - targetPos, Color.white, 1);
				point.AddConnectedPoint(endPoint);
			}
		}
		
		//points startPos can see
		foreach (Point point in points)
		{
			float distance = Vector3.Distance(startPos, point.GetPos());
			if (!Physics.Raycast(startPos, point.GetPos() - startPos, distance))
			{
				//Debug.DrawRay(startPos, point.GetPos() - startPos, Color.white, 1);
				Point startPoint = new Point(startPos, (int)states.ks);
				point.SetPrevPoint(startPoint);
				point.SetState((int)states.ko);
				point.SetScore(distance + Vector3.Distance(targetPos, point.GetPos()));
			}
		}
		
		//Go through until we find the exit or run out of connections
		bool searchedAll = false;
		bool foundEnd = false;
		
		while(!searchedAll)
		{
			searchedAll = true;
			List<Point> foundConnections = new List<Point>();
			foreach (Point point in points)
			{
				if (point.GetState() == (int)states.ko)
				{
					searchedAll = false;
					List<Point> potentials = point.GetConnectedPoints();
					
					foreach (Point potentialPoint in potentials)
					{
						if (potentialPoint.GetState() == (int)states.ku)
						{
							potentialPoint.AddPotentialPrevPoint(point);
							foundConnections.Add(potentialPoint);
							potentialPoint.SetScore(Vector3.Distance(startPos, potentialPoint.GetPos()) + Vector3.Distance(targetPos, potentialPoint.GetPos()));
						} 

						else if (potentialPoint.GetState() == (int)states.ke)
						{
							//Found the exit
							foundEnd = true;
							endPoint.AddConnectedPoint(point);
						}
					}
					point.SetState((int)states.kc);
				}
			}

			foreach (Point connection in foundConnections)
			{
				connection.SetState((int)states.ko);

				//Find lowest scoring prev point
				float lowestScore = 0;
				Point bestPrevPoint = null;
				bool first = true;

				foreach (Point prevPoints in connection.GetPotentialPrevPoints())
				{
					if (first)
					{
						lowestScore = prevPoints.GetScore();
						bestPrevPoint = prevPoints;
						first = false;
					} 

					else
					{
						if (lowestScore > prevPoints.GetScore())
						{
							lowestScore = prevPoints.GetScore();
							bestPrevPoint = prevPoints;
						}
					}
				}

				connection.SetPrevPoint(bestPrevPoint);
			}
		}
		
		if (foundEnd)
		{
			//trace back finding shortest route (lowest score)
			List<Point> shortestRoute = null;
			float lowestScore = 0;
			bool firstRoute = true;

			foreach (Point point in endPoint.GetConnectedPoints())
			{
				float score = 0;
				bool tracing = true;
				Point currPoint = point;
				List<Point> route = new List<Point>();

				route.Add(endPoint);

				while(tracing)
				{
					route.Add(currPoint);

					if (currPoint.GetState() == (int)states.ks)
					{
						if (firstRoute)
						{
							shortestRoute = route;
							lowestScore = score;
							firstRoute = false;
						} 

						else
						{
							if (lowestScore > score)
							{
								shortestRoute = route;
								lowestScore = score;
							}
						}

						tracing = false;
						break;
					}
					score += currPoint.GetScore();
					currPoint = currPoint.GetPrevPoint();
				}
			}
			
			shortestRoute.Reverse();
			List<Vector3> path = new List<Vector3>();

			foreach (Point point in shortestRoute)
			{
				path.Add(point.GetPos());
			}

			return path;
		} 

		else
		{
			return null;
		}
	}
}